using System;
using UnityEngine;

namespace MMH
{
	public class TimeSystem : MonoBehaviour
	{
		private static TimeSystem _instance;
		public static TimeSystem Instance { get { return _instance; } }

		public static event EventHandler<OnTickArgs> OnTick;

		public const float TICK_DURATION = 0.2f;

		private int tick;
		private float tickTimer;

		public class OnTickArgs : EventArgs
		{
			public int tick;
		}

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

			tick = 0;
		}

		private void Update()
		{
			tickTimer += Time.deltaTime;

			if (tickTimer >= TICK_DURATION)
			{
				tickTimer -= TICK_DURATION;
				tick++;

				OnTick?.Invoke(this, new OnTickArgs { tick = tick });
			}
		}
	}
}
