using System;
using Microsoft.Build.Framework;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.MSBuildTasks {
	internal class TaskLog : ILog {
		private readonly IBuildEngine _buildEngine;

		public TaskLog(IBuildEngine buildEngine) {
			_buildEngine = buildEngine;
		}

		public IBuildEngine BuildEngine {
			get { return _buildEngine; }
		}

		public void WriteLine(string line, params object[] args) {
			Write(String.Format(line, args));
		}

		public void Write(string line) {
			BuildEngine.LogMessageEvent(ToMessage(line));
		}

		private BuildMessageEventArgs ToMessage(string line) {
			return new BuildMessageEventArgs(
				line, 
				String.Empty, 
				String.Empty, 
				MessageImportance.Normal
			);
		}
	}
}
