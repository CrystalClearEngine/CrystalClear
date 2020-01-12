namespace CrystalClear.SerializationSystem
{
	public interface IExtraObjectData
	{
		// TODO consider updating naming. Get *what* data?
		ExtraDataObject GetData();
		void SetData(ExtraDataObject data);
	}
}
