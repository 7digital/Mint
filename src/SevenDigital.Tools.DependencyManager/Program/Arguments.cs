using System;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Program {
	internal class Arguments {
		public Arguments(string command, String workingDirectory, String assembly) {
			Command				= command;
			WorkingDirectory	= workingDirectory;
			Assembly			= assembly;
		}

		public string Command { get; private set; }

		public string WorkingDirectory { get; private set; }

		public string Assembly { get; private set; }

		public ILog Log { get; set; }
	}
}