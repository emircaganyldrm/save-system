using UnityEngine;

namespace SaveSystem
{
	public class SaveController : MonoBehaviour
	{
		[SerializeField] private SavedData saveDefaults;

		private void Awake()
		{
			SaveManager.Init(saveDefaults);
		}

		private void OnDisable()
		{
			SaveManager.SaveLocal();
		}

		//this may not work properly do not rely on it
		private void OnApplicationQuit()
		{
			SaveManager.SaveLocal();
		}
	}
}