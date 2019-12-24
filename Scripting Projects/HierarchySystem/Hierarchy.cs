using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalClear.HierarchySystem
{
	[Serializable]
	public class Hierarchy : IDictionary<string, HierarchyObject> // TODO maybe make this generic and allow different types of Hierarchies to be formed from this? Should in that case also be in ScriptUtilities...
	{
		public Hierarchy()
		{

		}

		public Hierarchy(Action toSubscribe)
		{
			OnHierarchyChange += toSubscribe;
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

		public void ClearOnHierarchyChange()
		{
			OnHierarchyChange = null;
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
		/// <param name="child">The HierarchyObject that should recieve a name change</param>
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
		/// <param name="currentName">The HierarchyObject that should recieve a name change</param>
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
	}
}
