using System;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public partial class CitizenWander : CitizenState
	{
		public static event EventHandler<OnUpdateCitizenPositionEventArgs> OnUpdateCitizenPosition;

		private Citizen citizen;

		public CitizenWander(Citizen citizen)
		{
			this.citizen = citizen;
		}

		public override void Tick()
		{
			if (citizen.Cooldown <= 0)
			{
				Direction direction = Utils.RandomEnumValue<Direction>();

				citizen.Direction = direction;

				int2 testPosition = citizen.Position + MapSystem.DirectionVectors[direction];

				if (!MapSystem.Instance.IsSolid(testPosition) && MapSystem.Instance.OnMap(testPosition))
				{
					OnUpdateCitizenPositionEventArgs eventArgs = new OnUpdateCitizenPositionEventArgs
					{
						Ticks = MapSystem.DirectionCosts[direction],
						PreviousPosition = citizen.Position,
						Citizen = citizen
					};

					citizen.Position = testPosition;
					citizen.Cooldown = eventArgs.Ticks;

					OnUpdateCitizenPosition?.Invoke(this, eventArgs);
				}
			}
		}
	}
}
