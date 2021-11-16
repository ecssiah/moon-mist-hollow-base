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

				if (MapSystem.Instance.IsPassable(citizen.Position, direction))
				{
					OnUpdateCitizenPositionEventArgs eventArgs = new OnUpdateCitizenPositionEventArgs
					{
						Ticks = MapSystem.DirectionCosts[direction],
						PreviousPosition = citizen.Position,
						Citizen = citizen
					};

					citizen.Position += MapSystem.DirectionVectors[direction];
					citizen.Cooldown = eventArgs.Ticks;

					OnUpdateCitizenPosition?.Invoke(this, eventArgs);
				}
			}
		}
	}
}
