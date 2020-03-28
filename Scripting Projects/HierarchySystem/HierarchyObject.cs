using CrystalClear.HierarchySystem.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CrystalClear.HierarchySystem
{
	/// <summary>
	/// A HierarchyObject is an object that exists in a Hierarchy, can have child objects and which can have HierarchyScripts attatched.
	/// </summary>
	public abstract class HierarchyObject
									// TODO probably limit naming to alphabetic only.
	{
		#region Virtual Event Methods
		// Overrideable event methods.
		// Q: Why not use events?
		// A: They should be implementable in deriving classes without needing to set up the subsriptions.

		/// <summary>
		/// OnLocalHierarchyChange is called when the LocalHierarchy is modified.
		/// </summary>
		protected internal virtual void OnLocalHierarchyChange()
		{

		}

		/// <summary>
		/// OnReparent is called when the HierarchyObject's parent updates.
		/// </summary>
		/// <param name="newParent">The new parent.</param>
		protected internal virtual void OnReparent(HierarchyObject newParent)
		{

		}
		#endregion

		#region Script Handling
		/// <summary>
		/// Adds a Script to the HierarchyObject using the type as name.
		/// </summary>
		/// <param name="hierarchyObject">The HierarchyObject to add the Script to.</param>
		/// <param name="script">The Script to add to the HierarchyObject.</param>
		/// <returns>The resulting HierarchyObject.</returns>
		
		public static HierarchyObject operator +(HierarchyObject hierarchyObject, Script script)
		{
			HierarchyObject result = hierarchyObject;
			result.AddScriptManually(script);
			return result;
		}

		public static HierarchyObject operator +(HierarchyObject hierarchyObject, Type scriptType)
		{
			HierarchyObject result = hierarchyObject;
			result.AddScript(scriptType);
			return result;
		}

		/// <summary>
		/// The Scripts that are currently attatched to this object.
		/// </summary>
		public Dictionary<string, Script> AttatchedScripts = new Dictionary<string, Script>();

		/// <summary>
		/// Adds a HiearchyScript based on the specified type to this HierarchyObject.
		/// </summary>
		/// <param name="scriptType">The type of the Script to add.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public void AddHierarchyScript(Type scriptType, object[] constructorParameters = null)
		{
			AddScriptManually(new Script(this, scriptType, constructorParameters));
		}

		/// <summary>
		/// Adds a Script of any type other than HierarchyScript to this HiearchyObject.
		/// </summary>
		/// <param name="scriptType">The type of the Script to add.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public void AddNonHiearchyScriptScript(Type scriptType, object[] constructorParameters = null)
		{
			AddScriptManually(new Script(scriptType, constructorParameters));
		}

		/// <summary>
		/// Adds a Script of any type to this HiearchyObject. If this is a HierarchyScript or not will be automatically detected.
		/// </summary>
		/// <param name="scriptType">The type of the Script to add.</param>
		/// <param name="constructorParameters">The parameters to use for the constructor.</param>
		public void AddScript(Type scriptType, object[] constructorParameters = null)
		{
			AddScriptManually(new Script(scriptType, constructorParameters, this));
		}

		public void AddScripts(Type[] scriptTypes, object[][] constructorParameters = null)
		{
			if (constructorParameters != null && scriptTypes.Length != constructorParameters.Length)
			{
				throw new ArgumentException("Incorrect scriptType/ConstructorParameter count!");
			}
			for (int i = 0; i < scriptTypes.Length; i++)
			{
				Type scriptType = scriptTypes[i];
				object[] constructorParameter = null;
				if (constructorParameters != null)
				{
					constructorParameter = constructorParameters[i];
				}

				AddScriptManually(new Script(scriptType, constructorParameter, this));
			}
		}

		public void AddScripts(string[] names, Script[] scripts)
		{
			if (names.Length != scripts.Length)
			{
				throw new ArgumentException("Parameter 'names' and 'scripts are not equal lengths.'");
			}

			for (int i = 0; i < scripts.Length; i++)
			{
				AddScriptManually(scripts[i], names[i]);
			}
		}

		/// <summary>
		/// Adds a Script directly to this HierarchyObject. Note that this will *not* automatically attatch the Script to the HierachyObject.
		/// </summary>
		/// <param name="script">The Script to add.</param>
		public void AddScriptManually(Script script, string name = null)
		{
			// Replace name with default name if not provided.
			if (name == null)
			{
				name = Utilities.EnsureUniqueName(script.ScriptType.Name, AttatchedScripts.Keys);
			}

			AttatchedScripts.Add(name, script);
		}
		#endregion

		#region Helper Properties
		/// <summary>
		/// Is this HierarchyObject the root of the entire hierarchy (has no parent)?
		/// </summary>
		public bool IsRoot
		{
			get
			{
				// If this HierarchyObject has no parent that means that it has to be at the root, since HierarchyManager cannot be used as parent.
				return (Parent == null);
			}
		}

		/// <summary>
		/// Checks wether or not this Hierarchy is in the LoadedHierarchies' list.
		/// </summary>
		public bool IsLoadedHierarchy
		{
			get
			{
				return HierarchyManager.LoadedHierarchies.Values.Contains(Root);
			}
		}

		/// <summary>
		/// Returns the root of this Hierarchy.
		/// </summary>
		public HierarchyObject Root
		{
			get
			{
				// Is this the root?
				if (IsRoot)
				{
					return this;
				}
				// If not, then we should refer to our parent. They should know :).
				else
				{
					return Parent.Root;
				}
			}
		}

		/// <summary>
		/// Returns the name of the containing hierarchy, or calls the HierarchyManager's rename method if set.
		/// </summary>
		public string HierarchyName
		{
			get => HierarchyManager.GetHierarchyName(Root);
			set
			{
				HierarchyManager.SetHierarchyName(Root, value);
			}
		}

		/// <summary>
		/// Returns the name of this HierarchyObject by looking it up via GetName() on the parent (or HierarchyManager if this HierarchyObject is the root). This property supports setting the name, which uses the SetName() method on the parent or HierarchyManager if this Hierarchy object is the root.
		/// </summary>
		public string Name
		{
			get
			{
				if (IsRoot == false)
				{
					return Parent.GetChildName(this);
				}
				else
				{
					return HierarchyManager.GetHierarchyName(this);
				}
			}
			set => Parent.SetChildName(this, value);
		}

		/// <summary>
		/// The entire current hierarchy from the root, for scripts modifying pleasure.
		/// </summary>
		internal Hierarchy Hierarchy => Root.LocalHierarchy;

		public string Path // TODO document this
		{
			get
			{
				string path = Name;

				if (!IsRoot)
				{
					string parentPath = Parent.Path;
					path = path.Insert(0, parentPath + @"\");
				}

				return path;
			}
		}
		#endregion

		#region Hierarchy Management
		/// <summary>
		/// The local hierarchy, containing all child HierarchyObjects that this HierarchyObject has.
		/// </summary>
		private Hierarchy localHierarchy = null;
		public Hierarchy LocalHierarchy
		{
			get
			{
				if (localHierarchy == null)
				{
					localHierarchy = new Hierarchy(this);
				}
				return localHierarchy;
			}
		}

		/// <summary>
		/// Accesses the LocalHierarchy of this HierarchyObject.
		/// </summary>
		public HierarchyObject this[string index]
		{
			get
			{
				return LocalHierarchy[index];
			}
			set
			{
				LocalHierarchy[index] = value;
			}
		}

		/// <summary>
		/// The field referencing this HierarchyObject's parent in the Hierarchy.
		/// </summary>
		private WeakReference<HierarchyObject> parent = new WeakReference<HierarchyObject>(null);
		/// <summary>
		/// Returns the parent, or utlizes ReParentChild() to set it.
		/// </summary>
		public HierarchyObject Parent
		{
			get
			{
				parent.TryGetTarget(out HierarchyObject hierarchyObject);/* ?? throw new Exception("This HierarchyObject has no parent! Please check using IsRoot beforehand.")*/;
				return hierarchyObject;
			}

			set
			{
				ReParentThis(value);
			}
		}


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
		/// This method sets the name of the specified key in the LocalHierarchy to the new key. 
		/// </summary>
		/// <param name="currentName">The HierarchyObject that should recieve a name change</param>
		/// <param name="newName">The new name for the child</param>
		public void SetChildName(string currentName, string newName)
		{
			HierarchyObject hierarchyObject = LocalHierarchy[currentName];
			RemoveChild(currentName);
			AddChild(newName, hierarchyObject);
		}

		/// <summary>
		/// Returns the name of a child HierarchyObject that is present in the LocalHierarchy of this HierarchyObject.
		/// </summary>
		/// <param name="child">The HierarchyObject to get the name of.</param>
		/// <returns>The name of this object.</returns>
		public string GetChildName(HierarchyObject child)
		{
			string key = LocalHierarchy.First(x => ReferenceEquals(x.Value, child)).Key;
			return key;
		}

		/// <summary>
		/// Adds a HierarchyObject to the LocalHierarchy and also set the HierarchyObject's parent accordingly
		/// </summary>
		/// <param name="name">The name of the HierarchyObject to add.</param>
		/// <param name="child">The HierarchyObject to add.</param>
		public void AddChild(string name, HierarchyObject child)
		{
			LocalHierarchy.AddChild(name, child);
			child.SetUp(this);
		}

		/// <summary>
		/// Changes the parent of a HierarchyObject in the LocalHierarchy.
		/// </summary>
		/// <param name="newParent">The parent to move the child to.</param>
		/// <param name="child">The child to move.</param>
		public void ReParentChild(HierarchyObject newParent, HierarchyObject child)
		{
			string childName = child.Name;
			RemoveChild(child);
			newParent.AddChild(childName, child);
		}

		/// <summary>
		/// Changes the parent of a HierarchyObject in a Hierarchy.
		/// </summary>
		/// <param name="oldParent">The parent to remove the HierarchyObject from.</param>
		/// <param name="newParent">The parent to add the HierarchyObject to.</param>
		/// <param name="child">The child object to re-parent.</param>
		public static void ReParent(HierarchyObject oldParent, HierarchyObject newParent, HierarchyObject child)
		{
			string childName = child.Name;
			oldParent.RemoveChild(child);
			newParent.AddChild(childName, child);
		}

		/// <summary>
		/// Changes the parent of this HierarchyObject.
		/// </summary>
		/// <param name="newParent">The parent to add the HierarchyObject to.</param>
		public void ReParentThis(HierarchyObject newParent)
		{
			string childName = Name;
			if (!IsRoot)
			{
				Parent.RemoveChild(childName);
			}
			newParent.AddChild(childName, this);
		}

		/// <summary>
		/// Sets up the HierarchyObject.
		/// </summary>
		/// <param name="parent">Optional parent override.</param>
		public void SetUp(HierarchyObject parent = null)
		{
			if (Parent == null && parent == null) // Parent null check.
			{
				throw new ArgumentException("No parent specified! Please set the parent before calling or include it as a parameter.");
			}

			if (parent != null) // The parent parameter isn't at default value, need to set the current object parent.
			{
				this.parent.SetTarget(parent);
			}

			OnReparent(Parent);
		}

		/// <summary>
		/// Removes the specified child specified by HierarchyObject from the LocalHierarchy.
		/// </summary>
		/// <param name="child">The child HierarchyObject to remove.</param>
		public void RemoveChild(HierarchyObject child)
		{
			string key = LocalHierarchy.First(HierarchyObject => HierarchyObject.Value == child).Key;

			RemoveChild(key);
		}

		/// <summary>
		/// Removes the specified child specified by name from the LocalHierarchy.
		/// </summary>
		/// <param name="childName">The child's name.</param>
		public void RemoveChild(string childName)
		{
			LocalHierarchy.RemoveChild(childName);
		}

		/// <summary>
		/// Follows a path relatively from this point and returns the specified HierarchyObject.
		/// </summary>
		/// <param name="path">The path to follow. HierarchyObjects are separated by '/'.</param>
		/// <returns>The HierarchyObject at the end of the path.</returns>
		public HierarchyObject FollowPath(string path)
		{
			string[] pathSegments = path.Split('/'); // Split the path into the individual HierarchyObjects to follow.

			if (pathSegments.Length == 1) // We have reached the end of the path, this HierarchyObject is the destination!
			{
				return this; // Return this HierarchyObject.
			}

			string pathToFollow = path.Remove(0, pathSegments[0].Length + 1); // Remove the top level HierarchyObject from the path as we continue forwards.
			string nextObject = pathSegments[1]; // The next HierarchyObject's name.

			return LocalHierarchy[nextObject].FollowPath(pathToFollow); // We are going to do some of that sweet bitter sweet recursion magic by returning the result of a follow call to the HierarchyObject that is next in the path.
		}
		#endregion

		#region Finding
		/// <summary>
		/// <summary>
		/// Finds all types that derive from HierarchyObject and returns them.
		/// </summary>
		/// <param name="assembly">The assembly to find the HierarchyObjects in.</param>
		/// <returns>The found HierarchyObjects.</returns>
		public static Type[] FindHierarchyObjectTypesInAssembly(Assembly assembly) => FindHierarchyObjectTypesInTypes(assembly.GetTypes());

		/// <summary>
		/// Finds all types that derive from HierarchyObject and returns them.
		/// </summary>
		/// <param name="types">The types to find the HierarchyObjects in.</param>
		/// <returns>The found HierarchyObjects.</returns>
		public static Type[] FindHierarchyObjectTypesInTypes(params Type[] types)
		{
			// Find and store the found HierarchyObject types.
			Type[] customHierarchyObjects = (from type in types // Iterator variable.
											 where IsHierarchyObject(type) // Is the type a HierarchyObject?
											 select type).ToArray();
			// Return the found HierarchyObjects.
			return customHierarchyObjects;
		}

		/// <summary>
		/// Checks wether the provided type derives from HierarchyObject.
		/// </summary>
		/// <param name="type">The type to check wether it derives from HierarchyObject.</param>
		/// <returns>"ether the provided type derives from HierarchyObject.</returns>
		public static bool IsHierarchyObject(Type type)
		{
			return type.IsSubclassOf(typeof(HierarchyObject));
		}
		#endregion
	}
}
