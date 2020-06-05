using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Concurrency
{
	public static partial class SchedulerExtensions
	{
		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask(
			this IScheduler scheduler,
			Func<CancellationToken, IScheduler, Task> taskBuilder)
		{
			return scheduler.ScheduleTask(default(object), (ct, s, st) => taskBuilder(ct, scheduler));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask(
			this IScheduler scheduler,
			Func<CancellationToken, IScheduler, Task<IDisposable>> taskBuilder)
		{
			return scheduler.ScheduleTask(default(object), (ct, s, st) => taskBuilder(ct, scheduler));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="dueTime">Relative time after which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask(
			this IScheduler scheduler,
			TimeSpan dueTime,
			Func<CancellationToken, IScheduler, Task> taskBuilder)
		{
			return scheduler.ScheduleTask(default(object), dueTime, (ct, s, st) => taskBuilder(ct, scheduler));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="dueTime">Relative time after which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask(
			this IScheduler scheduler,
			TimeSpan dueTime,
			Func<CancellationToken, IScheduler, Task<IDisposable>> taskBuilder)
		{
			return scheduler.ScheduleTask(default(object), dueTime, (ct, s, st) => taskBuilder(ct, scheduler));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="dueTime">Absolute time at which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask(
			this IScheduler scheduler,
			DateTimeOffset dueTime,
			Func<CancellationToken, IScheduler, Task> taskBuilder)
		{
			return scheduler.ScheduleTask(default(object), dueTime, (ct, s, st) => taskBuilder(ct, scheduler));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="dueTime">Absolute time at which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask(
			this IScheduler scheduler,
			DateTimeOffset dueTime,
			Func<CancellationToken, IScheduler, Task<IDisposable>> taskBuilder)
		{
			return scheduler.ScheduleTask(default(object), dueTime, (ct, s, st) => taskBuilder(ct, scheduler));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="state">State to pass to the asynchronous method.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask<TState>(
			this IScheduler scheduler, TState state,
			Func<CancellationToken, IScheduler, TState, Task> taskBuilder)
		{
			return scheduler.Schedule(state, (s, st) => InvokeTask(scheduler, st, taskBuilder));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="state">State to pass to the asynchronous method.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask<TState>(
			this IScheduler scheduler,
			TState state,
			Func<CancellationToken, IScheduler, TState, Task<IDisposable>> taskBuilder)
		{
			return scheduler.Schedule(state, (s, st) => InvokeTaskWithDisposable(scheduler, st, taskBuilder));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="state">State to pass to the asynchronous method.</param>
		/// <param name="dueTime">Relative time after which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask<TState>(
			this IScheduler scheduler,
			TState state,
			TimeSpan dueTime,
			Func<CancellationToken, IScheduler, TState, Task> taskBuilder)
		{
			return scheduler.Schedule(state, dueTime, (s, st) => InvokeTask(scheduler, st, taskBuilder));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="state">State to pass to the asynchronous method.</param>
		/// <param name="dueTime">Relative time after which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask<TState>(
			this IScheduler scheduler,
			TState state,
			TimeSpan dueTime,
			Func<CancellationToken, IScheduler, TState, Task<IDisposable>> taskBuilder)
		{
			return scheduler.Schedule(state, dueTime, (s, st) => InvokeTaskWithDisposable(scheduler, st, taskBuilder));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="state">State to pass to the asynchronous method.</param>
		/// <param name="dueTime">Absolute time at which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask<TState>(
			this IScheduler scheduler,
			TState state,
			DateTimeOffset dueTime,
			Func<CancellationToken, IScheduler, TState, Task> taskBuilder)
		{
			return scheduler.Schedule(state, dueTime, (s, st) => InvokeTask(scheduler, st, taskBuilder));
		}

		/// <summary>
		/// Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
		/// </summary>
		/// <param name="scheduler">Scheduler to schedule work on.</param>
		/// <param name="state">State to pass to the asynchronous method.</param>
		/// <param name="dueTime">Absolute time at which to execute the action.</param>
		/// <param name="taskBuilder">Asynchronous method to run the work.</param>
		/// <returns>Disposable object that allows to cancel outstanding work on cooperative cancellation points or through the cancellation token passed to the asynchronous method.</returns>
		public static IDisposable ScheduleTask<TState>(
			this IScheduler scheduler,
			TState state,
			DateTimeOffset dueTime,
			Func<CancellationToken, IScheduler, TState, Task<IDisposable>> taskBuilder)
		{
			return scheduler.Schedule(state, dueTime, (s, st) => InvokeTaskWithDisposable(scheduler, st, taskBuilder));
		}

		/// <summary>
		/// Awaits a task execution on the specified scheduler, providing the result.
		/// </summary>
		/// <returns>A task that will provide the result of the execution.</returns>
		public static Task<T> Run<T>(this IScheduler source, Func<CancellationToken, Task<T>> taskBuilder, CancellationToken cancellationToken)
		{
			var completion = new TaskCompletionSource<T>();

			var disposable = new SingleAssignmentDisposable();
			var ctr = default(CancellationTokenRegistration);

			if (cancellationToken.CanBeCanceled)
			{
				ctr = cancellationToken.Register(() =>
				{
					completion.TrySetCanceled();
					disposable.Dispose();
				});
			}

			disposable.Disposable = source.ScheduleTask(
				async (ct, _) =>
				{
					try
					{
						var result = await taskBuilder(ct);
						completion.TrySetResult(result);
					}
					catch (Exception e)
					{
						completion.TrySetException(e);
					}
					finally
					{
						ctr.Dispose();
					}
				}
			);

			return completion.Task;
		}

		/// <summary>
		/// Awaits a task execution on the specified scheduler, providing the result.
		/// </summary>
		/// <returns>A task that will provide the result of the execution.</returns>
		public static Task<T> Run<T>(this IScheduler source, Func<T> func, CancellationToken cancellationToken)
		{
			var completion = new TaskCompletionSource<T>();

			var disposable = new SingleAssignmentDisposable();
			var ctr = default(CancellationTokenRegistration);

			if (cancellationToken.CanBeCanceled)
			{
				ctr = cancellationToken.Register(() =>
				{
					completion.TrySetCanceled();
					disposable.Dispose();
				});
			}

			disposable.Disposable = source.Schedule(
				() =>
				{
					if (disposable.IsDisposed)
					{
						return; // CT canceled
						}

					try
					{
						var result = func();
						completion.TrySetResult(result);
					}
					catch (Exception e)
					{
						completion.TrySetException(e);
					}
					finally
					{
						ctr.Dispose();
					}
				}
			);

			return completion.Task;
		}

		/// <summary>
		/// Awaits a task on the specified scheduler, without providing a result.
		/// </summary>
		/// <returns>A task that will complete when the work has completed.</returns>
		public static Task Run(this IScheduler source, Func<CancellationToken, Task> taskBuilder, CancellationToken ct)
		{
			return Run(
				source,
				async ct2 => { await taskBuilder(ct2); return Unit.Default; },
				ct
			);
		}

		/// <summary>
		/// Awaits a task on the specified scheduler, without providing a result.
		/// </summary>
		/// <returns>A task that will complete when the work has completed.</returns>
		public static Task Run(this IScheduler source, Action action, CancellationToken ct)
		{
			var completion = new TaskCompletionSource<Unit>();

			var disposable = new SingleAssignmentDisposable();
			var ctr = default(CancellationTokenRegistration);

			if (ct.CanBeCanceled)
			{
				ctr = ct.Register(() =>
				{
					completion.TrySetCanceled();
					disposable.Dispose();
				});
			}

			disposable.Disposable = source.Schedule(
				() =>
				{
					if (disposable.IsDisposed)
					{
						return; // CT canceled
						}

					try
					{
						action();
						completion.TrySetResult(Unit.Default);
					}
					catch (Exception e)
					{
						completion.TrySetException(e);
					}
					finally
					{
						ctr.Dispose();
					}
				}
			);

			return completion.Task;
		}

		// WARNING: Do not replace with a call to InvokeTaskWithDisposable with .ContinueWith(_ => Disposable.Empty) because
		// exceptions are not handled correctly by the scheduler. Hence, we need to keep the two methods (InvokeTask & InvokeTaskWithDisposable)
		private static IDisposable InvokeTask<TState>(
			IScheduler scheduler,
			TState state,
			Func<CancellationToken, IScheduler, TState, Task> taskBuilder)
		{
			var subscriptions = new SerialDisposable();
			var cancellationDisposable = new CancellationDisposable();
			subscriptions.Disposable = cancellationDisposable;

			taskBuilder(cancellationDisposable.Token, scheduler, state)
				.ContinueWith(
				t =>
				{
					if (t.IsFaulted)
					{
						subscriptions.Disposable = scheduler.Schedule(() => { t.Exception.Handle(e => e is OperationCanceledException); });
					}
					else
					{
						subscriptions.Disposable = null;
					}
				},
				TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.ExecuteSynchronously);

			return subscriptions;
		}

		private static IDisposable InvokeTaskWithDisposable<TState>(
			IScheduler scheduler,
			TState state,
			Func<CancellationToken, IScheduler, TState, Task<IDisposable>> taskBuilder)
		{
			var subscriptions = new SerialDisposable();
			var cancellationDisposable = new CancellationDisposable();
			subscriptions.Disposable = cancellationDisposable;

			taskBuilder(cancellationDisposable.Token, scheduler, state)
				.ContinueWith(
				t =>
				{
					if (t.IsCanceled)
					{
						subscriptions.Disposable = null;
					}
					else if (t.IsFaulted)
					{
						subscriptions.Disposable = scheduler.Schedule(() => { t.Exception.Handle(e => e is OperationCanceledException); });
					}
					else
					{
						subscriptions.Disposable = t.Result;
					}
				},
				TaskContinuationOptions.AttachedToParent | TaskContinuationOptions.ExecuteSynchronously);

			return subscriptions;
		}
	}
}
