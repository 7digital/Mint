using SevenDigital.Tools.DependencyManager.Controllers.Simple;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Views;

namespace SevenDigital.Tools.DependencyManager.Program {
	internal class ControllerSelector {
		public IController SelectBasedOn(Arguments arguments) {
			switch (arguments.Command) {
				case "conflict":
					return new DependencyReportController(
						new ConflictTextView(arguments.Log),
						arguments.WorkingDirectory,
						arguments.Assembly
						);
				case "report":
						return new DependencyReportController(
						new ReportTextView(arguments.Log),
						arguments.WorkingDirectory,
						arguments.Assembly
						);
				default: return null;
			}
		}
	}
}