namespace SevenDigital.Tools.DependencyManager.Interfaces {
	public interface IAssemblyLoader {
		IAssembly Load(string fullyQualifiedPath);
	}
}