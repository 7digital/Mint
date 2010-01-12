using System;

namespace SevenDigital.Tools.DependencyManager.Program {
	public class ArgumentsAdapter {
		private readonly string[] _inputArguments;
	
		public ArgumentsAdapter(string[] inputArguments) {
			_inputArguments = inputArguments;
		}

		public String this[Name name] {
			get { return Get(name); }
		}

		public string Get(Name name) {
			CheckArgumentListLength();

			switch(name) {
				case Name.Command:
					return _inputArguments[0];
				case Name.WorkingDirectory:
					return _inputArguments[1];
				case Name.AssemblyName:
					return _inputArguments[2];
				default:
					throw new ArgumentException(
						string.Format("Argument <{0}> not found.",name)
					);
			}
		}

		private void CheckArgumentListLength() {
			if (false == IsValid)
				throw new ArgumentException(
					String.Format(
						"The argument list doesn't have the correct length which is {0}", 
						Enum.GetNames(typeof(Name)).Length.ToString()
					)
				);
		}

		public bool IsValid {
			get {
				return _inputArguments.Length == Enum.GetNames(typeof(Name)).Length;
			}
		}

		public String Usage {
			get {
				return "command workingDirectory assemblyName";
			}
		}
	}
}