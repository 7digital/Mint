using System;
using System.IO;
using SevenDigital.Tools.DependencyManager.Core;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Views.Generic;

namespace SevenDigital.Tools.DependencyManager.Controllers.Simple {
	public class DependencyReportController : IController {
		public DependencyReportController(
			IView<DependencyReport> view, 
			String workingDirectory, 
			String assemblyName
		) {
			_view = view;
			_workingDirectory = workingDirectory;
			_assembly = assemblyName;
		}

		public void Index() {
			var report = NewReport();
			_view.DataSource = report;
			_view.Display();
		}

		private DependencyReport NewReport() {
			return new DependencyReportProvider(FullPath).Load();
		}

		private String FullPath {
			get {
				return Path.Combine(WorkingDirectory, AssemblyName);
			}
		}

		private string WorkingDirectory {
			get { return _workingDirectory; }
		}

		private string AssemblyName {
			get { return _assembly; }
		}

		private readonly IView<DependencyReport> _view;
		private readonly string _workingDirectory;
		private readonly string _assembly;
	}
}