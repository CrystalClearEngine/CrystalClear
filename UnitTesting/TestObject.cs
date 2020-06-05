using System;
using System.Collections.Generic;

namespace UnitTests
{
	internal class TestObject
		: IEquatable<TestObject>
	{
		public TestObject(string stringData)
		{
			StringData = stringData;
		}

		public TestObject(string stringData, bool booly)
		{
			StringData = stringData;
			Booly = booly;
		}

		public string StringData;

		private bool booly;
		public bool Booly { get => booly; set => booly = value; }

		public override bool Equals(object obj)
		{
			return Equals(obj as TestObject);
		}

		public bool Equals(TestObject other)
		{
			return other != null &&
				   StringData == other.StringData;
		}

		public override int GetHashCode()
		{
			return 1045584480 + EqualityComparer<string>.Default.GetHashCode(StringData);
		}
	}
}