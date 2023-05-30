using System.IO;
using UnityEngine;
using SaveSystem.Cryptology;

namespace SaveSystem
{
	public static class Saver
	{
		/// <summary>
		/// Takes a class, encrypts it and saves it to a file
		/// </summary>
		/// <param name="data">Class you desired</param>
		/// <param name="savePath">Persistent data path + data.rft may be used</param>
		/// <typeparam name="T">Type you desired</typeparam>
		public static void Save<T>(T data, string savePath)
		{
			var save = JsonUtility.ToJson(data);
			save = save.ToEncrypted();
			
			File.WriteAllText(savePath, save);
		}
		
		
		/// <summary>
		/// Loads the file in specified path, decrypts it and returns it as a class
		/// </summary>
		/// <param name="savePath">Persistent data path + data.rft may be used</param>
		/// <typeparam name="T">Type you desired</typeparam>
		public static T Load<T>(string savePath)
		{
			var save = File.ReadAllText(savePath);
			save = save.FromEncrypted();
			
			return JsonUtility.FromJson<T>(save);
		}
	}

}