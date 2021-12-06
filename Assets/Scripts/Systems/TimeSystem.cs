using System;
using UnityEngine;

namespace MMH
{
	public class TimeSystem : GameSystem<TimeSystem>
	{
		public static event EventHandler<OnTickArgs> OnTick;

		public const float TICK_DURATION = 0.2f;

		private int _tick;
		private float _tickTimer;

		public class OnTickArgs : EventArgs
		{
			public int Tick;
		}

		protected override void Awake()
		{
			base.Awake();

			_tick = 0;
		}

		private void Update()
		{
			_tickTimer += Time.deltaTime;

			if (_tickTimer >= TICK_DURATION)
			{
				_tickTimer -= TICK_DURATION;
				_tick++;

				OnTick?.Invoke(this, new OnTickArgs { Tick = _tick });
			}
		}
	}
}
