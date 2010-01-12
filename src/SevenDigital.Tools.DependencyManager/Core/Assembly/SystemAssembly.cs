using System.Collections;
using System.Reflection;
using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Core.Assembly {
	public class SystemAssembly : IAssembly {
		private readonly System.Reflection.Assembly _innerAssembly;

		public SystemAssembly(System.Reflection.Assembly innerAssembly) {
			_innerAssembly = innerAssembly;
		}

		public AssemblyName GetName() {
			return _innerAssembly.GetName();
		}

		public IEnumerable GetReferencedAssemblies() {
			return _innerAssembly.GetReferencedAssemblies();
		}
	}
}
