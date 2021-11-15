using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class CitizenWander : CitizenState
	{
		private CitizenSystem citizenSystem;
		private MapSystem mapSystem;

		private int tickCounter;

		public CitizenWander(CitizenSystem citizenSystem, MapSystem mapSystem)
		{
			this.citizenSystem = citizenSystem;
			this.mapSystem = mapSystem;

			tickCounter = 0;
		}

		public override void Tick(Citizen citizen)
		{
			tickCounter++;

			int tickLimit = (int)math.clamp(20 - citizen.Attributes.Speed, 1, 20);

			if (tickCounter > tickLimit)
			{
				tickCounter = 0;

				Direction direction = Utils.RandomEnumValue<Direction>();

				citizen.Direction = direction;

				int2 testPosition = citizen.Position + MapSystem.DirectionVectors[direction];

				if (!mapSystem.IsSolid(testPosition) && mapSystem.OnMap(testPosition))
				{
					citizen.Position = testPosition;

					citizenSystem.UpdateCitizen(citizen);
				}
			}
		}
	}
}
