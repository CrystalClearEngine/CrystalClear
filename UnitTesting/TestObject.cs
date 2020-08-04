using System;
using System.Collections.Generic;

namespace UnitTests
{
	internal class TestObject
		: IEquatable<TestObject>
	{
		public string StringData;

		public TestObject(string stringData)
		{
			StringData = stringData;
		}

		public TestObject(string stringData, bool booly)
		{
			StringData = stringData;
			Booly = booly;
		}

		public bool Booly { get; set; }

		public bool Equals(TestObject other) =>
			other != null &&
			StringData == other.StringData;

		public override bool Equals(object obj) => Equals(obj as TestObject);

		public override int GetHashCode() => 1045584480 + EqualityComparer<string>.Default.GetHashCode(StringData);
	}
}