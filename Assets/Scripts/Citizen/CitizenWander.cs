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
						Citizen = citizen,
						StartPosition = citizen.Position,
					};

					citizen.Position += MapSystem.DirectionVectors[citizen.Direction];

					citizen.Cooldown = MapSystem.DirectionCosts[citizen.Direction];

					OnUpdateCitizenPosition?.Invoke(this, eventArgs);
				}
				else
				{
					OnUpdateCitizenDirectionArgs eventArgs = new OnUpdateCitizenDirectionArgs
					{
						Citizen = citizen
					};

					citizen.Cooldown = 4;

					OnUpdateCitizenDirection?.Invoke(this, eventArgs);
				}
			}
		}
	}
}
