using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SevenDigital.Tools.DependencyManager.Core;

namespace SevenDigital.Tools.DependencyManager.Unit.Tests {
	[TestFixture]
	public class DependencyReportTests {
		
		private List<Dependency> _dependencies;
		private IEnumerable<Dependency> _conflicts;
		private Dependency _aReferencesUpToDateB;
		private Dependency _cReferencesOutOfDateB;
		private Dependency _aReferencesOutOfDateB;

		[SetUp]
		public void Setup() {
			_aReferencesOutOfDateB = Dependency.New().
				For("Assembly_A", "1.0").
				ThatReferences("Assembly_B", "1.0").
				WithActualReferenceVersion("1.1").Build();

			_cReferencesOutOfDateB = Dependency.New().
				For("Assembly_C", "1.0").
				ThatReferences("Assembly_B", "1.0").
				WithActualReferenceVersion("1.1").Build();

			_aReferencesUpToDateB = Dependency.New().
				For("Assembly_A", "2.0").
				ThatReferences("Assembly_B", "2.0").
				WithActualReferenceVersion("2.0").Build();

			_dependencies = new List<Dependency> {
        		_aReferencesOutOfDateB, _cReferencesOutOfDateB, _aReferencesUpToDateB
        	};

			_conflicts = new DependencyReport(_dependencies).Conflicts;
		}
		[Test]
		public void Conflicts_returns_a_list_of_references_whose_version_differ_from_latest_library_version() {
			Assert.That(_conflicts.Count(), Is.EqualTo(2));
			CollectionAssert.Contains(_conflicts, _aReferencesOutOfDateB);
			CollectionAssert.Contains(_conflicts, _cReferencesOutOfDateB);
		}

		[Test]
		public void Conflicts_does_not_include_assembly_that_references_the_latest_version() {
			CollectionAssert.DoesNotContain(_conflicts, _aReferencesUpToDateB); 
		}
	}
}
