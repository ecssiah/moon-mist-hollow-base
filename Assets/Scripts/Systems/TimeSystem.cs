using UnityEngine;

namespace MMH
{
	public class TimeSystem : SimulationSystem
	{
		public override void Init()
		{
			SetupEvents();
		}

		private void SetupEvents()
		{
			SimulationManager.OnTick += Tick;
		}

		protected override void Tick(object sender, OnTickArgs eventArgs)
		{

		}

		public override void Quit()
		{
			SimulationManager.OnTick -= Tick;
		}
	}
}
