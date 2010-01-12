using System;
using SevenDigital.Tools.DependencyManager.Core;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Views.Generic;

namespace SevenDigital.Tools.DependencyManager.Views {
	public class ConflictTextView : IView<DependencyReport> {
		
		public ConflictTextView(ILog log) {
			_log = log;
		}

		public DependencyReport DataSource {
			set { _dataSource = value; }
		}

		public void Display() {
			PrintConflicts();	
		}

		private void PrintConflicts() {
			if (!_dataSource.HasConflicts) return;

			foreach (var dependency in _dataSource.Conflicts) {
				PrintSummary(dependency);
			}
		}

		private void PrintSummary(Dependency dependency) {
			if (dependency.IsMajorConflict) {
				Log.WriteLine("***** Major Revision Conflict ***** ");
			}

			Log.WriteLine(
				"{1}{0}References: \t\t\t{2}{0}" + 
				"Reference Version: \t\t{3}{0}" +
				"Actual Reference Version: \t{4}{0}",
				Environment.NewLine,
				dependency.Assembly.Name,
				dependency.Reference.Name,
				dependency.Reference.Version,
				dependency.ActualReference.Version
			);
		}

		private ILog Log { get { return _log; } }
		private readonly ILog _log;
		private DependencyReport _dataSource;
	}
}