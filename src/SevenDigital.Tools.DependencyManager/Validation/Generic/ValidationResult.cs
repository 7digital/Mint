using System;

namespace SevenDigital.Tools.DependencyManager.Validation.Generic {
	public class ValidationResult<T> where T : class {
		public static ValidationResult<T> Succeeded(T data) {
			return new ValidationResult<T>(true, data);
		}
		
		public static ValidationResult<T> FailedBecause(String reason, params Object[] args) {
			return new ValidationResult<T>(false, String.Format(reason, args));
		}

		public ValidationResult(Boolean success, String reason) : 
			this(success, reason, null) { }

		public ValidationResult(Boolean success, T data) : this(success, null, data) {}

		public ValidationResult(Boolean success, String reason, T data) {
			_success = success;
			_reason = reason;
			_data = data;
		}

		public bool Success {
			get { return _success; }
		}

		public string Reason {
			get { return _reason; }
		}

		public T Data {
			get { return _data; }
		}

		private readonly Boolean _success;
		private readonly String _reason;
		private readonly T _data;
	}
}