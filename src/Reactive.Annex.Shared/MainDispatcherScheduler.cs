#if __ANDROID__ || __IOS__ || __WASM__ || WINDOWS
using System.Reactive.Disposables;
using Microsoft.UI.Dispatching;

namespace System.Reactive.Concurrency
{
	public partial class MainDispatcherScheduler : IDispatcherScheduler
	{
		private readonly DispatcherQueue _dispatcher;
		private readonly DispatcherQueuePriority _priority;

		public MainDispatcherScheduler(DispatcherQueue dispatcher)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			_priority = DispatcherQueuePriority.Normal;
		}

		public MainDispatcherScheduler(DispatcherQueue dispatcher, DispatcherQueuePriority priority)
		{
			_dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			_priority = priority;
		}

		public DateTimeOffset Now => DateTimeOffset.Now;

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
		{
			var subscription = new SerialDisposable();
			var d = new CancellationDisposable();

			_dispatcher.TryEnqueue(
				_priority,
				() =>
				{
					if (!subscription.IsDisposed)
					{
						subscription.Disposable = action(this, state);
					}
				}
			);
			return new CompositeDisposable(subscription, d);
		}

		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action) =>
			ScheduleCore(state, dueTime - Now, action);

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action) =>
			ScheduleCore(state, dueTime, action);
	}
}
#endif
