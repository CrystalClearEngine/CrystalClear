﻿using System.Runtime.Serialization;

// TODO: might make redundant by storing name as part of ImaginaryHierarchyObjects?
namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	public sealed class ImaginaryHierarchy : ImaginaryHierarchyObject
	{
		public ImaginaryHierarchy(ImaginaryHierarchyObject imaginaryHierarchyObject, string name)
		{
			ImaginaryObjectBase = imaginaryHierarchyObject.ImaginaryObjectBase;

			AttachedScripts = imaginaryHierarchyObject.AttachedScripts;
			LocalHierarchy = imaginaryHierarchyObject.LocalHierarchy;

			HierarchyName = name;
		}

		private ImaginaryHierarchy()
		{
		}

		[DataMember] public string HierarchyName { get; private set; }

		public ImaginaryHierarchyObject GetHierarchyObject() => this;
	}
}