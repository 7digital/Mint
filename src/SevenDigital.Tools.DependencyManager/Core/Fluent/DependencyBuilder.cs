using System;
using System.Reflection;

namespace SevenDigital.Tools.DependencyManager.Core.Fluent {
	public class DependencyBuilder {
		private AssemblyName _assemblyName;
		private AssemblyName _referenced;
		private AssemblyName _latestReferenceVersion;

		public static DependencyBuilder New() {
			return new DependencyBuilder();
		}

		public static implicit operator Dependency(DependencyBuilder builder) {
			return builder.Build();
		}

		public DependencyBuilder For(String assemblyName, String version) {
			return For(NewAssemblyName(assemblyName, version));
		}

		public DependencyBuilder For(AssemblyName assemblyName) {
			_assemblyName = assemblyName;

			return this;
		}

		public DependencyBuilder ThatReferences(String referenceName, String version) {
			return ThatReferences(NewAssemblyName (referenceName, version));
		}

		public DependencyBuilder ThatReferences(AssemblyName referencedAssemblyName) {
			_referenced = referencedAssemblyName;

			return this;
		}

		public DependencyBuilder WithActualReferenceVersion(String version) {
			if (null == _referenced)
				throw new InvalidOperationException(
					"Unable to set the latest reference version because " + 
					"no reference has been set yet. " + 
					"Be sure to call <ThatReferences> first."
				);

			return WithActualReference(NewAssemblyName(_referenced.Name, version));
		}

		public DependencyBuilder WithActualReference(AssemblyName latestReference) {
			_latestReferenceVersion = latestReference;

			return this;
		}

		private AssemblyName NewAssemblyName(string name, String version) {
			return new AssemblyName(name) {
				Version = new Version(version)
			};
		}

		public Dependency Build() {
			EnsureConfig();

			return new Dependency {
              	Assembly = _assemblyName, 
              	Reference			= _referenced,
              	ActualReference	= _latestReferenceVersion ?? _referenced
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