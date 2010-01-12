using System;
using System.Linq;
using SevenDigital.Tools.DependencyManager.Core;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Views.Generic;

namespace SevenDigital.Tools.DependencyManager.Views {
	public class ReportTextView : IView<DependencyReport> {
		
		public ReportTextView(ILog log) {
			_log = log;
		}

		public DependencyReport DataSource {
			set { _dataSource = value; }
		}

		public void Display() {

			PrintHeading("Count of assemblies referenced");
            PrintDependencies();
			PrintConflicts();	
		}

		private void PrintDependencies() {

			var list =
				_dataSource.Dependencies.GroupBy(
					x => x.Reference.Name + "_" + x.Reference.Version).ToList();

			foreach (IGrouping<string, Dependency> key in list) {
				Log.WriteLine(key.Key.PadRight(50) + key.Count());
			}
		}

		private void PrintConflicts() {
			if (!_dataSource.HasConflicts) return;

			PrintHeading("Incorrect Dependencies Summary");

			foreach (var dependency in _dataSource.Conflicts) {
				PrintSummary(dependency);
			}
		}

		private void PrintHeading(string text) {
			Log.WriteLine("------------------------------------------");
			Log.WriteLine(text);
			Log.WriteLine("------------------------------------------");
		}

		private void PrintSummary(Dependency dependency) {
			if (dependency.IsMajorConflict) {
				Log.WriteLine("***** Major Revision Conflict ***** ");
			}

			Log.WriteLine(@"{1} is referencing assembly {2}{0}  Version being referenced {3}{0}  Version in Lib {4}{0}",
				Environment.NewLine,
				dependency.Assembly.Name,
				dependency.Reference.Name,
				dependency.Reference.Version,
				dependency.ActualReference.Version
			);
		}

		private ILog Log { get { return _log; } }
		private readonly string _command;
		private readonly ILog _log;
		private DependencyReport _dataSource;
	}
}