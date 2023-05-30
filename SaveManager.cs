using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SaveSystem
{
	public static class SaveManager
	{
		public static readonly string savePath = Application.persistentDataPath + "/data.sav";

		public static ref SavedData SavedData => ref savedData;
		private static SavedData savedData;

		public static event Action OnSave;

		/// <summary>
		/// Initializes the save manager, if there is a save file it will load it, if not it will create a new one with the given default data
		/// </summary>
		public static void Init(SavedData defaultSavedData)
		{
			if (File.Exists(savePath))
				Load();
			else
				savedData = defaultSavedData;
		}

		public static void SaveLocal()
		{
			OnSave?.Invoke();
			Saver.Save(savedData, savePath);
		}

		private static SavedData Load()
		{
			savedData = Saver.Load<SavedData>(savePath);
			return savedData;
		}
	}
}