using UnityEngine;

namespace MMH
{
	public class CitizenWander : CitizenState
	{
		private MapSystem mapSystem;

		private int tickCounter;

		public CitizenWander(MapSystem mapSystem)
		{
			this.mapSystem = mapSystem;

			tickCounter = 0;
		}

		public override void Tick(Citizen citizen)
		{
			tickCounter++;

			if (tickCounter > 20)
			{
				tickCounter = 0;

				Direction direction = Utils.RandomEnumValue<Direction>();

				citizen.Direction = direction;

				Debug.Log($"{citizen.Id} {citizen.Direction}");
			}
		}
	}
}
