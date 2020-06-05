using System;

namespace System.Reactive.Concurrency
{
	/// <summary>
	/// This is a flag interface for a specific type of <see cref="IScheduler"/>
	/// that should run its actions on a UI thread.
	/// </summary>
	public interface IDispatcherScheduler : IScheduler
	{
	}

	public static class DispatcherSchedulerExtensions
	{
		public static IDispatcherScheduler ToDispatcherScheduler(this IScheduler scheduler)
		{
			return new DispatcherSchedulerAdapter(scheduler);
		}
	}

	public class DispatcherSchedulerAdapter : IDispatcherScheduler
	{
		private readonly IScheduler _inner;

		public DispatcherSchedulerAdapter(IScheduler inner)
		{
			_inner = inner ?? throw new ArgumentNullException(nameof(inner));
		}

		public DateTimeOffset Now => _inner.Now;

		public IDisposable Schedule<TState>(TState state, Func<IScheduler, TState, IDisposable> action)
			=> _inner.Schedule(state, action);

		public IDisposable Schedule<TState>(TState state, TimeSpan dueTime, Func<IScheduler, TState, IDisposable> action)
			=> _inner.Schedule(state, dueTime, action);

		public IDisposable Schedule<TState>(TState state, DateTimeOffset dueTime, Func<IScheduler, TState, IDisposable> action)
			=> _inner.Schedule(state, dueTime, action);
	}
}
