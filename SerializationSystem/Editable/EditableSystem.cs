using System;
using System.Reflection;

namespace CrystalClear.SerializationSystem
{
	public static class EditableSystem
	{
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
							// Cache the result.
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
			if (type.IsEditable(out EditableAttribute attribute))
			{
				string methodName = attribute.CreatorMethodName;
				if (methodName is null)
				{
					foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (method.GetCustomAttribute<CreatorAttribute>() != null)
						{
							// Cache the result.
							attribute.CreatorMethodName = method.Name;
							// TODO: try... catch etc
							return (CreatorDelegate)method.CreateDelegate(typeof(CreatorDelegate));
						}
					}
				}
				else
				{
					// TODO: try... catch etc
					return (CreatorDelegate)type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).CreateDelegate(typeof(CreatorDelegate));
				}
			}

			return null;
		}
	}
}