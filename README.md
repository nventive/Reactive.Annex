# Reactive.Annex

`Reactive.Annex` contains extension methods to help developers make the most out of the Reactive world.
`Reactive.Annex.Uno` also contains an easy-to-use implementation of a dispatcher scheduler following the `IScheduler` interface.

[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE)

## Getting Started

Add the `Reactive.Annex` nuget package and its dependencies to your project.

Then, make sure you are using the `System.Reactive.Linq` and `System.Reactive.Concurrency` namespaces in your file.

You are now ready to use these handy extension methods!

If your project uses the [Uno platform](https://platform.uno/), you can also add the `Reactive.Annex.Uno` package to your project. You can now create a `MainDispatcherScheduler`, which implements the `IDispatcherScheduler` interface.

```
var dispatcher = new MainDispatcherScheduler(CoreDispatcher.Main);
```

## Features

`Reactive.Annex` features:

* `IBackgroundScheduler` and `IDispatcherScheduler`: you no longer need to rely on argument comments to indicate whether the `IScheduler` argument should be a dispatcher or a background scheduler!

* `IObservable` extensions
    - `FirstAsync`: Creates a task from an IObservable with the first value observed.
    - `FromAsync`: Converts an async method into an observable sequence.
	- `SelectManyDisposePrevious`: Runs an async action each time your observable sequence produces a new value while making sure to cancel the previous action if it's still running.
    - `SkipWhileSelectMany`: Projects element of an observable sequence to another observable sequence, skipping new elements while resulting observable is not completed, and merges the resulting observable sequences into one observable sequence.

* `IScheduler` extensions
    - `ScheduleTask`: Schedules work using an asynchronous method, allowing for cooperative scheduling in an imperative coding style.
    - `Run`: Awaits a task execution on the specified scheduler, providing the result.

`Reactive.Annex.Uno` features:

* `MainDispatcherScheduler`: an easy-to-use implementation of `IDispatcherScheduler`

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

## Contributors

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- ALL-CONTRIBUTORS-LIST:END -->
