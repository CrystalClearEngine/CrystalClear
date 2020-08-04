using System;

// TODO: if ever static methods are allowed in interfaces, or constructors for that matter, rewrite all this and instead use an interface - IEditable.
namespace CrystalClear.SerializationSystem
{
	// TODO: decide if they should be inherited.
	[AttributeUsage(AttributeTargets.Class)]
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

		public string EditorMethodName { get; internal set; }

		public string CreatorMethodName { get; internal set; }
	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class EditorAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class CreatorAttribute : Attribute
	{
	}
}