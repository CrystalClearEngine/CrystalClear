using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

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

	[Serializable]
	[DataContract]
	// TODO: Maybe EditorData should store ImaginaryObjects instead? (Should create them when accessed aswell etc.)
	public struct EditorData : IDictionary<string, string>, IEquatable<EditorData>
	{
		public static EditorData GetEmpty()
		{
			return new EditorData();
		}

		public string this[string dataName]
		{
			get
			{
				if (!DataDictionary.ContainsKey(dataName))
					return null;
				return DataDictionary[dataName];
			}

			set
			{
				if (!DataDictionary.ContainsKey(dataName))
					DataDictionary.Add(dataName, value);
				else
					DataDictionary[dataName] = value;
			}
		}

		private Dictionary<string, string> dataDictionary;
		private Dictionary<string, string> DataDictionary
		{
			get
			{
				if (dataDictionary == null)
				{
					dataDictionary = new Dictionary<string, string>();
				}
				return dataDictionary;
			}
			set
			{
				dataDictionary = value;
			}
		}

		#region Dictionary Implementation
		public ICollection<string> Keys => ((IDictionary<string, string>)DataDictionary).Keys;

		public ICollection<string> Values => ((IDictionary<string, string>)DataDictionary).Values;

		public int Count => ((IDictionary<string, string>)DataDictionary).Count;

		public bool IsReadOnly => ((IDictionary<string, string>)DataDictionary).IsReadOnly;

		public bool ContainsKey(string key) => DataDictionary.ContainsKey(key);

		public void Add(string key, string value) => DataDictionary.Add(key, value);

		public bool Remove(string key) => DataDictionary.Remove(key);

		public bool TryGetValue(string key, out string value) => DataDictionary.TryGetValue(key, out value);

		public void Add(KeyValuePair<string, string> item) => Add(item.Key, item.Value);

		public void Clear() => DataDictionary.Clear();

		public bool Contains(KeyValuePair<string, string> item) => throw new NotImplementedException();

		void ICollection<KeyValuePair<string, string>>.CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) => throw new NotSupportedException();

		public bool Remove(KeyValuePair<string, string> item) => Remove(item.Key);

		public IEnumerator<KeyValuePair<string, string>> GetEnumerator() => DataDictionary.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => DataDictionary.GetEnumerator();
		#endregion

		#region Equality
		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(EditorData))
				return false;
			else
				return Equals((EditorData)obj);
		}

		public bool Equals(EditorData other)
		{
			if (other == null)
			{
				return false;
			}

			return DataDictionary.Equals(other.DataDictionary);
		}

		public override int GetHashCode()
		{
			return DataDictionary.GetHashCode();
		}

		public static bool operator ==(EditorData left, EditorData right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(EditorData left, EditorData right)
		{
			return !(left == right);
		}
		#endregion
	}

	public static class EditableSystem
	{
		public static bool IsEditable(this Type type, out EditableAttribute editableAttribute)
		{
			foreach (object attribute in type.GetCustomAttributes(true))
				if (attribute.GetType() == typeof(EditableAttribute))
				{
					editableAttribute = (EditableAttribute)attribute;
					return true;
				}

			editableAttribute = null;
			return false;
		}

		public static bool IsEditable(this Type type) => IsEditable(type, out _);

		public delegate void EditorDelegate(ref EditorData editorData);

		public static void OpenEditor(Type type, ref EditorData current)
		{
			if (!type.IsEditable())
				throw new ArgumentException("This type is not editable.");

			FindEditor(type)(ref current);
		}

		public static EditorDelegate FindEditor(Type type)
		{
			if (type.IsEditable(out EditableAttribute attribute))
			{
				string methodName = attribute.EditorMethodName;
				if (methodName != null)
					// TODO: try... catch etc
					return (EditorDelegate)type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).CreateDelegate(typeof(EditorDelegate));
				else
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
			}

			return null;
		}

		public static TCreatorType Create<TCreatorType>(Type type, EditorData data)
		{
			return FindCreator<TCreatorType>(type)(data);
		}

		public static Func<EditorData, TCreatorType> FindCreator<TCreatorType>(Type type)
		{
			if (type.IsEditable(out EditableAttribute attribute))
			{
				string methodName = attribute.EditorMethodName;
				if (methodName != null)
					// TODO: try... catch etc
					return (Func<EditorData, TCreatorType>)type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).CreateDelegate(typeof(Func<EditorData, TCreatorType>));
				else
				{
					foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						if (method.GetCustomAttribute<CreatorAttribute>() != null)
						{
							// Cache the result.
							attribute.CreatorMethodName = method.Name;
							// TODO: try... catch etc
							return (Func<EditorData, TCreatorType>)method.CreateDelegate(typeof(Func<EditorData, TCreatorType>));
						}
					}
				}
			}

			return null;
		}
	}
}