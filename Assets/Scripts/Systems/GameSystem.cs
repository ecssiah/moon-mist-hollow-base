using UnityEngine;

public class GameSystem<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
	{
        get
		{
            if (_instance == null)
			{
                _instance = FindObjectOfType<T>();

                if (_instance == null)
				{
					GameObject systemGameObject = new GameObject();

                    _instance = systemGameObject.AddComponent<T>();
				}
			}

            return _instance;
		}
	}

    protected virtual void Awake()
	{
		_instance = this as T;
	}
}
