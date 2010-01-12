using System;
using System.IO;
using SevenDigital.Tools.DependencyManager.Controllers.Simple;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Views;

namespace SevenDigital.Tools.DependencyManager.Program {
	public class Program {
		static void Main(String[] args) {
			Arguments arguments = ValidateArgs(args);
				
			Log.WriteLine("Inspecting directory: {0}", arguments.WorkingDirectory);
			Log.WriteLine("Analyzing dependencies of: {0}\r\n\r\n", arguments.Assembly);
	
			IController controller = SelectController(arguments);

			TryRun(controller);

			Environment.Exit(0);
		}

		private static Arguments ValidateArgs(string[] args) {
			ArgumentsAdapter argumentsAdapter = new ArgumentsAdapter(args);

			Unless(argumentsAdapter.AreValid, () =>
			                                  DieBecause("Incorrect arguments, usage: ", argumentsAdapter.Usage)
				);

			string argument = argumentsAdapter.Get(ArgumentsAdapter.Names.Command);
			string workingDirectory = argumentsAdapter.Get(ArgumentsAdapter.Names.WorkingDirectory);
			string assemblyName = argumentsAdapter.Get(ArgumentsAdapter.Names.AssemblyName);

			Unless(Directory.Exists(workingDirectory), () =>
			                                           DieBecause("The library directory could not be found: <{0}>", workingDirectory)
				);

			Unless(Command.Exists(argument), () =>
			                                 DieBecause("The command is invalid: {0}", argument)
				);


			return new Arguments(argument,workingDirectory,assemblyName);
		}

		private static void TryRun(IController controller) {
			try {
				controller.Index();
			} catch (Exception e) {
				DieBecause("An error has occurred: {0}", e.Message);
			}
		}

		private static IController SelectController(Arguments arguments) {
			switch (arguments.Command) {
				case "report":
				case "conflict":
				case "assert" :
					return new DependencyReportController(
						new DependencyReportTextView(Log),
						arguments.WorkingDirectory,
						arguments.Assembly
						);
				default : return null;
			}
		}

		private static void DieBecause(String reason, params Object[] args) {
			Log.WriteLine(reason, args);
			Die();
		}

		private static void Die() {
			Environment.Exit(1);
		}

		private static void Unless(Boolean condition, Action thenDoThis) {
			if (false == condition) {
				thenDoThis();
			}
		}

		private static readonly ILog _log = new ConsoleLogger();

		private static ILog Log {
			get { return _log; }
		}
	}
}