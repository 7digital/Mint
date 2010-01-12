using System;
using System.IO;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Program;
using SevenDigital.Tools.DependencyManager.Validation.Generic;

namespace SevenDigital.Tools.DependencyManager {
	public class Application {
		private const string EXECUTABLE = "chubbyrain";

		static void Main(String[] args) {
			Arguments arguments = TryClean(args);
				
			Log.WriteLine("Inspecting directory: {0}", arguments.WorkingDirectory);
			Log.WriteLine("Analyzing dependencies of: {0}\r\n\r\n", arguments.Assembly);

			TryRun(arguments);
		}

		private static void TryRun(Arguments arguments) {
			try {
				IController controller = ControllerSelector.SelectBasedOn(arguments);
				
				controller.Index();

				ExitLikeVanPersie();
			} catch (Exception e) {
				DieBecause("An error has occurred: {0}", e.Message);
			}
		}

		private static Arguments TryClean(String[] args) {
			ValidationResult<Arguments> result = Validate(args);

			DieUnless(result.Success, result.Reason);

			return result.Data;
		}

		private static ValidationResult<Arguments> Validate(String[] args) {
			ArgumentsAdapter arguments = new ArgumentsAdapter(args);

			if (false == arguments.IsValid)
				return ValidationResult<Arguments>.FailedBecause(
					"Incorrect arguments, usage: {0} {1}",
                    EXECUTABLE,
				    arguments.Usage
				);
			
			var command				= arguments[Name.Command];
			var workingDirectory	= arguments[Name.WorkingDirectory];
			var assemblyName		= arguments[Name.AssemblyName];

			if (false == Directory.Exists(workingDirectory)) 
				return ValidationResult<Arguments>.FailedBecause(
					"The working directory could not be found: <{0}>", workingDirectory
				);

			if (false == Commands.Contains(command))
				return ValidationResult<Arguments>.FailedBecause(
					"The command is invalid: <{0}>", command
				);

			return ValidationResult<Arguments>.Succeeded(
				new Arguments(
					command,
					workingDirectory,
					assemblyName
				) { Log = Log }
			);
		}

		private static void DieUnless(Boolean condition, String reason, params Object[] args) {
			Unless(condition, () => DieBecause(reason, args));
		}

		private static void Unless(Boolean condition, Action thenDoThis) {
			if (false == condition) {
				thenDoThis();
			}
		}

		private static void ExitLikeVanPersie() { // i.e, gracefully
			Environment.Exit(0);
		}

		private static void DieBecause(String reason, params Object[] args) {
			Log.WriteLine(reason, args);
			Die();
		}

		private static void Die() { Environment.Exit(1); }

		private static ILog Log {
			get { return new ConsoleLogger(); }
		}

		private static ControllerSelector ControllerSelector {
			get { return new ControllerSelector(); }
		}
	}
}