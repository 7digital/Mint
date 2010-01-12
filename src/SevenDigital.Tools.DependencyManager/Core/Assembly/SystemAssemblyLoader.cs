using SevenDigital.Tools.DependencyManager.Interfaces;

namespace SevenDigital.Tools.DependencyManager.Core.Assembly {
	public class SystemAssemblyLoader : IAssemblyLoader {
		public IAssembly Load(string fullyQualifiedPath) {
			return new SystemAssembly(System.Reflection.Assembly.LoadFrom(fullyQualifiedPath));
		}
	}
}