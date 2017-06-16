using System;
using System.Diagnostics;
using System.IO;

namespace oledid.SyntaxImprovement.Logging
{
	public class TxtLogger
	{
		private readonly string logName;
		private readonly DirectoryInfo directory;
		private bool hasTimeStamp;

		public TxtLogger(string logName)
		{
			this.logName = logName;
			directory = InitializeLogDirectory();
		}

		private DirectoryInfo InitializeLogDirectory()
		{
			var dir = new DirectoryInfo(logName);
			if (dir.Exists == false)
			{
				dir.Create();
			}
			return dir;
		}

		public void Write(params string[] args)
		{
			AppendToLogFile(writer =>
			{
				var message = GetString(args);
				Console.Write(message);
				writer.Write(message);
			});
		}

		public void WriteLine(params string[] args)
		{
			AppendToLogFile(writer =>
			{
				var message = GetString(args);
				Console.WriteLine(message);
				writer.WriteLine(message);
			});
		}

		private void AppendToLogFile(Action<StreamWriter> writeMessage)
		{
			try
			{
				var logFilePath = InitializeLogFile();

				using (var writer = File.AppendText(logFilePath))
				{
					if (!hasTimeStamp)
					{
						writer.WriteLine();
						writer.Write(TimeStamp);
						hasTimeStamp = true;
					}

					writeMessage(writer);

					writer?.Close();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Logging feilet: {ex.Message}");
			}
		}

		private string InitializeLogFile()
		{
			var logFileName = DateTime.Today.ToString(DateTimeFormats.yyyy_MM_dd_ISO) + ".txt";
			return Path.Combine(directory.FullName, logFileName);
		}

		private static string GetString(params string[] args)
		{
			var value = string.Join(string.Empty, args).Replace(Environment.NewLine, Environment.NewLine + TimeStamp);
			return value;
		}

		private static string TimeStamp => DateTime.Now.ToString("[HH:mm:ss] ");
	}
}