# ShoutcastListenersMonitor

Simple app for monitoring shoutcast stream listeners. Can be customized by writing custom classes for logging.

# Usage

Run ```monitor.exe http://xxx.xxx.xxx.xxx:8000/stats``` to monitor listeners for the specified stream.
Default time interval is **120** seconds. If you want to change that, replace value in the main program class.

# Custom loggers

If you want, you can write your custom logging classes. The default ones are:
- **ConsoleLogger** that logs data to the console standard output
- **TextFileLogger** that logs data to the file named with current date

You can combine multiple loggers using **CompositeLogger** class.

# License

Copyright 2018 Michał Kleszczyński

MIT License
