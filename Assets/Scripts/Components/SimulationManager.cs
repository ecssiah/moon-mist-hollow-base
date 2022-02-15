using System;
using UnityEngine;

namespace MMH
{
	public class SimulationManager : MonoBehaviour
	{
		public static SimulationManager Instance { get; private set; }

		public static event EventHandler<OnTickArgs> OnTick;

		public TimeSystem TimeSystem { get; private set; }
		public MapSystem MapSystem { get; private set; }
		public EntitySystem EntitySystem { get; private set; }

		private int _tick;
		private float _tickTimer;

		void Awake()
		{
			EnforceSingleInstance();

			_tick = 0;
			_tickTimer = 0f;

			TimeSystem = new TimeSystem();
			MapSystem = new MapSystem();
			EntitySystem = new EntitySystem();
		}

		private void EnforceSingleInstance()
		{
			if (Instance != null && Instance != this)
			{
				Destroy(gameObject);
			}
			else
			{
				Instance = this;
			}
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

			if (_tickTimer >= SimulationInfo.TickDuration)
			{
				_tick++;

				_tickTimer -= SimulationInfo.TickDuration;

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
