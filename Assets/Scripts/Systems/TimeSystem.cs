using UnityEngine;

namespace MMH
{
	public class TimeSystem : GameSystem
	{
		public override void Init()
		{
			SetupEvents();
		}

		private void SetupEvents()
		{
			GameManager.OnTick += Tick;
		}

		public override void Tick(object sender, OnTickArgs eventArgs)
		{

		}

		public override void Quit()
		{
			GameManager.OnTick -= Tick;
		}
	}
}
