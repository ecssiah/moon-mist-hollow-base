using System;
using UnityEngine;

namespace MMH
{
	public class TimeSystem : MonoBehaviour
	{
		public class OnTickEventArgs : EventArgs
		{
			public int tick;
		}

		public static event EventHandler<OnTickEventArgs> OnTick;

		private const float TICK_DURATION = 0.2f;

		private int tick;
		private float tickTimer;

		private void Awake()
		{
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
