using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CrystalClear.HierarchySystem
{
	public class Hierarchy
		: IDictionary<string, HierarchyObject>
		, IEquatable<Hierarchy>
	{ // TODO maybe implement IPropertyChanged?
		public Hierarchy(HierarchyObject owner)
		{
			OnHierarchyChange += owner.OnLocalHierarchyChange;
		}

		private Dictionary<string, HierarchyObject> hierarchy = new Dictionary<string, HierarchyObject>();

		public HierarchyObject this[string index]
		{
			get => hierarchy[index];
			set
			{
				hierarchy[index] = value;
				OnHierarchyChange();
			}
		}

		/// <summary>
		/// This event is called when a change in the Hierarchy has been made, such as changing the value of a child, or adding or removing a child.
		/// </summary>
		public event Action OnHierarchyChange;

		#region Properties
		public ICollection<string> Keys => hierarchy.Keys;

		public ICollection<HierarchyObject> Values => hierarchy.Values;

		public int Count => hierarchy.Count;

		public bool IsReadOnly => false;
		#endregion

		#region Management
		/// <summary>
		/// This method sets the name of the specified child to the specified new key. 
		/// </summary>
		/// <param name="child">The HierarchyObject that should receive a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(HierarchyObject child, string newName)
		{
			string key = GetChildName(child);
			RemoveChild(key);
			AddChild(newName, child);
		}

		/// <summary>
		/// This method sets the name of the specified key in the Hierarchy to the new key. 
		/// </summary>
		/// <param name="currentName">The HierarchyObject that should receive a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(string currentName, string newName)
		{
			HierarchyObject hierarchyObject = hierarchy[currentName];
			RemoveChild(currentName);
			AddChild(newName, hierarchyObject);
		}

		/// <summary>
		/// Returns the name of a child HierarchyObject that is present in the Hierarchy of this HierarchyObject.
		/// </summary>
		/// <param name="child">The HierarchyObject to get the name of.</param>
		/// <returns>The name of this object.</returns>
		public string GetChildName(HierarchyObject child)
		{
			string key = hierarchy.First(x => ReferenceEquals(x.Value, child)).Key;
			return key;
		}

		/// <summary>
		/// Adds a HierarchyObject to the Hierarchy and also set the HierarchyObject's parent accordingly
		/// </summary>
		/// <param name="name">The name of the HierarchyObject to add.</param>
		/// <param name="child">The HierarchyObject to add.</param>
		public void AddChild(string name, HierarchyObject child)
		{
			hierarchy.Add(name, child);
			OnHierarchyChange?.Invoke();
		}

		/// <summary>
		/// Removes the specified child specified by HierarchyObject from the Hierarchy.
		/// </summary>
		/// <param name="child">The child HierarchyObject to remove.</param>
		public void RemoveChild(HierarchyObject child)
		{
			string key = hierarchy.First(HierarchyObject => HierarchyObject.Value == child).Key;
			RemoveChild(key);
		}

		/// <summary>
		/// Removes the specified child specified by name from the Hierarchy.
		/// </summary>
		/// <param name="childName">The child's name.</param>
		public bool RemoveChild(string childName)
		{
			bool okBooler = hierarchy.Remove(childName);
			OnHierarchyChange();
			return okBooler;
		}

		#region Dictionary Implementation
		public bool ContainsKey(string key) => hierarchy.ContainsKey(key);

		public void Add(string key, HierarchyObject value) => AddChild(key, value);

		public bool Remove(string key) => RemoveChild(key);

		public bool TryGetValue(string key, out HierarchyObject value) => hierarchy.TryGetValue(key, out value);

		public void Add(KeyValuePair<string, HierarchyObject> item) => AddChild(item.Key, item.Value);

		public void Clear() => hierarchy.Clear();

		public bool Contains(KeyValuePair<string, HierarchyObject> item) => hierarchy.Contains(item);

		void ICollection<KeyValuePair<string, HierarchyObject>>.CopyTo(KeyValuePair<string, HierarchyObject>[] array, int arrayIndex) => throw new NotSupportedException();

		public bool Remove(KeyValuePair<string, HierarchyObject> item) => RemoveChild(item.Key);

		public IEnumerator<KeyValuePair<string, HierarchyObject>> GetEnumerator() => hierarchy.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => hierarchy.GetEnumerator();
		#endregion
		#endregion

		public override bool Equals(object obj)
		{
			return Equals(obj as Hierarchy);

			//if (!(obj is Hierarchy))
			//	return false;

			//Hierarchy hierarchy = (Hierarchy)obj;

			//return (this.hierarchy.Equals(hierarchy.hierarchy) &
			//	this.OnHierarchyChange.Equals(hierarchy.OnHierarchyChange));
		}

		public bool Equals(Hierarchy other)
		{
			return other != null &&
				hierarchy.Equals(other.hierarchy) || hierarchy.Count == 0 && other.hierarchy.Count == 0;
		}

		public override int GetHashCode()
		{
			return 218564712 + EqualityComparer<Dictionary<string, HierarchyObject>>.Default.GetHashCode(hierarchy);
		}

		public static bool operator ==(Hierarchy left, Hierarchy right)
		{
			return EqualityComparer<Hierarchy>.Default.Equals(left, right);
		}

		public static bool operator !=(Hierarchy left, Hierarchy right)
		{
			return !(left == right);
		}
	}

	public struct Hierarchy<T>
		: IDictionary<string, T>
		, IEquatable<Hierarchy<T>>
	{
		public Hierarchy(Dictionary<string, T> baseDictionary = null)
		{
			hierarchy = baseDictionary is null ? new Dictionary<string, T>() : baseDictionary;
			OnHierarchyChange = null;
		}

		private Dictionary<string, T> hierarchy;

		public T this[string index]
		{
			get => hierarchy[index];
			set
			{
				hierarchy[index] = value;
				OnHierarchyChange();
			}
		}

		/// <summary>
		/// This event is called when a change in the Hierarchy has been made, such as changing the value of a child, or adding or removing a child.
		/// </summary>
		public event Action OnHierarchyChange;

		#region Properties
		public ICollection<string> Keys => hierarchy.Keys;

		public ICollection<T> Values => hierarchy.Values;

		public int Count => hierarchy.Count;

		public bool IsReadOnly => false;
		#endregion

		#region Management
		/// <summary>
		/// This method sets the name of the specified child to the specified new key. 
		/// </summary>
		/// <param name="child">The T that should receive a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(T child, string newName)
		{
			string key = GetChildName(child);
			RemoveChild(key);
			AddChild(newName, child);
		}

		/// <summary>
		/// This method sets the name of the specified key in the Hierarchy to the new key. 
		/// </summary>
		/// <param name="currentName">The T that should receive a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(string currentName, string newName)
		{
			T T = hierarchy[currentName];
			RemoveChild(currentName);
			AddChild(newName, T);
		}

		/// <summary>
		/// Returns the name of a child T that is present in the Hierarchy of this T.
		/// </summary>
		/// <param name="child">The T to get the name of.</param>
		/// <returns>The name of this object.</returns>
		public string GetChildName(T child)
		{
			string key = hierarchy.First(x => ReferenceEquals(x.Value, child)).Key;
			return key;
		}

		/// <summary>
		/// Adds a T to the Hierarchy and also set the T's parent accordingly
		/// </summary>
		/// <param name="name">The name of the T to add.</param>
		/// <param name="child">The T to add.</param>
		public void AddChild(string name, T child)
		{
			hierarchy.Add(name, child);
			OnHierarchyChange?.Invoke();
		}

		/// <summary>
		/// Removes the specified child specified by T from the Hierarchy.
		/// </summary>
		/// <param name="child">The child T to remove.</param>
		public void RemoveChild(T child)
		{
			string key = hierarchy.First(T => T.Value.Equals(child)).Key;
			RemoveChild(key);
		}

		/// <summary>
		/// Removes the specified child specified by name from the Hierarchy.
		/// </summary>
		/// <param name="childName">The child's name.</param>
		public bool RemoveChild(string childName)
		{
			bool okBooler = hierarchy.Remove(childName);
			OnHierarchyChange();
			return okBooler;
		}

		#region Dictionary Implementation
		public bool ContainsKey(string key) => hierarchy.ContainsKey(key);

		public void Add(string key, T value) => AddChild(key, value);

		public bool Remove(string key) => RemoveChild(key);

		public bool TryGetValue(string key, out T value) => hierarchy.TryGetValue(key, out value);

		public void Add(KeyValuePair<string, T> item) => AddChild(item.Key, item.Value);

		public void Clear() => hierarchy.Clear();

		public bool Contains(KeyValuePair<string, T> item) => hierarchy.Contains(item);

		void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) => throw new NotSupportedException();

		public bool Remove(KeyValuePair<string, T> item) => RemoveChild(item.Key);

		public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => hierarchy.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => hierarchy.GetEnumerator();
		#endregion
		#endregion

		public override bool Equals(object obj)
		{
			return Equals(obj as Hierarchy);

			//if (!(obj is Hierarchy))
			//	return false;

			//Hierarchy hierarchy = (Hierarchy)obj;

			//return (this.hierarchy.Equals(hierarchy.hierarchy) &
			//	this.OnHierarchyChange.Equals(hierarchy.OnHierarchyChange));
		}

		public bool Equals(Hierarchy<T> other)
		{
			return hierarchy.Equals(other.hierarchy) || hierarchy.Count == 0 && other.hierarchy.Count == 0;
		}

		public override int GetHashCode()
		{
			return 218564712 + EqualityComparer<Dictionary<string, T>>.Default.GetHashCode(hierarchy);
		}

		public static bool operator ==(Hierarchy<T> left, Hierarchy<T> right)
		{
			return EqualityComparer<Hierarchy<T>>.Default.Equals(left, right);
		}

		public static bool operator !=(Hierarchy<T> left, Hierarchy<T> right)
		{
			return !(left == right);
		}
	}
}
