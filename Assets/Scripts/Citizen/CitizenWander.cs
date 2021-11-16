using System;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public partial class CitizenWander : CitizenState
	{
		public static event EventHandler<OnUpdateCitizenPositionEventArgs> OnUpdateCitizenPosition;

		private Citizen citizen;

		private int tickCounter;

		public CitizenWander(Citizen citizen)
		{
			this.citizen = citizen;

			tickCounter = 0;
		}

		public override void Tick()
		{
			tickCounter++;

			int tickLimit = (int)math.clamp(20 - citizen.Attributes.Speed, 1, 20);

			if (tickCounter > tickLimit)
			{
				tickCounter = 0;

				Direction direction = Utils.RandomEnumValue<Direction>();

				citizen.Direction = direction;

				int2 testPosition = citizen.Position + MapSystem.DirectionVectors[direction];

				if (!MapSystem.Instance.IsSolid(testPosition) && MapSystem.Instance.OnMap(testPosition))
				{
					OnUpdateCitizenPositionEventArgs eventArgs = new OnUpdateCitizenPositionEventArgs
					{
						PreviousPosition = citizen.Position,
						Citizen = citizen
					};

					citizen.Position = testPosition;

					OnUpdateCitizenPosition?.Invoke(this, eventArgs);
				}
			}
		}
	}
}
