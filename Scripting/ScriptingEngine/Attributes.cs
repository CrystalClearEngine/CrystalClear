using System;

/// <summary>
/// Attributes used by the scripting engine to better dechipher the users written code.
/// </summary>
namespace CrystalClear.Scripting.ScriptAttributes
{
	public sealed class VisibleAttribute : Attribute
	{

	}

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ScriptAttribute : Attribute
	{

	}
}
