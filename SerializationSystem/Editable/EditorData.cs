using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	[DataContract]
	// TODO: Maybe EditorData should store ImaginaryObjects instead? (Should create them when accessed aswell etc.)
	public class EditorData : IDictionary<string, string>, IEquatable<EditorData>
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
				{
					return null;
				}

				return DataDictionary[dataName];
			}

			set
			{
				if (!DataDictionary.ContainsKey(dataName))
				{
					DataDictionary.Add(dataName, value);
				}
				else
				{
					DataDictionary[dataName] = value;
				}
			}
		}
		[DataMember]
		private Dictionary<string, string> DataDictionary = new Dictionary<string, string>();

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
			{
				return false;
			}
			else
			{
				return Equals((EditorData)obj);
			}
		}

		public bool Equals(EditorData other)
		{
			if (other is null)
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
			if (!(left is null || right is null))
			{
				return left.Equals(right);
			}
			else
			{
				return false;
			}
		}

		public static bool operator !=(EditorData left, EditorData right)
		{
			if (!(left is null || right is null))
			{
				return !(left == right);
			}
			else
			{
				return false;
			}
		}
		#endregion
	}
}