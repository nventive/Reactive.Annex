# Reactive.Annex

New types and extension methods built on top of [`System.Reactive`](https://github.com/dotnet/reactive).

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)
![Version](https://img.shields.io/nuget/v/Reactive.Annex?style=flat-square)
![Downloads](https://img.shields.io/nuget/dt/Reactive.Annex?style=flat-square)

## Getting Started

1. Add the `Reactive.Annex` nuget package to your project.

1. Then, make sure you are using the `System.Reactive.Linq` and `System.Reactive.Concurrency` namespaces in your file.

You are now ready to use these handy extension methods!

## Next Steps
If your project uses [Uno Platform](https://platform.uno/), you can also add the following packages.
- `Reactive.Annex.Uno` for projects using **Uno.UI** and **UWP**.
- `Reactive.Annex.Uno.WinUI` for projects using **Uno.WinUI** or **WinUI**.

With this, You can create a `MainDispatcherScheduler`, which implements the `IDispatcherScheduler` interface.

### WinUI / Uno.WinUI
```csharp
Microsoft.UI.Dispatching.DispatcherQueue dispatcherQueue = //...
var scheduler = new MainDispatcherScheduler(dispatcherQueue);
```
### UWP / Uno.UI
```csharp
Windows.UI.Core coreDispatcher = //...
var scheduler = new MainDispatcherScheduler(coreDispatcher);
```

## Features

### Specialized Interfaces for DI
You can use `IBackgroundScheduler` and `IDispatcherScheduler` to clearly differentiate background schedulers from dispatcher schedulers. Both interfaces implement `IScheduler`. This is useful when using dependency injection.

#### Implementations
`MainDispatcherScheduler` is an implementation of `IDispatcherScheduler`. It's available from the `Reactive.Annex.Uno` and `Reactive.Annex.Uno.WinUI` packages.
- Use `Reactive.Annex.Uno` in projets using **UWP** or **Uno.UI**.
- Use `Reactive.Annex.Uno.WinUI` in projects using **WinUI** or **Uno.WinUI**.

### Extension Methods on `IObservable`
- `FirstAsync`: Creates a task from an IObservable with the first value observed.
- `FromAsync`: Converts an async method into an observable sequence.
- `SelectManyDisposePrevious`: Runs an async action each time your observable sequence produces a new value while making sure to cancel the previous action if it's still running.
- `SkipWhileSelectMany`: Projects element of an observable sequence to another observable sequence, skipping new elements while resulting observable is not completed, and merges the resulting observable sequences into one observable sequence.

### Extension Methods on `IScheduler`
- `ScheduleTask`: Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
- `Run`: Awaits a task execution on the specified scheduler, providing the result.

## Changelog

Please consult the [CHANGELOG](CHANGELOG.md) for more information about version
history.

## License

This project is licensed under the Apache 2.0 license - see the
[LICENSE](LICENSE) file for details.

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details on the process for
contributing to this project.

Be mindful of our [Code of Conduct](CODE_OF_CONDUCT.md).