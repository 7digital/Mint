using System.Collections;
using System.Reflection;

namespace SevenDigital.Tools.DependencyManager.Interfaces {
	public interface IAssembly {
		AssemblyName GetName();
		IEnumerable GetReferencedAssemblies();
	}
}