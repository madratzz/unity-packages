using UnityEngine;

namespace CustomUtilities
{
	public abstract class SingletonInternal<T> : MonoBehaviour where T : Component
	{
		/// <summary>
		///     The instance.
		/// </summary>
		protected static T instance;

		protected virtual void OnValidate()
		{
			gameObject.name = typeof(T).Name;
		}
	}

	public abstract class Singleton<T> : SingletonInternal<T> where T : Component
	{
		public static T Instance
		{
			get
			{
				if (instance == null)
				{
#if UNITY_2023_1_OR_NEWER
					instance = FindFirstObjectByType<T>();
#else
					instance = FindObjectOfType<T>();
#endif
					if (instance == null) instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
				}

				return instance;
			}
		}

		protected virtual void Awake()
		{
			if (instance == null)
				instance = this as T;
		}
	}

	public abstract class SingletonPersistent<T> : Singleton<T> where T : Component
	{
		protected override void Awake()
		{
			if (instance == null)
			{
				instance = this as T;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				Destroy(gameObject);
			}
		}
	}
}