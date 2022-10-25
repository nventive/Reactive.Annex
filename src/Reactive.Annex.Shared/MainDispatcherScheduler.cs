#if __ANDROID__ || __IOS__ || __WASM__ || WINDOWS
using System;
using System.Reactive.Disposables;
#if WINUI
using Microsoft.UI.Dispatching;
#else
using Windows.UI.Core;
#endif

namespace System.Reactive.Concurrency
{
	public partial class MainDispatcherScheduler : IDispatcherScheduler
	{
#if WINUI
		private readonly DispatcherQueue _dispatcher;
		private readonly DispatcherQueuePriority _priority;
#else
		private readonly CoreDispatcher _dispatcher;
		private readonly CoreDispatcherPriority _priority;
#endif
		public MainDispatcherScheduler(

#if WINUI
		DispatcherQueue dispatcher
#else
		CoreDispatcher dispatcher
#endif
			)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
#if WINUI
			_priority = DispatcherQueuePriority.Normal;
#else
			_priority = CoreDispatcherPriority.Normal;
#endif
		}

		public MainDispatcherScheduler(
#if WINUI
			DispatcherQueue dispatcher,
			DispatcherQueuePriority priority
#else
			CoreDispatcher dispatcher,
			CoreDispatcherPriority priority
#endif
			)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			_priority = priority;
		}

		public DateTimeOffset Now => DateTimeOffset.Now;

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
		{
			var subscription = new SerialDisposable();
			var d = new CancellationDisposable();

			_dispatcher.
#if WINUI
				TryEnqueue
#else
				RunAsync
#endif
				(
				_priority, () =>
				{
					if (!subscription.IsDisposed)
					{
						subscription.Disposable = action(this, state);
					}
#if WINUI
				});
#else
				})
				.AsTask(d.Token);
#endif
			return new CompositeDisposable(subscription, d);
		}

		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action) =>
			ScheduleCore(state, dueTime - Now, action);

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action) =>
			ScheduleCore(state, dueTime, action);
	}
}
#endif
