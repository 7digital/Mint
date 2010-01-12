using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SevenDigital.Tools.DependencyManager.Core;
using SevenDigital.Tools.DependencyManager.Core.Assembly;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Views;

namespace SevenDigital.Tools.DependencyManager.Program {
	public class CommandFactory {
		
		public CommandFactory(ILog log) {
			_view = log;
		}

		public Action<String, String> Invalid {
			get {
				return (workingDirectory, assemblyName) => View.WriteLine("Invalid command supplied.");
			}
		}

		public Action<String, String> Conflict {
			get { return Analyse; }
		}

		public Action<String, String> Analyse {
			get {
				return (workingDirectory, assemblyName) => {
				       	IAssembly assembly =
				       		new SystemAssembly(
				       			Assembly.LoadFrom(Path.Combine(workingDirectory, assemblyName)));

				       	var finder = new DependencyFinder(new SystemAssemblyLoader(), workingDirectory);

				       	var report = GetReport(finder.Find(assembly));

				       	report.Display();
				       };
			}
		}	
			
		private ILog View {
			get { return _view; }
		}

		private ReportTextView GetReport(IEnumerable<Dependency> dependencies) {
			return new ReportTextView(View);
		}

		private readonly ILog _view;
	}
}