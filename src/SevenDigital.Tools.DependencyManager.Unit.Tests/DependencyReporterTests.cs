using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;
using SevenDigital.Tools.DependencyManager.Core;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Unit.Tests {
	[TestFixture]
	public class DependencyReporterTests
	{
		private IAssemblyLoader _assemblyLoader;
		private IAssembly _mockAssemblyA;
		private IAssembly _mockAssemblyB;
		private IAssembly _mockAssemblyC;
		private IAssembly _mockAssemblyD;
		private IAssembly _mockAssemblyE;
		private AssemblyName _assemblyNameA;
		private AssemblyName _assemblyNameB;
		private AssemblyName _assemblyNameC;
		private AssemblyName _assemblyNameD;
		private AssemblyName _assemblyNameE;
		private AssemblyName _assemblyToIgnore;
		private const string PATH = @"PATH\";

		[SetUp]
		public void setup() {
			
			_assemblyNameA = new AssemblyName("SevenDigital.AssemblyA");
			_assemblyNameB = new AssemblyName("SevenDigital.AssemblyB");
			_assemblyNameC = new AssemblyName("SevenDigital.AssemblyC");
			_assemblyNameD = new AssemblyName("SevenDigital.AssemblyD");
			_assemblyNameE = new AssemblyName("SevenDigital.AssemblyE");
			_assemblyToIgnore = new AssemblyName("IgnoreMe.AssemblyF");

			var assemblyADependencies = new List<AssemblyName> { _assemblyNameB, _assemblyNameC, _assemblyNameE };
			var assemblyBDependencies = new List<AssemblyName> { _assemblyNameC, _assemblyNameD }; 
			var assemblyCDependencies = new List<AssemblyName> { _assemblyNameE };
			var assemblyDDependencies = new List<AssemblyName> { _assemblyNameE, _assemblyToIgnore };

			_mockAssemblyA = GenerateMockAssembly(_assemblyNameA, assemblyADependencies);
			_mockAssemblyB = GenerateMockAssembly(_assemblyNameB, assemblyBDependencies);
			_mockAssemblyC = GenerateMockAssembly(_assemblyNameC, assemblyCDependencies);
			_mockAssemblyD = GenerateMockAssembly(_assemblyNameD, assemblyDDependencies);
			_mockAssemblyE = GenerateMockAssembly(_assemblyNameE, new List<AssemblyName>());

			_assemblyLoader = MockRepository.GenerateStub<IAssemblyLoader>();

			_assemblyLoader.Stub(x => x.Load(PATH + _assemblyNameA + ".dll")).Return(_mockAssemblyA);
			_assemblyLoader.Stub(x => x.Load(PATH + _assemblyNameB + ".dll")).Return(_mockAssemblyB);
			_assemblyLoader.Stub(x => x.Load(PATH + _assemblyNameC + ".dll")).Return(_mockAssemblyC);
			_assemblyLoader.Stub(x => x.Load(PATH + _assemblyNameD + ".dll")).Return(_mockAssemblyD);
			_assemblyLoader.Stub(x => x.Load(PATH + _assemblyNameE + ".dll")).Return(_mockAssemblyE);
			
		}

		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_for_AssemblyA() {
			var dependencyReporter = new DependencyFinder(_assemblyLoader, PATH);
			
			var dependencies = dependencyReporter.Find(_mockAssemblyA);

			Assert.That(dependencies.Count, Is.EqualTo(7));

			Assert.That(dependencies.Where(x => x.Reference == _assemblyNameB).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.Reference == _assemblyNameC).Count(), Is.EqualTo(2));
			Assert.That(dependencies.Where(x => x.Reference == _assemblyNameD).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.Reference == _assemblyNameE).Count(), Is.EqualTo(3));
		}


		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_2()
		{
			var dependencyReporter = new DependencyFinder(_assemblyLoader, PATH);

			var dependencies = dependencyReporter.Find(_mockAssemblyB);
			Assert.That(dependencies.Count, Is.EqualTo(4));
			Assert.That(dependencies.Where(x => x.Reference == _assemblyNameC).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.Reference == _assemblyNameD).Count(), Is.EqualTo(1));
			Assert.That(dependencies.Where(x => x.Reference == _assemblyNameE).Count(), Is.EqualTo(2));
		}

		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_3()
		{
			var dependencyReporter = new DependencyFinder(_assemblyLoader, PATH);

			var dependencies = dependencyReporter.Find(_mockAssemblyC);
			Assert.That(dependencies.Count, Is.EqualTo(1));
			Assert.That(dependencies[0].Reference, Is.EqualTo(_assemblyNameE));
		}

		[Test]
		public void AnalyseAssembly_returns_correct_dependency_count_4()
		{
			var dependencyReporter = new DependencyFinder(_assemblyLoader, PATH);

			var dependencies = dependencyReporter.Find(_mockAssemblyE);
			Assert.That(dependencies.Count, Is.EqualTo(0));
		}

		[Test]
		public void AnalyseAssembly_should_ignore_assemblies_that_dont_start_with_sevendigital()
		{
			var dependencyReporter = new DependencyFinder(_assemblyLoader, PATH);

			var dependencies = dependencyReporter.Find(_mockAssemblyD);
			Assert.That(dependencies.Count, Is.EqualTo(1), 
			            "should only return one assembly in dependency list and ignore assemblies that don't start with SevenDigital");
		}

		private IAssembly GenerateMockAssembly(AssemblyName assemblyName, List<AssemblyName> dependencyList) {
			var mockAssembly = MockRepository.GenerateStub<IAssembly>();
			mockAssembly.Stub(x => x.GetName()).Return(assemblyName);
			mockAssembly.Stub(x => x.GetReferencedAssemblies()).Return(dependencyList);
			return mockAssembly;
		}
	}
}