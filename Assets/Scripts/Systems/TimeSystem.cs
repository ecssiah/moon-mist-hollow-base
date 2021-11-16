using System;
using UnityEngine;

namespace MMH
{
	public class TimeSystem : MonoBehaviour
	{
		private static TimeSystem _instance;
		public static TimeSystem Instance { get { return _instance; } }

		public class OnTickEventArgs : EventArgs
		{
			public int tick;
		}

		public static event EventHandler<OnTickEventArgs> OnTick;

		public const float TICK_DURATION = 0.2f;

		private int tick;
		private float tickTimer;

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

				OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });
			}
		}
	}
}
