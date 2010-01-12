using System;
using System.IO;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Rhino.Mocks;

namespace SevenDigital.Tools.DependencyManager.MSBuildTasks.Unit.Tests {
	[TestFixture]
	public class ConflictReportTaskTests {
		private const string ASSEMBLY_NAME = "SevenDigital.TestAssembly";
		private String _workingDirectory = Path.GetFullPath("lib/SampleAssemblies");

	    private String _assemblyPathWithConflicts;

	    [SetUp]
		public void Setup() {
			Given_a_working_directory_containing_conflicted_assemblies();
            _assemblyPathWithConflicts = _workingDirectory + "/" + ASSEMBLY_NAME + ".dll";
		}

		public string WorkingDirectory {
			get { return _workingDirectory; }
		}

        [Test]
        public void Execute_throws_NullReferenceException_if_assemblypaths_not_supplied()
        {
            Assert.Throws(typeof(NullReferenceException), () =>
                new ConflictReportTask().Execute()
            );
        }
		
		[Test]
		public void Execute_throws_directory_not_found_if_assembly_path_does_not_exist() {
			Assert.Throws(typeof(DirectoryNotFoundException), () => 
				new ConflictReportTask {
					AssemblyPaths = new []{"C:/thisassemblydoesnotexist/"}
				}.Execute()
			);
		}

		[Test]
		public void Execute_throws_file_not_found_if_assembly_does_not_exist() {
		    Assert.Throws(typeof (FileNotFoundException),
		                  () =>
		                  new ConflictReportTask
		                  {
		                      AssemblyPaths = new []{
		                          _workingDirectory + "/thisassemblydoesnotexist.dll"}
				}.Execute()
			);
		}



        [Test]
        public void Given_a_working_directory_containing_conflicted_assemblies_then_build_engine_is_notified_with_error()
        {
            Given_a_working_directory_containing_conflicted_assemblies();

            var mockBuildEngine = MockRepository.GenerateStub<IBuildEngine>();

            var theTask = new ConflictReportTask
            {
                AssemblyPaths = new []{_assemblyPathWithConflicts},
                BuildEngine = mockBuildEngine
                
            };

            theTask.Execute();

            mockBuildEngine.AssertWasCalled(
                engine => engine.LogErrorEvent(Arg<BuildErrorEventArgs>.Is.Anything)
            );
        }

		[Test]
		public void Given_a_working_directory_containing_conflicted_assemblies_then_execute_returns_true_because_the_task_is_a_report() {
			Given_a_working_directory_containing_conflicted_assemblies();

			var theTask = NewTask();

			Assert.True(
				theTask.Execute(), 
				"Expected false to be returned because the assembly <{0}> has conflicts.", 
				ASSEMBLY_NAME
		    );
		}

		[Test]
		public void Execute_emits_working_directory_to_build_engine() {
			var mockBuildEngine = MockRepository.GenerateStub<IBuildEngine>();
			var theTask = NewTask();

			theTask.BuildEngine = mockBuildEngine;
			theTask.Execute();

			var workingDirMessage = String.Format(
				"Inspecting directory: {0}",
			     _workingDirectory
			);

			mockBuildEngine.AssertWasCalled(
				engine => engine.LogMessageEvent(
					Arg<BuildMessageEventArgs>.Matches(
						arg => arg.Message == workingDirMessage
					)
				)
			);
		}
		
		[Test]
		public void Execute_emits_assembly_name_to_build_engine() {
			var mockBuildEngine = MockRepository.GenerateStub<IBuildEngine>();
			var theTask = NewTask();

			theTask.BuildEngine = mockBuildEngine;
			theTask.Execute();

			var workingDirMessage = String.Format(
				"Analyzing dependencies of: {0}\r\n\r\n",
			     ASSEMBLY_NAME
			);

			mockBuildEngine.AssertWasCalled(
				engine => engine.LogMessageEvent(
					Arg<BuildMessageEventArgs>.Matches(
						arg => arg.Message == workingDirMessage
					)
				)
			);
		}

		private void Given_a_working_directory_containing_conflicted_assemblies() {
			_workingDirectory = Path.GetFullPath("lib/1.2.121");
		}

		private ConflictReportTask NewTask() {
			var inertBuildEngine = MockRepository.GenerateStub<IBuildEngine>();

			return new ConflictReportTask
            {
                AssemblyPaths = new []{_assemblyPathWithConflicts},
                BuildEngine = inertBuildEngine
                
            };
		}
	}
}