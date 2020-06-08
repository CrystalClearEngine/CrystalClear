using CrystalClear.HierarchySystem;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	/// <summary>
	/// ImaginaryHierarchyObject is an derivative of ImaginaryObject that is designed specifically for storing HierarchyObjects and the extra data that goes along with it.
	/// </summary>
	[DataContract]
	public class ImaginaryHierarchyObject : ImaginaryObject
	{
		/// <summary>
		/// Contains the children of this ImaginaryHierarchyObject.
		/// </summary>
		[DataMember]
		public virtual Dictionary<string, ImaginaryHierarchyObject> LocalHierarchy { get; set; } = new Dictionary<string, ImaginaryHierarchyObject>();

		/// <summary>
		/// Contains the scripts which will be added to the HierarchyObject when an instance is created.
		/// </summary>
		[DataMember]
		public virtual Dictionary<string, ImaginaryScript> AttatchedScripts { get; set; } = new Dictionary<string, ImaginaryScript>();

		/// <summary>
		/// A weak reference to the parent of this ImaginaryHierarchyObject. Not serialized.
		/// </summary>
		public virtual ImaginaryHierarchyObject Parent
		{
			get
			{
				parent.TryGetTarget(out ImaginaryHierarchyObject editorHierarchyObject);
				return editorHierarchyObject;
			}
			// TODO: make this actually perform a parent change (like how HierarchyObject does)?
			set
			{
				if (parent != null)
					parent.SetTarget(value);
				else
					parent = new WeakReference<ImaginaryHierarchyObject>(value);
			}
		}
		/// <summary>
		/// The weak reference used by the Parent property. Not serialized.
		/// </summary>
		private WeakReference<ImaginaryHierarchyObject> parent = new WeakReference<ImaginaryHierarchyObject>(null);

		/// <summary>
		/// Creates an HierarchyObject from this ImaginaryHierarchyObject.
		/// </summary>
		/// <param name="parent">The parent to use for this HierarchyObject.</param>
		/// <returns>The instanciated HierarchyObject.</returns>
		public HierarchyObject CreateInstance(HierarchyObject parent)
		{
			HierarchyObject instance;

			if (UsesConstructorParameters())
			{
				object[] constructionParmaters = new object[ImaginaryConstructionParameters.Length];
				for (int i = 0; i < ImaginaryConstructionParameters.Length; i++)
				{
					constructionParmaters[i] = ImaginaryConstructionParameters[i].CreateInstance();
				}

				instance = (HierarchyObject)Activator.CreateInstance(GetConstructionType(), args: constructionParmaters);
			}
			else
			{
				instance = (HierarchyObject)EditableSystem.Create(GetConstructionType(), EditorData);
			}

			if (parent != null)
			{
				instance.SetUp(parent);
			}

			foreach (string imaginaryHierarchyName in LocalHierarchy.Keys)
			{
				instance.LocalHierarchy.Add(imaginaryHierarchyName, LocalHierarchy[imaginaryHierarchyName].CreateInstance(instance));
			}

			foreach (ImaginaryScript imaginaryScript in AttatchedScripts.Values)
			{
				instance.AddScriptManually(imaginaryScript.CreateInstance(instance));
			}

			return instance;
		}

		[DataMember]
		public string ConstructionTypeName { get; private set; }

		// TODO: determine if a cache for this is neccessary.
		public Type GetConstructionType()
		{
			return Type.GetType(ConstructionTypeName, true);
		}
	}
}
