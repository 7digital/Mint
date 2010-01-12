using System;
using System.Linq;

namespace SevenDigital.Tools.DependencyManager.Program {
	public static class Commands {
		private static readonly String[] _available = new [] {
			"report", 
			"conflict"
		};

		public static bool Contains(string argument) {
			return _available.Contains(argument);	
		}		
	}
}