using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
	public static partial class ObservableExtensions
	{
		/// <summary>
		/// Create a task from an IObservable with the first value observed.
		/// </summary>
		/// <remarks>
		/// Observable producing NO VALUES will result in an exception.
		/// To prevent this, use .FirstOrDefaultAsync() instead.
		/// </remarks>
		public static Task<T> FirstAsync<T>(this IObservable<T> source, CancellationToken ct)
		{
			var tcs = new TaskCompletionSource<T>();

			var subscription = new SingleAssignmentDisposable();

			var ctr = default(CancellationTokenRegistration);
			if (ct.CanBeCanceled)
			{
				ctr = ct
					.Register(() =>
					{
						tcs.TrySetCanceled();
						subscription.Dispose();
					});
			}

			subscription.Disposable =
				source
					.Finally(ctr.Dispose) // no null-check needed (struct)
					.Subscribe(
						next =>
						{
							tcs.TrySetResult(next);
							subscription.Dispose();
						},
						exception => tcs.TrySetException(exception),
						() =>
						{
							if (!tcs.Task.IsCompleted)
							{
								tcs.TrySetException(new InvalidOperationException("Sequence contains no elements."));
							}
						});

			return tcs.Task;
		}
	}
}
