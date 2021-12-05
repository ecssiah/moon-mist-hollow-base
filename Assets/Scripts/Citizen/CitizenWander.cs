using System;

namespace MMH
{
	public partial class CitizenWander : CitizenState
	{
		public static event EventHandler<OnUpdateCitizenDirectionArgs> OnUpdateCitizenDirection;
		public static event EventHandler<OnUpdateCitizenPositionArgs> OnUpdateCitizenPosition;

		private Citizen citizen;

		public CitizenWander(Citizen citizen)
		{
			this.citizen = citizen;

			citizenStateType = CitizenStateType.CitizenWander;
		}

		public override void Tick()
		{
			if (citizen.Cooldown <= 0)
			{
				citizen.Direction = Utils.RandomEnumValue<Direction>();

				if (MapSystem.Instance.IsPassable(citizen.Position, citizen.Direction))
				{
					OnUpdateCitizenPositionArgs eventArgs = new OnUpdateCitizenPositionArgs
					{
						Ticks = MapSystem.DirectionCosts[citizen.Direction],
						PreviousPosition = citizen.Position,
						Citizen = citizen
					};

					citizen.Position += MapSystem.DirectionVectors[citizen.Direction];
					citizen.SetCooldown(eventArgs.Ticks);

					OnUpdateCitizenPosition?.Invoke(this, eventArgs);
				}
				else
				{
					OnUpdateCitizenDirectionArgs eventArgs = new OnUpdateCitizenDirectionArgs
					{
						Citizen = citizen
					};

					citizen.SetCooldown(4);

					OnUpdateCitizenDirection?.Invoke(this, eventArgs);
				}
			}
		}
	}
}
