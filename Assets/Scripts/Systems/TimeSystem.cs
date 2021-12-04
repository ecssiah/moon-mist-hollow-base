using System;
using UnityEngine;

namespace MMH
{
	public class TimeSystem : GameSystem<TimeSystem>
	{
		public static event EventHandler<OnTickArgs> OnTick;

		public const float TICK_DURATION = 0.2f;

		private int tick;
		private float tickTimer;

		public class OnTickArgs : EventArgs
		{
			public int tick;
		}

		protected override void Awake()
		{
			base.Awake();

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
