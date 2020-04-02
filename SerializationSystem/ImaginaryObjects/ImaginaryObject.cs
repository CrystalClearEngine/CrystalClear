using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem
{
	/// <summary>
	/// An ImaginaryObject is an object that stores the construction data for the object so that they can be created in the editor, then editen, serialized and finally deserialized and an instance can be created.
	/// </summary>
	[Serializable]
	[DataContract]
	public class ImaginaryObject
	{
		/// <summary>
		/// Creates an ImaginaryObject with the specified type and constructor parameters.
		/// </summary>
		/// <param name="constructionType">The type of object to represent.</param>
		/// <param name="constructorParameters">The constructor parameters to use initially.</param>
		public ImaginaryObject(Type constructionType, ImaginaryObject[] constructorParameters)
		{
			ConstructionTypeName = constructionType.AssemblyQualifiedName;
			ConstructionParameters = constructorParameters ?? Array.Empty<ImaginaryObject>();
		}

		protected ImaginaryObject()
		{

		}

		/// <summary>
		/// The AssemblyQualifiedType name of the object's type.
		/// </summary>
		[DataMember]
		public string ConstructionTypeName { get; set; }

		/// <summary>
		/// The parameters to be used when constructing the object.
		/// </summary>
		[DataMember]
		public ImaginaryObject[] ConstructionParameters;

		/// <summary>
		/// Returns the type that ConstructionTypeName references.
		/// </summary>
		/// <returns>The type that ConstructionTypeName references.</returns>
		public Type GetConstructionType()
		{
			return Type.GetType(ConstructionTypeName, true);
		}

		public virtual object CreateInstance()
		{
			List<object> list = new List<object>();

			foreach (ImaginaryObject imaginaryObject in ConstructionParameters)
			{
				list.Add(imaginaryObject.CreateInstance());
			}

			return Activator.CreateInstance(GetConstructionType(), list.ToArray());
		}

		/// <summary>
		/// Writes the construction info of the object using the provided BinaryWriter and encoding.
		/// </summary>
		/// <param name="writer">The BinaryWriter to use to write the data to.</param>
		/// <param name="encoding">The encoding to use for writing data.</param>
		internal void WriteConstructionInfo(BinaryWriter writer, Encoding encoding)
		{
			// Write the construction type name so that it can be retrieved in the deserialization process.
			writer.Write(ConstructionTypeName);
			// Write the number of ConstructionParameters so that the deserializer knows how many ConstructorParameters it has to read.
			writer.Write(ConstructionParameters.Length);
			// Iterate through all constructor parameters.
			foreach (ImaginaryObject parameter in ConstructionParameters)
			{
				// Write the parameter's construction type name so that it can be retrieved when deserializing.
				writer.Write(parameter.ConstructionTypeName);

				// Does the parameter qualify as an ImaginaryPrimitive?
				if (ImaginaryPrimitive.QualifiesAsPrimitive(parameter))
				{
					writer.Write((parameter as ImaginaryPrimitive).StringValue);
				}
				// Otherwise recursively write the parameters construction info.
				else
				{
					parameter.WriteConstructionInfo(writer, encoding);
				}
			}
		}
	}
}
