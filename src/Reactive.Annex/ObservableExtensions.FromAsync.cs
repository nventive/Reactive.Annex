using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
	/// <summary>
	/// Provides a set of static methods for writing in-memory queries over observable sequences.
	/// </summary>
	public static partial class ObservableExtensions
	{
		/// <summary>
		/// Converts to asynchronous function into an observable sequence. Each subscription to the resulting sequence causes the function to be started.
		/// The CancellationToken passed to the asynchronous function is tied to the observable sequence's subscription that triggered the function's invocation and can be used for best-effort cancellation.
		/// </summary>
		/// <remarks>
		/// This operator is the same as <see cref="Observable.FromAsync{TResult}(System.Func{System.Threading.Tasks.Task{TResult}})"/> except that it 
		/// ensure to run synchronously the completion instead of running it on an uncontrolled context (i.e. TaskPool).
		/// </remarks>
		/// <typeparam name="T">The type of the result returned by the asynchronous function.</typeparam>
		/// <param name="factory">Asynchronous function to convert.</param>
		/// <param name="scheduler">Scheduler</param>
		/// <returns>An observable sequence exposing the result of invoking the function, or an exception.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
		/// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous function will be signaled.</remarks>
		internal static IObservable<T> FromAsync<T>(Func<CancellationToken, Task<T>> factory, IScheduler scheduler = null)
		{
			// The issue with Observable.FromAsync() is that the observer.OnNext() which will complete the task is 
			// executed in a ContinueWith, i.e. using an unknown SynchronizationContext (Default TaskPool), so all 
			// subscribers will receive and handle the value with an invalid context.

			var o = Observable.Create<T>(async (observer, ct) =>
			{
				try
				{
					var result = await factory(ct);
					observer.OnNext(result);
					observer.OnCompleted();
				}
				catch (Exception e)
				{
					observer.OnError(e);
				}
			});

			o = scheduler != null ? o.SubscribeOn(scheduler) : o;

			return o;
		}

		/// <summary>
		/// Converts to asynchronous action into an observable sequence. Each subscription to the resulting sequence causes the function to be started.
		/// The CancellationToken passed to the asynchronous function is tied to the observable sequence's subscription that triggered 
		/// the function's invocation and can be used for best-effort cancellation.
		/// </summary>
		/// <remarks>
		/// This operator is the same as <see cref="Observable.FromAsync(System.Func{System.Threading.Tasks.Task})"/> except that it 
		/// ensure to run synchronously the completion instead of running it on an uncontrolled context (i.e. TaskPool).
		/// </remarks>
		/// <param name="factory">Asynchronous function to convert.</param>
		/// <param name="scheduler">Scheduler</param>
		/// <returns>An observable sequence exposing the result of invoking the function, or an exception.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
		/// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous function will be signaled.</remarks>
		internal static IObservable<Unit> FromAsync(Func<CancellationToken, Task> factory, IScheduler scheduler = null)
		{
			factory = factory ?? throw new ArgumentNullException(nameof(factory));

			// The issue with Observable.FromAsync() is that the observer.OnNext() which will complete the task is 
			// executed in a ContinueWith, i.e. using an unknown SynchronizationContext (Default TaskPool), so all 
			// subscribers will receive and handle the value with an invalid context.

			var o = Observable.Create<Unit>(async (observer, ct) =>
			{
				try
				{
					await factory(ct);
					observer.OnNext(Unit.Default);
					observer.OnCompleted();
				}
				catch (Exception e)
				{
					observer.OnError(e);
				}
			});

			o = scheduler != null ? o.SubscribeOn(scheduler) : o;

			return o;
		}


		/// <summary>
		/// Converts to asynchronous action into an observable sequence. Each subscription to the resulting sequence causes the action to be started.
		/// The CancellationToken passed to the asynchronous action is tied to the observable sequence's subscription that triggered the action's invocation and can be used for best-effort cancellation.
		/// </summary>
		/// <remarks>
		/// This operator is the same as <see cref="Observable.FromAsync{TResult}(System.Func{System.Threading.Tasks.Task{TResult}})"/> except that it 
		/// ensure to run synchronously the completion instead of running it on an uncontrolled context (i.e. TaskPool).
		/// </remarks>
		/// <param name="factory">Asynchronous action to convert.</param>
		/// <returns>An observable sequence exposing a Unit value upon completion of the action, or an exception.</returns>
		/// <remarks>When a subscription to the resulting sequence is disposed, the CancellationToken that was fed to the asynchronous function will be signaled.</remarks>
		/// <exception cref="ArgumentNullException"><paramref name="factory"/> is null.</exception>
		internal static IObservable<Unit> FromAsync(Func<CancellationToken, Task> factory)
		{
			// The issue with Observable.FromAsync() is that the observer.OnNext() which will complete the task is 
			// executed in a ContinueWith, i.e. using an unknown SynchronizationContext (Default TaskPool), so all 
			// subscribers will receive and handle the value with an invalid context.

			return Observable.Create<Unit>(async (observer, ct) =>
			{
				try
				{
					await factory(ct);
					observer.OnNext(Unit.Default);
					observer.OnCompleted();
				}
				catch (Exception e)
				{
					observer.OnError(e);
				}
			});
		}
	}
}
