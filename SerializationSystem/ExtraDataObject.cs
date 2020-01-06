using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public struct ExtraDataObject : IDictionary<string, object>
	{
		[DataMember]
		private Dictionary<string, object> data;

		public Dictionary<string, object> Data
		{
			get
			{
				if (data == null)
				{
					data = new Dictionary<string, object>();
				}
				return data;
			}

			set
			{
				if (data == null)
				{
					data = new Dictionary<string, object>();
				}
				data = value;
			}
		}

		public object this[string valueName]
		{
			get => Data[valueName];
			set => Data[valueName] = value;
		}

		#region IDictionary Implementation
		public ICollection<string> Keys => ((IDictionary<string, object>)Data).Keys;

		public ICollection<object> Values => ((IDictionary<string, object>)Data).Values;

		public int Count => ((IDictionary<string, object>)Data).Count;

		public bool IsReadOnly => ((IDictionary<string, object>)Data).IsReadOnly;

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, object>)Data).ContainsKey(key);
		}

		public void Add(string key, object value)
		{
			((IDictionary<string, object>)Data).Add(key, value);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, object>)Data).Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return ((IDictionary<string, object>)Data).TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			((IDictionary<string, object>)Data).Add(item);
		}

		public void Clear()
		{
			((IDictionary<string, object>)Data).Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>)Data).Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((IDictionary<string, object>)Data).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>)Data).Remove(item);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return ((IDictionary<string, object>)Data).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, object>)Data).GetEnumerator();
		}
		#endregion
	}
}
