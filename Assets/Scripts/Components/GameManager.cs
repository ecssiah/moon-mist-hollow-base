using System;
using UnityEngine;

namespace MMH
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        public static event EventHandler<OnTickArgs> OnTick;

        public SimulationSettings SimulationSettings { get; set; }

		public TimeSystem TimeSystem { get; set; }
        public MapSystem MapSystem { get; set; }
        public EntitySystem EntitySystem { get; set; }

        private int _tick;
        private float _tickTimer;

        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            SimulationSettings = Resources.Load<SimulationSettings>("SOInstances/Simulation Settings");

            _tick = 0;
            _tickTimer = 0f;

            TimeSystem = new TimeSystem();
            MapSystem = new MapSystem();
            EntitySystem = new EntitySystem();
        }

		void Start()
		{
            TimeSystem.Init();
            MapSystem.Init();
            EntitySystem.Init();
		}

		void Update()
        {
            _tickTimer += Time.deltaTime;

            if (_tickTimer >= SimulationSettings.TickDuration)
            {
                _tickTimer -= SimulationSettings.TickDuration;
                _tick++;

                OnTick?.Invoke(this, new OnTickArgs { Tick = _tick });
            }
        }

        void OnDisable()
		{
            TimeSystem.Quit();
            MapSystem.Quit();
            EntitySystem.Quit();
        }
	}
}
