using System;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Program {
	public class ConsoleLogger : ILog
	{
		public void Write(string line)
		{
			Console.Write(line);
		}

		public void WriteLine(string line, params object[] args)
		{
			Console.WriteLine(line, args);
		}
	}
}