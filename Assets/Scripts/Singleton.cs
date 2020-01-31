using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	[SerializeField]
	private bool dontDestroyOnLoad = false;

	private static T _Instance;

	/// <summary>
	/// The Singleton instance, if don't exist create a new one
	/// </summary>
	public static T Instance
	{
		get
		{
			// search for an object of type T in the scene
			if (_Instance == null)
			{
				_Instance = FindObjectOfType<T>();

				/*if(_Instance == null)
				{
					GameObject go = new GameObject(typeof(T).Name);
					_Instance = go.AddComponent<T>();
				}

				Debug.LogError("Instance is null");*/
			}

			return _Instance;
		}
	}

	private void Awake()
	{
		if (_Instance == null)
		{
			_Instance = (T)this;

			if (dontDestroyOnLoad)
			{
				DontDestroyOnLoad(gameObject);
			}

			OnAwake();
		}
		else if (_Instance != this)
		{
			Destroy(gameObject);
			DestroyImmediate(this);
		}
	}

	protected virtual void OnAwake()
	{

	}
}
