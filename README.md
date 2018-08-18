# SHOUTCAST MONITOR & DATA ANALYZER

This is a set of two applications:
- Shoutcast Monitor - app for monitoring shoutcast stream listeners count
- Data Analyzed - app for reading data gathered by **Shoutcast Monitor** and presenting it in tables and charts

Current version: 1.2

**Apps has polish and english localization. Screens shows application in polish**

# **Shoutcast Monitor**

Console version of the app. Can be customized by writing custom classes for logging.

Run ```monitor.exe http://xxx.xxx.xxx.xxx:8000/stats``` to monitor listeners for the specified stream.
Default time interval is **120** seconds. If you want to change that, replace value in the main program class.

# **Shoutcast Monitor GUI**

Graphical version of the app. You can specify URL and time interval. Application will store text files in the ***data*** directory in the root directory of the application.

![Shoutcast Monitor GUI](/screenshots/Monitor.png?raw=true)

# **Data Analyzer**

Data analyzer reads data gathered by **Shoutcast Monitor** and presents it in a graphical way.
You can specify directory with data and file prefix. App will display statistical data for each day in a table and each of them on a plot:
- Average number of listeners
- Time without listeners
- Number of listeners in peak

![Data Analyzer Tables](/screenshots/Analyzer_Tables.png?raw=true)

![Data Analyzer Plots](/screenshots/Analyzer_Plots.png?raw=true)

Second tab allows to see data from each day on a plot.

![Data Analyzer Daily Plots](/screenshots/Analyzer_Selector.png?raw=true)

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
