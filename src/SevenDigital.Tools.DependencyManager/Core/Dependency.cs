using System;
using System.Reflection;
using SevenDigital.Tools.DependencyManager.Core.Fluent;

namespace SevenDigital.Tools.DependencyManager.Core {
	public class Dependency {
		public AssemblyName Assembly { get; set; }
		public AssemblyName Reference { get; set; }
		public AssemblyName ActualReference { get; set; }

		public bool IsConflict {
			get { return ActualReference.Version != Reference.Version; }
		}

		public bool IsMajorConflict {
			get { return !IsSameVersion; }
		}

		public static DependencyBuilder New() {
			return DependencyBuilder.New();
		}

		private Boolean IsSameVersion {
			get {
				return Reference.Version.Major == ActualReference.Version.Major &&
				       Reference.Version.Minor == ActualReference.Version.Minor;
			}
		}
	}
}