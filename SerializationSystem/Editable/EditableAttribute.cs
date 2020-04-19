using System;

namespace CrystalClear.SerializationSystem
{
	// TODO: decide if they should be inherited.
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public sealed class EditableAttribute : Attribute
	{
		public EditableAttribute(string editorMethodName, string creatorMethodName)
		{
			EditorMethodName = editorMethodName;
			CreatorMethodName = creatorMethodName;
		}

		public EditableAttribute()
		{

		}

		public string EditorMethodName { get; internal set; } = null;

		public string CreatorMethodName { get; internal set; } = null;
	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class EditorAttribute : Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class CreatorAttribute : Attribute
	{

	}
}