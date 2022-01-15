using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
	LevelLoader levelLoader = null;

	internal System.Action Callback_OnLevelWasLoaded = null;
	internal System.Action Callback_OnMenuWasLoaded = null;

	void Start()
	{
		SceneManager.sceneLoaded += LevelWasLoaded;
	}

	internal void InitLevelLoader(LevelLoader newLevelLoader)
	{
		levelLoader = newLevelLoader;
	}

	/// <summary>
	/// Load specific level with buildIndex
	/// </summary>
	/// <param name="levelToLoad"></param>
	internal void LoadLevel(int levelToLoad)
	{
		System.GC.Collect();

		// Security
		if (levelLoader == null)
		{
			if (SceneManager.GetActiveScene().buildIndex != 0)
				Debug.LogWarning("Not level loader !");

			SceneManager.LoadScene(levelToLoad);
			return;
		}

		StartCoroutine(LoadAsynchronously(levelToLoad));
	}

	/// <summary>
	/// Load specific level with name
	/// </summary>
	/// <param name="levelToLoad"></param>
	internal void LoadLevel(string levelToLoad)
	{
		int buildIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + levelToLoad + ".unity");
		LoadLevel(buildIndex);
	}

	/// <summary>
	/// Load the scene MainMenu
	/// </summary>
	internal void LoadMenu()
	{
		LoadLevel("MainMenu");
	}

	/// <summary>
	/// Load buildIndex level + 1
	/// </summary>
	internal void LoadNextLevel()
	{
		int activeSceneCount = SceneManager.GetActiveScene().buildIndex;

		if (activeSceneCount != SceneManager.sceneCountInBuildSettings - 1)
			LoadLevel(activeSceneCount + 1);

		else
			LoadMenu(); // ou load les crédits / la scène final ou que sais-je encore
	}

	/// <summary>
	/// Notify when a level was loaded
	/// </summary>
	/// <param name="scene"></param>
	/// <param name="mode"></param>
	void LevelWasLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.buildIndex != 0)
		{
			if (scene.buildIndex == 1)
			{
				Callback_OnMenuWasLoaded?.Invoke();
				return;
			}

			Callback_OnLevelWasLoaded?.Invoke();
		}
	}

	/// <summary>
	/// Load the scene asynchronously for the load bar
	/// </summary>
	/// <param name="sceneIndex"></param>
	/// <returns></returns>
	IEnumerator LoadAsynchronously(int sceneIndex)
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

		levelLoader.gameObject.SetActive(true);

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);

			levelLoader.SetProgress(progress);

			yield return null;
		}
	}

	internal string CurrentSceneName()
	{
		return SceneManager.GetActiveScene().name;
	}

	internal int CurrentSceneIndex()
	{
		return SceneManager.GetActiveScene().buildIndex;
	}
}
