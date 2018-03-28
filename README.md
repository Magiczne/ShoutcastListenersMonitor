# ShoutcastListenersMonitor

Simple app for monitoring shoutcast stream listeners. Can be customized by writing custom classes for logging.

# Usage

Run ```monitor.exe http://xxx.xxx.xxx.xxx:8000/stats``` to monitor listeners for the specified stream.
Default time interval is **120** seconds. If you want to change that, replace value in the main program class.

# Custom loggers

If you want, you can write your custom logging classes. 

They should implement interface:
```c#
interface IDataLogger
{
  void Log(int listeners);
  void Error(string message);
}
```

You can use the default ones too:
- **ConsoleLogger** that logs data to the console standard output
- **TextFileLogger** that logs data to the file named with current date

Multiple loggers can be used when combined using **CompositeLogger** class.

# License

Copyright 2018 Michał Kleszczyński

MIT License
