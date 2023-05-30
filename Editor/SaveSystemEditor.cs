using System.IO;
using SaveSystem.Cryptology;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SaveSystem.Editor
{
	public class SaveSystemEditor : EditorWindow
	{
		[SerializeField] private VisualTreeAsset treeAsset;

		private string saveJson;
		private string[] lines;
		private bool foundSave;

		private void OnEnable()
		{
			RefreshData();
		}

		private void CreateGUI()
		{
			treeAsset.CloneTree(rootVisualElement);

			VisualElement labelsVisualElement = rootVisualElement.Q<VisualElement>("labels");
			
			if (foundSave)
				foreach (string line in lines)
				{
					Label label = new(line)
					{
						style =
						{
							borderBottomWidth = 2,
							borderBottomColor = new Color(.22f, .22f, .22f)
						}
					};

					labelsVisualElement.Add(label);
				}
			else
				labelsVisualElement.Add(new Label("No Save Data Found"));
		}

		[MenuItem("Tools/Save System/Show Save Data")]
		private static void ShowWindow()
		{
			SaveSystemEditor window = GetWindow<SaveSystemEditor>();
			window.maxSize = new Vector2(500, 500);
			window.minSize = new Vector2(500, 500);
			window.titleContent = new GUIContent("Save Data");
			window.Show();
		}

		[MenuItem("Tools/Save System/Clear Saves")]
		public static void DeleteSaves()
		{
			if(EditorUtility.DisplayDialog("Warning","Are you sure you want to delete all playerprefs and saves?", "Yes", "No"))
			{
				PlayerPrefs.DeleteAll();
				File.Delete(SaveManager.savePath);
			}
		}

		private void RefreshData()
		{
			if (File.Exists(SaveManager.savePath))
			{
				saveJson = File.ReadAllText(SaveManager.savePath);
				saveJson = saveJson.FromEncrypted();

				saveJson = saveJson.Replace("{", "");
				saveJson = saveJson.Replace("}", "");
				saveJson = saveJson.Replace("\"", "");

				lines = saveJson.Split(',');
				foundSave = true;
			}
		}
	}
}