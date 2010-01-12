using System;
using System.Reflection;
using NUnit.Framework;
using SevenDigital.Tools.DependencyManager.Core;

namespace SevenDigital.Tools.DependencyManager.Unit.Tests {
	[TestFixture]
	public class DependencyTests
	{
		[Test]
		public void IsConflict_returns_true_for_conflict_of_nonbreaking_change_number() {
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(1, 2, 5, 4);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 4);

			Assert.That(dependency.IsConflict, Is.True, "should report a minor conflict if the version numbers are not the same");
		}

		[Test]
		public void IsConflict_returns_false_for_identical_version_numbers()
		{
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(1, 2, 3, 4);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 4);

			Assert.That(dependency.IsConflict, Is.False, "should not report a conflict if the version numbers are identical");
		}

		[Test]
		public void IsMajorConflict_returns_false_for_identical_version_numbers() {
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(1, 2, 3, 4);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 4);

			Assert.That(dependency.IsMajorConflict, Is.False, "should not report a conflict if the version numbers are identical");
		}

		[Test]
		public void IsMajorConflict_returns_false_for_version_numbers_with_only_3rd_number_different()
		{
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(1, 2, 3, 4);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 5, 4);

			Assert.That(dependency.IsMajorConflict, Is.False, "should not report a conflict if the version numbers are same except for 3rd number");
		}

		[Test]
		public void IsMajorConflict_returns_false_for_version_numbers_with_only_4th_number_different()
		{
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(1, 2, 3, 4);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 8);

			Assert.That(dependency.IsMajorConflict, Is.False, "should not report a conflict if the version numbers are same except for 4th number");
		}

		[Test]
		public void IsMajorConflict_returns_false_for_version_numbers_with_only_3rd_and_4th_number_different()
		{
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(1, 2, 5, 4);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 8);

			Assert.That(dependency.IsMajorConflict, Is.False, "should not report a conflict if the version numbers are same except for 3rd and 4th numbers");
		}

		[Test]
		public void IsMajorConflict_returns_true_for_version_numbers_with_2nd_number_different()
		{
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(1, 3, 3, 8);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 8);

			Assert.That(dependency.IsMajorConflict, Is.True, "should report a conflict if the version number 2nd number is different");
		}

		[Test]
		public void IsMajorConflict_returns_true_for_version_numbers_with_1st_number_different()
		{
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(2, 2, 3, 8);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 8);

			Assert.That(dependency.IsMajorConflict, Is.True, "should report a conflict if the version number 1st number is different");
		}

		[Test]
		public void IsMajorConflict_returns_true_for_version_numbers_with_1st_and_2nd_number_different()
		{
			var dependency = new Dependency();
			dependency.ActualReference = GetNewAssemblyNameWithVersion(3, 4, 3, 8);
			dependency.Reference = GetNewAssemblyNameWithVersion(1, 2, 3, 8);

			Assert.That(dependency.IsMajorConflict, Is.True, "should report a conflict if the version number 1st and 2nd numbers are different");
		}


		private AssemblyName GetNewAssemblyNameWithVersion(int major, int breaking, int nonBreaking, int random)
		{
			return new AssemblyName
			       	{
			       		Version = new Version(major, breaking, nonBreaking, random)
			       	};
		}
	}
}