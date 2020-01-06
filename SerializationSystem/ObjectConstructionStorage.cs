using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using K4os.Compression.LZ4.Streams;

namespace CrystalClear.SerializationSystem
{
	[Serializable]
	[DataContract]
	public struct ExtraDataObject : IDictionary<string, object>
	{
		[DataMember]
		private Dictionary<string, object> data;

		public Dictionary<string, object> Data
		{
			get
			{
				if (data == null)
				{
					data = new Dictionary<string, object>();
				}
				return data;
			}

			set
			{
				if (data == null)
				{
					data = new Dictionary<string, object>();
				}
				data = value;
			}
		}

		public object this[string valueName]
		{
			get => Data[valueName];
			set => Data[valueName] = value;
		}

		#region IDictionary Implementation
		public ICollection<string> Keys => ((IDictionary<string, object>)Data).Keys;

		public ICollection<object> Values => ((IDictionary<string, object>)Data).Values;

		public int Count => ((IDictionary<string, object>)Data).Count;

		public bool IsReadOnly => ((IDictionary<string, object>)Data).IsReadOnly;

		public bool ContainsKey(string key)
		{
			return ((IDictionary<string, object>)Data).ContainsKey(key);
		}

		public void Add(string key, object value)
		{
			((IDictionary<string, object>)Data).Add(key, value);
		}

		public bool Remove(string key)
		{
			return ((IDictionary<string, object>)Data).Remove(key);
		}

		public bool TryGetValue(string key, out object value)
		{
			return ((IDictionary<string, object>)Data).TryGetValue(key, out value);
		}

		public void Add(KeyValuePair<string, object> item)
		{
			((IDictionary<string, object>)Data).Add(item);
		}

		public void Clear()
		{
			((IDictionary<string, object>)Data).Clear();
		}

		public bool Contains(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>)Data).Contains(item);
		}

		public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
		{
			((IDictionary<string, object>)Data).CopyTo(array, arrayIndex);
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return ((IDictionary<string, object>)Data).Remove(item);
		}

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return ((IDictionary<string, object>)Data).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IDictionary<string, object>)Data).GetEnumerator();
		}
		#endregion
	}

	/// <summary>
	/// Internal type for use only to store data.
	/// </summary>
	[Serializable]
	[DataContract]
	internal struct ObjectStorage
	{
		[DataMember]
		internal string typeName;
		[DataMember]
		internal object[] constructorParameters;
		[DataMember]
		internal ExtraDataObject extraData;

		public ObjectStorage(Type objectType, object[] constructorParameters, ExtraDataObject extraData)
		{
			this.typeName = objectType.AssemblyQualifiedName;
			this.constructorParameters = constructorParameters;
			this.extraData = extraData;
		}

		public ObjectStorage(Type objectType, object[] constructorParameters, IExtraObjectData extraData)
		{
			this.typeName = objectType.AssemblyQualifiedName;
			this.constructorParameters = constructorParameters;
			this.extraData = extraData.GetData();
		}
	}

	public static class ObjectConstructionStorage
	{
		#region Creators
		public static object CreateFromSaveFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObjectStorage));

				ObjectStorage deserializedStorage = (ObjectStorage)dataContractSerializer.ReadObject(fileStream);

				object obj = Activator.CreateInstance(Type.GetType(deserializedStorage.typeName), deserializedStorage.constructorParameters);

				// Set the data.
				(obj as IExtraObjectData)?.SetData(deserializedStorage.extraData);

				return obj;
			}
		}

		public static object CreateFromStoreFile(string path)
		{
			using (FileStream fileStream = new FileStream(path, FileMode.Open))
			using (LZ4DecoderStream decompressionStream = LZ4Stream.Decode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				ObjectStorage deserializedStorage = (ObjectStorage)binaryFormatter.Deserialize(decompressionStream);

				object obj = Activator.CreateInstance(Type.GetType(deserializedStorage.typeName), deserializedStorage.constructorParameters);

				// Set the data.
				(obj as IExtraObjectData)?.SetData(deserializedStorage.extraData);

				return obj;
			}
		}
		#endregion

		#region SaveAndStores
		public static void SaveToFile(string path, Type type, object[] constructorParameters = null, object obj = null)
		{
			// Gets the extra data interface if it is present on the type.
			IExtraObjectData dataInterface = obj as IExtraObjectData;

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			{
				DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(ObjectStorage));

				dataContractSerializer.WriteObject(fileStream, new ObjectStorage(type, constructorParameters, dataInterface));
			}
		}

		public static void StoreToFile(string path, Type type, object[] constructorParameters = null, object obj = null)
		{
			// Gets the extra data interface if it is present on the type.
			IExtraObjectData dataInterface = obj as IExtraObjectData;

			using (FileStream fileStream = new FileStream(path, FileMode.Create))
			using (LZ4EncoderStream compressionStream = LZ4Stream.Encode(fileStream))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();

				binaryFormatter.Serialize(compressionStream, new ObjectStorage(type, constructorParameters, dataInterface)); 
			}
		}
		#endregion
	}
}
