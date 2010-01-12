using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Core {
	public class DependencyFinder {
		private readonly string _workingDirectory;
		private readonly IAssemblyLoader _assemblyLoader;
		private readonly List<Dependency> _dependencies;

		private string WorkingDirectory {
			get { return _workingDirectory; }
		}

		public DependencyFinder(IAssemblyLoader assemblyLoader, string workingDirectory) {
			_assemblyLoader = assemblyLoader;
			_workingDirectory = workingDirectory;
			_dependencies = new List<Dependency>();
		}

		public List<Dependency> Find(IAssembly assembly) {
			FindReferencesOf(assembly);

			return _dependencies;
		}

		private void FindReferencesOf(IAssembly assembly) {
			foreach (AssemblyName referenceName in assembly.GetReferencedAssemblies()) {
				Analyse(assembly.GetName(), referenceName);
			}
		}

		private void Analyse(
			AssemblyName assemblyName,
			AssemblyName referenceName
		) {
			if (referenceName.Name.StartsWith("SevenDigital")) {

				IAssembly reference = Load(referenceName);

				AddDependency(assemblyName, referenceName, reference.GetName());

				Unless(IsAlreadyAnalysed(referenceName), () =>
					FindReferencesOf(reference)
				);
			}
		}

		private IAssembly Load(AssemblyName assemblyName) {
			return _assemblyLoader.Load(
				Path.Combine(WorkingDirectory, assemblyName.Name + ".dll")
				);
		}

		private void AddDependency(
			AssemblyName assembly,
			AssemblyName reference,
			AssemblyName referenceOnDisk
		) {
			_dependencies.Add(
				Dependency.New().
					For(assembly).
					ThatReferences(reference).
					WithActualReference(referenceOnDisk)
			);
		}

		private bool IsAlreadyAnalysed(AssemblyName assembly) {
			return _dependencies.Exists(
				x => x.Assembly.FullName == assembly.FullName
			);
		}

		private void Unless(Boolean condition, Action thenDoThis) {
			if (false == condition) {
				thenDoThis();
			}
		}
	}
}