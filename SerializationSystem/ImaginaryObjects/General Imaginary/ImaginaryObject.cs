using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	/// <summary>
	/// An ImaginaryObject is an object that stores the construction or editor data for the object so that it can be created in the editor, serialized and finally deserialized and an instance can be created.
	/// </summary>
	[Serializable]
	[DataContract]
	public class ImaginaryObject : ImaginaryObjectBase<object>
	{
		/// <summary>
		/// Creates an ImaginaryObject with the specified type and constructor parameters.
		/// </summary>
		/// <param name="constructionType">The type of object to represent.</param>
		/// <param name="constructorParameters">The constructor parameters to use initially.</param>
		public ImaginaryObject(Type constructionType, ImaginaryObject[] constructorParameters = null)
		{
			ConstructionTypeName = constructionType.AssemblyQualifiedName;
			ImaginaryConstructionParameters = constructorParameters ?? Array.Empty<ImaginaryObject>();
		}

		public ImaginaryObject(Type constructionType, EditorData editorData)
		{
			if (!constructionType.IsEditable())
			{
				throw new ArgumentException($"ConstructionType is not editable! ConstructionType = {constructionType}");
			}

			ConstructionTypeName = constructionType.AssemblyQualifiedName;
			EditorData = editorData;
		}

		protected ImaginaryObject()
		{

		}

		/// <summary>
		/// The AssemblyQualifiedType name of the object's type. Do not set.
		/// </summary>
		[DataMember]
		public string ConstructionTypeName { get; protected set; }

		/// <summary>
		/// Returns the type that ConstructionTypeName references.
		/// </summary>
		/// <returns>The type that ConstructionTypeName references.</returns>
		public Type GetConstructionType()
		{
			if (ConstructionTypeCache is null)
			{
				ConstructionTypeCache = Type.GetType(ConstructionTypeName, true);
			}
			return ConstructionTypeCache;
		}
		private Type ConstructionTypeCache = null;

		[DataMember]
		public EditorData EditorData = null;

		/// <summary>
		/// The parameters to be used when constructing the object.
		/// </summary>
		[DataMember]
		public ImaginaryObject[] ImaginaryConstructionParameters;

		public bool UsesEditor()
		{
			if (EditorData is null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public bool UsesConstructorParameters()
		{
			return !UsesEditor();
		}

		public override object CreateInstance()
		{
			if (UsesConstructorParameters())
			{
				List<object> constructorObjects = new List<object>();

				foreach (ImaginaryObject imaginaryObject in ImaginaryConstructionParameters)
				{
					constructorObjects.Add(imaginaryObject.CreateInstance());
				}

				return Activator.CreateInstance(GetConstructionType(), constructorObjects.ToArray());
			}
			else
			{
				return EditableSystem.Create(GetConstructionType(), EditorData);
			}
		}

		/// <summary>
		/// Writes the construction info of the object using the provided BinaryWriter and encoding.
		/// </summary>
		/// <param name="writer">The BinaryWriter to use to write the data to.</param>
		/// <param name="encoding">The encoding to use for writing data.</param>
		internal override void WriteConstructionInfo(BinaryWriter writer, Encoding encoding)
		{
			lock (this)
			{
				// Write the construction type name so that it can be retrieved in the deserialization process.
				writer.Write(ConstructionTypeName);

				writer.Write(UsesConstructorParameters());

				if (UsesConstructorParameters())
				{
					// Write the number of ConstructionParameters so that the deserializer knows how many ConstructorParameters it has to read.
					writer.Write(ImaginaryConstructionParameters.Length);
					// Iterate through all constructor parameters.
					foreach (ImaginaryObject parameter in ImaginaryConstructionParameters)
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
				else
				{
					writer.Write(EditorData.Count);

					foreach (KeyValuePair<string, string> editorData in EditorData)
					{
						writer.Write(editorData.Key);
						writer.Write(editorData.Value);
					}
				}
			}
		}
	}
}
