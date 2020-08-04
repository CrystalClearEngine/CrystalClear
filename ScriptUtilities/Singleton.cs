// TODO: keep in mind: https://csharpindepth.com/articles/singleton

namespace CrystalClear.ScriptUtilities
{
	public abstract class Singleton<T> where T : class, new()
	{
		private static T _instance;

		public static T Instance => _instance ?? (_instance = new T());
	}
}