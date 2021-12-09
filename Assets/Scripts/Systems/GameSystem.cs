using UnityEngine;

namespace MMH
{
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
						GameObject newGameObject = new GameObject();

						_instance = newGameObject.AddComponent<T>();
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
}
