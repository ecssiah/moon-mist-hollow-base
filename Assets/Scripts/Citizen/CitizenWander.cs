using System;

namespace MMH
{
	public class CitizenWander : CitizenMovementState
	{
		private Citizen citizen;

		public CitizenWander(Citizen citizen)
		{
			this.citizen = citizen;

			citizenMovementStateType = CitizenMovementStateType.Wander;
		}

		public override void Tick()
		{
			if (citizen.CanAct())
			{
				Direction newDirection = Utils.RandomEnumValue<Direction>();

				if (MapSystem.Instance.IsPassable(citizen.Position, newDirection))
				{
					citizen.SetCooldown(MapSystem.DirectionCosts[newDirection]);

					citizen.SetDirection(newDirection);
					citizen.SetPosition(citizen.Position + MapSystem.DirectionVectors[newDirection]);
				}
				else
				{
					citizen.SetCooldown(4);

					citizen.SetDirection(newDirection);
				}
			}
		}
	}
}
