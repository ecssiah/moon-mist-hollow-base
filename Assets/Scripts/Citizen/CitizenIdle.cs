namespace MMH
{
	public class CitizenIdle : CitizenMovementState
	{
		private Citizen citizen;

		public CitizenIdle(Citizen citizen)
		{
			this.citizen = citizen;

			citizenMovementStateType = CitizenMovementStateType.Idle;
		}

		public override void Tick()
		{
			if (citizen.CanAct())
			{
				Direction newDirection = Utils.RandomEnumValue<Direction>();

				citizen.SetCooldown(8);

				citizen.SetDirection(newDirection);
			}
		}
	}
}
