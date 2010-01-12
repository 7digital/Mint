namespace SevenDigital.Tools.DependencyManager.Interfaces {
	public interface ILog {
		void Write(string line);
		void WriteLine(string line, params object[] args);
	}
}