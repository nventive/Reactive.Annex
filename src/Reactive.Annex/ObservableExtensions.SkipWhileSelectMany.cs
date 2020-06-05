using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
	public static partial class ObservableExtensions
	{
		/// <summary>
		/// Projects element of an observable sequence to an observable sequence, 
		/// skip new elements while resulting observable was not completed, 
		/// and merges the resulting observable sequences into one observable sequence.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <param name="includePending">Determine if, when a resulting observable complete, a new "resulting observable" should be created with last "skipped" value.</param>
		/// <returns></returns>
		public static IObservable<Unit> SkipWhileSelectMany<TSource>(this IObservable<TSource> source, Func<CancellationToken, TSource, Task> selector, bool includePending = true)
		{
			return source.SkipWhileSelectMany(v => FromAsync(ct => selector(ct, v)), includePending);
		}

		/// <summary>
		/// Projects element of an observable sequence to an observable sequence, 
		/// skip new elements while resulting observable was not completed, 
		/// and merges the resulting observable sequences into one observable sequence.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <param name="includePending">Determine if, when a resulting observable complete, a new "resulting observable" should be created with last "skipped" value.</param>
		/// <returns></returns>
		public static IObservable<TResult> SkipWhileSelectMany<TSource, TResult>(this IObservable<TSource> source, Func<CancellationToken, TSource, Task<TResult>> selector, bool includePending = true)
		{
			return source.SkipWhileSelectMany(v => FromAsync(ct => selector(ct, v)), includePending);
		}

		/// <summary>
		/// Projects element of an observable sequence to an observable sequence, 
		/// skip new elements while resulting observable was not completed, 
		/// and merges the resulting observable sequences into one observable sequence.
		/// </summary>
		/// <typeparam name="TSource"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="source"></param>
		/// <param name="selector"></param>
		/// <param name="includePending">Determine if, when a resulting observable complete, a new "resulting observable" should be created with last "skipped" value.</param>
		/// <returns></returns>
		public static IObservable<TResult> SkipWhileSelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector, bool includePending = true)
		{
			return Observable
				.Defer(() =>
				{
					var gate = new object();
					var isWorking = false;

					if (includePending)
					{
						var hasPending = false;
						TSource pending = default(TSource);

						Func<TSource, IObserver<TResult>, IDisposable> subscribe = null;
						subscribe = (t, observer) =>
						{
							var disposable = new SerialDisposable();
							disposable.Disposable = selector(t).Subscribe(
								observer.OnNext,
								observer.OnError,
								() =>
								{
									bool localHasPending;
									TSource localPending;
									lock (gate)
									{
										localHasPending = hasPending;
										localPending = pending;

										if (localHasPending)
										{
											hasPending = false;
											pending = default(TSource);
										}
										else
										{
											isWorking = false;
										}
									}

									if (localHasPending)
									{
										disposable.Disposable = subscribe(localPending, observer);
									}
									else
									{
										observer.OnCompleted();
										disposable.Disposable = null;
									}
								});

							return disposable;
						};

						return source
							.SelectMany(t => Observable.Create<TResult>(observer =>
							{
								bool localIsWorking;
								lock (gate)
								{
									localIsWorking = isWorking;
									if (localIsWorking)
									{
										hasPending = true;
										pending = t;
									}
									else
									{
										isWorking = true;
									}
								}

								if (localIsWorking)
								{
									observer.OnCompleted();
									return Disposable.Empty;
								}
								else
								{
									return subscribe(t, observer);
								}
							}));
					}
					else
					{
						return source
							.Where(_ =>
							{
								lock (gate)
								{
									if (isWorking)
									{
										return false;
									}
									else
									{
										isWorking = true;
										return true;
									}
								}
							})
							.SelectMany(t => selector(t).Finally(() => isWorking = false));
					}
				});
		}
	}
}
