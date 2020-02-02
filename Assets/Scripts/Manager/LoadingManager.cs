using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manage the loading screen
/// </summary>
public class LoadingManager : Singleton<LoadingManager>
{
	[SerializeField]
	private string LoadSceneName = "Load";

	[SerializeField]
	private float timeForBar = 0.25f;

	/// <summary>
	/// CanvasGroup who appart in the loading screen
	/// </summary>
	[SerializeField]
	private CanvasGroup canvasGroup;

	private Slider slider;

	private bool isLoading = false;

	private AsyncOperation asyncOperation;

	private int nextSceneNumber = -1;
	private string nextSceneName = null;

	protected override void OnAwake()
	{
		enabled = false;

		slider = GetComponentInChildren<Slider>();

		if (canvasGroup)
		{
			canvasGroup.alpha = 0;
		}
	}

	public void ReloadActualScene()
	{
		if (isLoading)
			return;

		if (nextSceneNumber == -1 && nextSceneName == null)
		{
			return;
		}

		StartCoroutine(LoadingOperation());
	}

	/// <summary>
	/// Load Scene with loading screen
	/// </summary>
	public void Load(int sceneNumber)
	{
		if(isLoading)
			return;

		nextSceneNumber = sceneNumber;
		nextSceneName = null;
		StartCoroutine(LoadingOperation());
	}

	/// <summary>
	/// Load Scene with loading screen
	/// </summary>
	public void Load(string sceneName)
	{
		if(isLoading)
			return;

		nextSceneNumber = -1;
		nextSceneName = sceneName;
		StartCoroutine(LoadingOperation());
	}

	private IEnumerator LoadingOperation()
	{
		//preparation
		isLoading = true;
		float pourcent;
		float timer;

		if (canvasGroup)
		{
			timer = 0;

			//fais apparaitre l'UI du Load
			while (isLoading)
			{
				timer += Time.deltaTime;
				pourcent = timer / timeForBar;

				//animations
				canvasGroup.alpha = pourcent;

				if (pourcent >= 1)
					break;

				yield return null;
			}
		}

		//chargement de la scene de transition
		asyncOperation = SceneManager.LoadSceneAsync(LoadSceneName);

		//attend le chargement de la scene de transition
		while(isLoading)
		{
			if(asyncOperation.isDone)
				break;

			yield return null;
		}

		//chargement de la vrai scene
		if(nextSceneNumber != -1)
		{
			asyncOperation = SceneManager.LoadSceneAsync(nextSceneNumber);
		}
		else if(nextSceneName != null)
		{
			asyncOperation = SceneManager.LoadSceneAsync(nextSceneName);
		}
		else
		{
			StopAllCoroutines();
			Debug.LogError("No scene name or number valid");
		}

		while(isLoading)
		{
			if(slider)
			{
				slider.value = asyncOperation.progress;
			}

			yield return null;

			if(asyncOperation.isDone)
				break;
		}

		if(canvasGroup)
		{
			timer = timeForBar;

			//fais disparaitre l'UI du Load
			while (isLoading)
			{
				timer -= Time.deltaTime;
				pourcent = timer / timeForBar;

				//animations
				canvasGroup.alpha = pourcent;

				if(timer <= 0)
					break;

				yield return null;
			}
		}

		isLoading = false;
	}
}
