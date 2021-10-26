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

		public class OnTurnEventArgs : EventArgs
		{
			public int turn;
		}

		public static event EventHandler<OnTickEventArgs> OnTick;
		public static event EventHandler<OnTurnEventArgs> OnTurn;

		private const float TICK_DURATION = 0.2f;
		private const float TICKS_PER_TURN = 20;

		private int tick;
		private float tickTimer;

		private int turn;

		private void Awake()
		{
			tick = 0;
			turn = 0;
		}

		private void Update()
		{
			tickTimer += Time.deltaTime;

			if (tickTimer >= TICK_DURATION)
			{
				tickTimer -= TICK_DURATION;
				tick++;

				OnTick?.Invoke(this, new OnTickEventArgs { tick = tick });

				if (tick >= TICKS_PER_TURN)
				{
					tick = 0;
					turn++;

					OnTurn?.Invoke(this, new OnTurnEventArgs { turn = turn });
				}
			}
		}
	}
}
