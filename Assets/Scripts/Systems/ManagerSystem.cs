using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class ManagerSystem : GameSystem<ManagerSystem>
    {
        private static GameSettings gameSettings;
        public static GameSettings Settings { get { return gameSettings; } }

        protected override void Awake()
        {
            base.Awake();

            gameSettings = Resources.Load<GameSettings>("SOInstances/GameSettings");
        }
    }
}
