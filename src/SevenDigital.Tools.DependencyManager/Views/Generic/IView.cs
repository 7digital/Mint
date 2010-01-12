namespace SevenDigital.Tools.DependencyManager.Views.Generic {
	public interface IView<T> where T : class {
		T DataSource { set; }
		void Display();
	}
}
