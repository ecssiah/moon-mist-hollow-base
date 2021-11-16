using System;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public partial class CitizenWander : CitizenState
	{
		public static event EventHandler<OnUpdateCitizenDirectionEventArgs> OnUpdateCitizenDirection;
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
				citizen.Direction = Utils.RandomEnumValue<Direction>();

				if (MapSystem.Instance.IsPassable(citizen.Position, citizen.Direction))
				{
					OnUpdateCitizenPositionEventArgs eventArgs = new OnUpdateCitizenPositionEventArgs
					{
						Ticks = MapSystem.DirectionCosts[citizen.Direction],
						PreviousPosition = citizen.Position,
						Citizen = citizen
					};

					citizen.Position += MapSystem.DirectionVectors[citizen.Direction];
					citizen.Cooldown = eventArgs.Ticks;

					OnUpdateCitizenPosition?.Invoke(this, eventArgs);
				}
				else
				{
					citizen.Cooldown = 4;

					OnUpdateCitizenDirectionEventArgs eventArgs = new OnUpdateCitizenDirectionEventArgs
					{
						Citizen = citizen
					};

					OnUpdateCitizenDirection?.Invoke(this, eventArgs);
				}
			}
		}
	}
}
