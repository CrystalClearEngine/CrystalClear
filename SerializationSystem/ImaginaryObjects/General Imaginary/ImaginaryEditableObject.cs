using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	public sealed class ImaginaryEditableObject : ImaginaryObject, IGeneralImaginaryObject
	{
		public ImaginaryEditableObject(Type constructionType, EditorData editorData)
		{
			if (!constructionType.IsEditable())
			{
				throw new ArgumentException($"ConstructionType is not editable! ConstructionType = {constructionType}");
			}

			TypeData = new TypeData(constructionType.AssemblyQualifiedName);
			EditorData = editorData;
		}

		public ImaginaryEditableObject()
		{ }

		[DataMember]
		public TypeData TypeData { get; set; }

		[DataMember]
		public EditorData EditorData;

		public override object CreateInstance()
		{
			return EditableSystem.Create(TypeData.GetConstructionType(), EditorData);
		}

		protected override void WriteConstructionInfo(BinaryWriter writer)
		{
			TypeData.WriteConstructionInfo(writer);

			writer.Write(EditorData.Count);

			foreach (KeyValuePair<string, string> editorData in EditorData)
			{
				writer.Write(editorData.Key);
				writer.Write(editorData.Value);
			}
		}

		protected override void ReadConstructionInfo(BinaryReader reader)
		{
			TypeData = new TypeData(reader);

			EditorData = EditorData.GetEmpty();

			for (int i = 0; i < reader.ReadInt32(); i++)
			{
				EditorData.Add(reader.ReadString(), reader.ReadString());
			}
		}
	}
}
