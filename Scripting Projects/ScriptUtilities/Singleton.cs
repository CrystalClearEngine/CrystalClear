namespace CrystalClear.EventSystem
{
	public abstract class Singleton<T> where T : class, new()
	{
		static Singleton()
		{
		}

		protected Singleton()
		{
		}

		private static T _instance;

		public static T Instance => _instance ?? (_instance = new T());
	}
}
