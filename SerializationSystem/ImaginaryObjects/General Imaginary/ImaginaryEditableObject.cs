using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CrystalClear.SerializationSystem.ImaginaryObjects
{
	[DataContract]
	public sealed class ImaginaryEditableObject : ImaginaryGeneralObject
	{
		public ImaginaryEditableObject(Type constructionType, EditorData editorData)
		{
			if (!constructionType.IsEditable())
			{
				throw new ArgumentException($"ConstructionType is not editable! ConstructionType = {constructionType}");
			}

			ConstructionTypeName = constructionType.AssemblyQualifiedName;
			EditorData = editorData;
		}

		public ImaginaryEditableObject()
		{ }

		[DataMember]
		public EditorData EditorData;

		public override object CreateInstance()
		{
			return EditableSystem.Create(GetConstructionType(), EditorData);
		}

		internal override void WriteConstructionInfo(BinaryWriter writer, Encoding encoding)
		{
			writer.Write(ConstructionTypeName);

			writer.Write(EditorData.Count);

			foreach (KeyValuePair<string, string> editorData in EditorData)
			{
				writer.Write(editorData.Key);
				writer.Write(editorData.Value);
			}
		}

		internal override void ReadConstructionInfo(BinaryReader reader, Encoding encoding)
		{
			ConstructionTypeName = reader.ReadString();

			EditorData = EditorData.GetEmpty();

			for (int i = 0; i < reader.ReadInt32(); i++)
			{
				EditorData.Add(reader.ReadString(), reader.ReadString());
			}
		}
	}
}
