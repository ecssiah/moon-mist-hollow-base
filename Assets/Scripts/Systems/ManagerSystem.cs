using UnityEngine;

namespace MMH
{
    public class ManagerSystem : GameSystem<ManagerSystem>
    {
        private static GameSettings _gameSettings;
        public static GameSettings Settings { get => _gameSettings; }

        protected override void Awake()
        {
            base.Awake();

            _gameSettings = Resources.Load<GameSettings>("SOInstances/GameSettings");
        }
    }
}
