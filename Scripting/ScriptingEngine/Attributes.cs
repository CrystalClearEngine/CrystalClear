﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting
{
	/// <summary>
	/// Attributes used by the scripting engine to better dechipher the users written code.
	/// </summary>
	namespace ScriptAttributes
	{
		public sealed class VisibleAttribute : Attribute
		{

		}

		[AttributeUsage(AttributeTargets.Class)]
		public sealed class ScriptAttribute : Attribute
		{

		}
	}
}