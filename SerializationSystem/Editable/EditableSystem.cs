using System;
using System.Collections.Generic;
using System.Reflection;

namespace CrystalClear.SerializationSystem
{
	// TODO: use IEditable interface instead.
	public static class EditableSystem
	{
		private static Dictionary<string, CreatorDelegate> creatorCache = new Dictionary<string, CreatorDelegate>();

		public static bool IsEditable(this Type type, out EditableAttribute editableAttribute)
		{
			foreach (object attribute in type.GetCustomAttributes(true))
			{
				if (attribute.GetType() == typeof(EditableAttribute))
				{
					editableAttribute = (EditableAttribute)attribute;
					return true;
				}
			}

			editableAttribute = null;
			return false;
		}

		public static bool IsEditable(this Type type) => IsEditable(type, out _);


		public static void OpenEditor(Type type, ref EditorData current)
		{
			if (!type.IsEditable())
			{
				throw new ArgumentException("This type is not editable.");
			}

			FindEditor(type)(ref current);
		}

		public delegate void EditorDelegate(ref EditorData editorData);

		public static EditorDelegate FindEditor(Type type)
		{
			if (type.IsEditable(out EditableAttribute attribute))
			{
				string methodName = attribute.EditorMethodName;
				if (methodName is null)
				{
					foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (method.GetCustomAttribute<EditorAttribute>() != null)
						{
							// Store the name so it doesn't have to be searched for again.
							attribute.EditorMethodName = method.Name;
							// TODO: try... catch etc
							return (EditorDelegate)method.CreateDelegate(typeof(EditorDelegate));
						}
					}
				}
				else
				{
					// TODO: try... catch etc
					return (EditorDelegate)type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).CreateDelegate(typeof(EditorDelegate));
				}
			}

			return null;
		}

		public static object Create(Type type, EditorData data)
		{
			if (type.IsEditable() == false)
			{
				throw new ArgumentException("Type is not Editable!");
			}

			if (data is null)
			{
				throw new ArgumentNullException("Data is null!");
			}

			CreatorDelegate creatorDelegate = FindCreator(type);

			if (creatorDelegate is null)
			{
				throw new Exception("No Creator was found!");
			}

			try
			{
				return creatorDelegate(data);
			}
			catch (Exception ex)
			{
				throw new Exception("Exception within the creator! (It will be thrown after this Exception.)", ex);
				throw;
			}
		}

		public delegate object CreatorDelegate(EditorData data);

		public static CreatorDelegate FindCreator(Type type)
		{
			if (creatorCache.ContainsKey(type.AssemblyQualifiedName))
			{
				return creatorCache[type.AssemblyQualifiedName];
			}

			if (type.IsEditable(out EditableAttribute attribute))
			{
				string methodName = attribute.CreatorMethodName;
				if (methodName is null)
				{
					foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (method.GetCustomAttribute<CreatorAttribute>() != null)
						{
							CreatorDelegate creatorDelegate = (CreatorDelegate)method.CreateDelegate(typeof(CreatorDelegate));
							// Store the name so it doesn't have to be searched for again.
							attribute.CreatorMethodName = method.Name;
							// Cache the result.
							creatorCache.Add(type.AssemblyQualifiedName, creatorDelegate);
							// TODO: try... catch etc
							return creatorDelegate;
						}
					}
				}
				else
				{
					CreatorDelegate creatorDelegate = (CreatorDelegate)type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).CreateDelegate(typeof(CreatorDelegate));
					// Cache the delegate to avoid having to use reflection to retrieve it again.
					creatorCache.Add(type.AssemblyQualifiedName, creatorDelegate);
					// TODO: try... catch etc
					return creatorDelegate;
				}
			}

			return null;
		}
	}
}