using System.Collections.Generic;
using System.Text;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager {
	public class StringLogger : ILog {
		private readonly StringBuilder _stringBuilder = new StringBuilder();

		public void Write(string line) {
			_stringBuilder.Append(line);
		}

		public void WriteLine(string line, params object[] args) {
			_stringBuilder.AppendFormat(line, args);
		}

		public override string ToString() {
			return _stringBuilder.ToString();
		}
	}
}
