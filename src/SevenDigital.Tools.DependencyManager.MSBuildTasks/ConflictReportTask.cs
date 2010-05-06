using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using SevenDigital.Tools.DependencyManager.Controllers.Simple;
using SevenDigital.Tools.DependencyManager.Core;
using SevenDigital.Tools.DependencyManager.Core.Assembly;
using SevenDigital.Tools.DependencyManager.Interfaces;
using SevenDigital.Tools.DependencyManager.Views;

namespace SevenDigital.Tools.DependencyManager.MSBuildTasks {
	public class ConflictReportTask : ITask {
		public bool Execute() {
		    bool noConflictsFound = true;
		    foreach (var assemblyPath in AssemblyPaths) {
                RequireProperties(assemblyPath);

                Message("Inspecting directory: {0}", GetWorkingDirectory(assemblyPath));
                Message("Analyzing dependencies of: {0}\r\n\r\n", GetAssemblyName(assemblyPath));

                var dependencyReport = NewReport(assemblyPath);

                if (dependencyReport.HasConflicts)
                {
                    noConflictsFound = false;

                    BuildEngine.LogErrorEvent(
                        NewError("*** The assembly <{0}> has <{1}> reference conflicts ***",
                            GetAssemblyName(assemblyPath),
                            dependencyReport.Conflicts.Count()
                        )
                    );

                    Display(dependencyReport);
                }
		    }

		    return noConflictsFound;
		}

		private void Display(DependencyReport dependencyReport) {
			new ConflictTextView(Log) {
				DataSource = dependencyReport
			}.Display();
		}

		private DependencyReport NewReport(string assemblyPath) {
            return new DependencyReportProvider(assemblyPath).Load();
		}

		private void Message(String format, params Object[] args) {
			Log.WriteLine(format, args);
		}

		private BuildErrorEventArgs NewError(String message, params Object[] args) {
			return new BuildErrorEventArgs(
				"",
				"",
				"",
				BuildEngine.LineNumberOfTaskNode,
				0,
				0,
				0,
				String.Format(message, args),
				String.Empty,
				String.Empty
			);
		}

		// TODO: Considering failing build instead of throwing exceptions
		private void RequireProperties(string assemblyPath) {
            if (null == assemblyPath)
				throw new ArgumentNullException("AssemblyPath");

            if (false == Directory.Exists(GetWorkingDirectory(assemblyPath)))
				throw new DirectoryNotFoundException(
					String.Format(
						"The working directory does not exist. <{0}>", GetWorkingDirectory(assemblyPath)
					)
				);

            if (false == File.Exists(assemblyPath))
				throw new FileNotFoundException(
					String.Format(
						"The assembly does not exist in the working directory. <{0}>",
                        assemblyPath
					)
				);
		}

        [Required]
        public string[] AssemblyPaths {get; set;}
	    public IBuildEngine BuildEngine { get; set; }
		public ITaskHost HostObject		{ get; set; }

	    private string GetAssemblyName(string assemblyPath)
        {
            return new SystemAssembly(System.Reflection.Assembly.LoadFrom(assemblyPath)).GetName().Name; 
        }

	    private String GetWorkingDirectory(string assemblyPath)
        {

            return Path.GetDirectoryName(assemblyPath);
            
        }

		private ILog Log {
			get { return _log ?? (_log = DefaultLog); }
		}

		private ILog DefaultLog {
			get { return new TaskLog(BuildEngine); }
		}

		private ILog _log;
	}
}