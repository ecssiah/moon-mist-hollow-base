using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class ManagerSystem : MonoBehaviour
    {
        private static ManagerSystem _instance;
        public static ManagerSystem Instance { get { return _instance; } }

        private static GameSettings gameSettings;
        public static GameSettings Settings { get { return gameSettings; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            gameSettings = Resources.Load<GameSettings>("SOInstances/GameSettings");
        }
    }
}
