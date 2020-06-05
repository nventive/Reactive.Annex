using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
	public static partial class ObservableExtensions
	{
		/// <summary>
		/// A SelectMany who ensure to immediately dispose the previous SelectMany result on a new value.
		/// </summary>
		public static IObservable<TResult> SelectManyDisposePrevious<TSource, TResult>(
			this IObservable<TSource> source,
			Func<TSource, CancellationToken, Task<TResult>> selector,
			IScheduler scheduler = null)
		{
			return SelectManyDisposePrevious(
				source,
				v => FromAsync(ct => selector(v, ct), scheduler)
			);
		}

		/// <summary>
		/// A SelectMany who ensure to immediately dispose the previous SelectMany result on a new value.
		/// </summary>
		public static IObservable<Unit> SelectManyDisposePrevious<TSource>(
			this IObservable<TSource> source,
			Func<TSource, CancellationToken, Task> selector,
			IScheduler scheduler = null)
		{
			return SelectManyDisposePrevious(
				source,
				v => FromAsync(ct => selector(v, ct), scheduler)
			);
		}

		/// <summary>
		/// This is a special SelectMany who ensure to immediately dispose the previous SelectMany result on a new value.
		/// </summary>
		public static IObservable<TResult> SelectManyDisposePrevious<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
		{
			return Observable.Create<TResult>(
				observer =>
				{
					var serialDisposable = new SerialDisposable();
					var gate = new object();
					var isWorking = false; // We are currently observing a child
					var isCompleted = false; // The parent observable has completed

					var disposable = source.Subscribe(
						next =>
						{
							isWorking = true;
							serialDisposable.Disposable = null;
							var projectedSource = selector(next);
							serialDisposable.Disposable = projectedSource
								.Subscribe(
									observer.OnNext,
									observer.OnError,
									() =>
									{
										lock (gate)
										{
											isWorking = false;
											if (isCompleted)
											{
												observer.OnCompleted();
											}
										}
									}
								);
						},
						observer.OnError,
						() =>
						{
							lock (gate)
							{
								isCompleted = true;
								if (!isWorking)
								{
									observer.OnCompleted();
								}
							}
						});

					return new CompositeDisposable(disposable, serialDisposable).Dispose;
				});
		}
	}
}
