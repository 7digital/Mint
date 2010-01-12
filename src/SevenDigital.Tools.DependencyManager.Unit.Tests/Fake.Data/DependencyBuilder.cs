using System;
using System.Reflection;

namespace SevenDigital.Tools.DependencyManager.Unit.Tests.Fake.Data {
	public class DependencyBuilder {
		private AssemblyName _assemblyName;
		private AssemblyName _referenced;
		private AssemblyName _latestReferenceVersion;

		public static DependencyBuilder New() {
			return new DependencyBuilder();
		}

		public DependencyBuilder For(String assemblyName, String version) {
			_assemblyName = NewAssemblyName(assemblyName, version);
			
			return this;
		}

		public DependencyBuilder ThatReferences(String referenceName, String version) {
			_referenced = NewAssemblyName (referenceName, version);

			return this;
		}
		
		public DependencyBuilder LatestReferenceVersion(String version) {
			if (null == _referenced)
				throw new InvalidOperationException(
					"Unable to set the latest reference version because " + 
					"no reference has been set yet. " + 
					"Be sure to call <ThatReferences> first."
				);

			_latestReferenceVersion = NewAssemblyName(_referenced.Name, version);

			return this;
		}

		private AssemblyName NewAssemblyName(string name, string version) {
			return new AssemblyName(name) {
				Version = new Version(version)
			};
		}

		public Dependency Build() {
			EnsureConfig();

			return new Dependency {
              	ReferencingAssembly = _assemblyName, 
              	Reference			= _referenced,
				LibraryReference	= _latestReferenceVersion ?? _referenced
			};
		}

		private void EnsureConfig() {
			if (null == _assemblyName)
				throw new InvalidOperationException(
					"No assembly has been supplied, have you invoked <For>?"
				);

			if (null == _referenced)
				throw new InvalidOperationException(
					"No referenced assembly has been supplied, have you invoked <ThatReferences>?"
				);
		}
	}
}