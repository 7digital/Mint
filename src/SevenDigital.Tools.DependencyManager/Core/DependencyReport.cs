using System.Collections.Generic;
using System.Linq;

namespace SevenDigital.Tools.DependencyManager.Core {
	public class DependencyReport {
		public bool HasConflicts {
			get { return Conflicts.Count() > 0; }
		}

		public DependencyReport(IEnumerable<Dependency> dependencies) {
			_dependencies = dependencies;
		}

		public IEnumerable<Dependency> Conflicts {
			get {
				return Dependencies.Where(dep => dep.IsConflict);
			}
		}

		public IEnumerable<Dependency> Dependencies {
			get { return _dependencies; }
		}

		private readonly IEnumerable<Dependency> _dependencies;
	}
}