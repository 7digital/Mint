using System;
using System.IO;
using SevenDigital.Tools.DependencyManager.Core;
using SevenDigital.Tools.DependencyManager.Core.Assembly;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Controllers.Simple {
	public class DependencyReportProvider {
		private readonly string _pathToAssembly;

		public DependencyReportProvider(String pathToAssembly) {
			_pathToAssembly = pathToAssembly;
		}

		public DependencyReport Load() {
			return new DependencyReport(DependencyFinder.Find(Assembly));
		}

		private DependencyFinder DependencyFinder {
			get {
				return new DependencyFinder(new SystemAssemblyLoader(), WorkingDirectory);
			}
		}

		private IAssembly Assembly {
			get { return new SystemAssembly(System.Reflection.Assembly.LoadFrom(PathToAssembly)); }
		}

		private String WorkingDirectory { 
			get {
				return Path.GetDirectoryName(PathToAssembly);
			}
		}

		internal string PathToAssembly {
			get { return _pathToAssembly; }
		}
	}
}