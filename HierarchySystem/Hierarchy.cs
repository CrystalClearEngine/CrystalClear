using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CrystalClear.HierarchySystem
{
	public sealed class Hierarchy
		: IDictionary<string, HierarchyObject>
			, IEquatable<Hierarchy>
			, INotifyPropertyChanged
	{
		private readonly Dictionary<string, HierarchyObject> hierarchy = new Dictionary<string, HierarchyObject>();

		public Hierarchy(HierarchyObject owner)
		{
			OnHierarchyChange += () => OnPropertyChanged();
			OnHierarchyChange += owner.OnLocalHierarchyChange;
		}

		public HierarchyObject this[string index]
		{
			get => hierarchy[index];
			set
			{
				hierarchy[index] = value;
				OnHierarchyChange();
			}
		}

		public bool Equals(Hierarchy other) =>
			other is not null &&
			hierarchy.Equals(other.hierarchy) || hierarchy.Count == 0 && other.hierarchy.Count == 0;

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		///     This event is raised when a change in the Hierarchy has been made, such as changing the value of a child, or adding
		///     or removing a child.
		/// </summary>
		public event Action OnHierarchyChange;

		public override bool Equals(object obj) => Equals(obj as Hierarchy);

		//if (!(obj is Hierarchy))
		//	return false;
		//Hierarchy hierarchy = (Hierarchy)obj;
		//return (this.hierarchy.Equals(hierarchy.hierarchy) &
		//	this.OnHierarchyChange.Equals(hierarchy.OnHierarchyChange));
		public override int GetHashCode() =>
			218564712 + EqualityComparer<Dictionary<string, HierarchyObject>>.Default.GetHashCode(hierarchy);

		public static bool operator ==(Hierarchy left, Hierarchy right) =>
			EqualityComparer<Hierarchy>.Default.Equals(left, right);

		public static bool operator !=(Hierarchy left, Hierarchy right) => !(left == right);

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#region Properties

		public ICollection<string> Keys => hierarchy.Keys;

		public ICollection<HierarchyObject> Values => hierarchy.Values;

		public int Count => hierarchy.Count;

		public bool IsReadOnly => false;

		#endregion

		#region Management

		/// <summary>
		///     This method sets the name of the specified child to the specified new key.
		/// </summary>
		/// <param name="child">The HierarchyObject that should receive a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(HierarchyObject child, string newName)
		{
			var key = GetChildName(child);
			RemoveChild(key);
			AddChild(newName, child);
		}

		/// <summary>
		///     This method sets the name of the specified key in the Hierarchy to the new key.
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
		///     Returns the name of a child HierarchyObject that is present in the Hierarchy of this HierarchyObject.
		/// </summary>
		/// <param name="child">The HierarchyObject to get the name of.</param>
		/// <returns>The name of this object.</returns>
		public string GetChildName(HierarchyObject child)
		{
			var key = hierarchy.First(x => ReferenceEquals(x.Value, child)).Key;
			return key;
		}

		/// <summary>
		///     Adds a HierarchyObject to the Hierarchy and also set the HierarchyObject's parent accordingly
		/// </summary>
		/// <param name="name">The name of the HierarchyObject to add.</param>
		/// <param name="child">The HierarchyObject to add.</param>
		public void AddChild(string name, HierarchyObject child)
		{
			hierarchy.Add(name, child);
			OnHierarchyChange?.Invoke();
		}

		/// <summary>
		///     Removes the specified child specified by HierarchyObject from the Hierarchy.
		/// </summary>
		/// <param name="child">The child HierarchyObject to remove.</param>
		public void RemoveChild(HierarchyObject child)
		{
			var key = hierarchy.First(HierarchyObject => HierarchyObject.Value == child).Key;
			RemoveChild(key);
		}

		/// <summary>
		///     Removes the specified child specified by name from the Hierarchy.
		/// </summary>
		/// <param name="childName">The child's name.</param>
		public bool RemoveChild(string childName)
		{
			var success = hierarchy.Remove(childName);
			OnHierarchyChange();
			return success;
		}

		#region Dictionary Implementation

		public bool ContainsKey(string key) => hierarchy.ContainsKey(key);

		public void Add(string key, HierarchyObject value)
		{
			AddChild(key, value);
		}

		public bool Remove(string key) => RemoveChild(key);

		public bool TryGetValue(string key, out HierarchyObject value) => hierarchy.TryGetValue(key, out value);

		public void Add(KeyValuePair<string, HierarchyObject> item)
		{
			AddChild(item.Key, item.Value);
		}

		public void Clear()
		{
			hierarchy.Clear();
		}

		public bool Contains(KeyValuePair<string, HierarchyObject> item) => hierarchy.Contains(item);

		void ICollection<KeyValuePair<string, HierarchyObject>>.CopyTo(KeyValuePair<string, HierarchyObject>[] array,
			int arrayIndex)
		{
			throw new NotSupportedException();
		}

		public bool Remove(KeyValuePair<string, HierarchyObject> item) => RemoveChild(item.Key);

		public IEnumerator<KeyValuePair<string, HierarchyObject>> GetEnumerator() => hierarchy.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => hierarchy.GetEnumerator();

		#endregion

		#endregion
	}
}