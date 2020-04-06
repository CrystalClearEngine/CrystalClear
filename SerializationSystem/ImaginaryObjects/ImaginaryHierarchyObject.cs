using CrystalClear.HierarchySystem;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// ImaginaryHierarchyObject is an derivative of ImaginaryObject that is designed specifically for storing HierarchyObjects and the extra data that goes along with it.
	/// </summary>
	[Serializable]
	[DataContract]
	public class ImaginaryHierarchyObject
		: ImaginaryObject
	{
		/// <summary>
		/// Creates an ImaginaryHierarchyObject using the provided parent, construction type and construction parameters.
		/// </summary>
		/// <param name="parent">Defines the ImaginaryHierarchyObject's position in the Hierarchy.</param>
		/// <param name="constructionType">The HierarchyObject type.</param>
		/// <param name="constructorParameters">The construction parameters.</param>
		public ImaginaryHierarchyObject(ImaginaryHierarchyObject parent, Type constructionType, ImaginaryObject[] constructorParameters) : base(constructionType, constructorParameters)
		{
			Parent = parent;
		}

		/// <summary>
		/// Contains the children of this ImaginaryHierarchyObject.
		/// </summary>
		[DataMember]
		public Dictionary<string, ImaginaryHierarchyObject> LocalHierarchy = new Dictionary<string, ImaginaryHierarchyObject>();
		
		/// <summary>
		/// Contains the scripts which will be added to the HierarchyObject when an instance is created.
		/// </summary>
		[DataMember]
		public Dictionary<string, ImaginaryScript> AttatchedScripts = new Dictionary<string, ImaginaryScript>();

		/// <summary>
		/// A weak reference to the parent of this ImaginaryHierarchyObject. Not serialized.
		/// </summary>
		public ImaginaryHierarchyObject Parent
		{
			get
			{
				parent.TryGetTarget(out ImaginaryHierarchyObject editorHierarchyObject);
				return editorHierarchyObject;
			}
			set
			{
				parent.SetTarget(value);
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
			object[] constructionParmaters = new object[ConstructionParameters.Length];
			for (int i = 0; i < ConstructionParameters.Length; i++)
			{
				constructionParmaters[i] = ConstructionParameters[i].CreateInstance();
			}

			HierarchyObject instance = (HierarchyObject)Activator.CreateInstance(GetConstructionType(), args: constructionParmaters);

			if (parent != null)
			{
				instance.SetUp(parent);
			}

			foreach (string imaginaryHierarchyName in LocalHierarchy.Keys)
			{
				instance.LocalHierarchy.Add(imaginaryHierarchyName, LocalHierarchy[imaginaryHierarchyName].CreateInstance(instance));
			}

			foreach (ImaginaryScript editorScript in AttatchedScripts.Values)
			{
				instance.AddScriptManually(editorScript.CreateInstance(instance));
			}

			return instance;
		}
	}
}
