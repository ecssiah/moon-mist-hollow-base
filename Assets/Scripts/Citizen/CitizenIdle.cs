namespace MMH
{
	public class CitizenIdle : CitizenMovementState
	{
		public CitizenIdle(Citizen citizen)
		{
			_citizen = citizen;

			_citizenMovementStateType = CitizenMovementStateType.Idle;
		}

		public override void Tick()
		{
			if (_citizen.CanAct())
			{
				Direction newDirection = Utils.RandomEnumValue<Direction>();

				_citizen.Cooldown = 8;
				_citizen.Direction = newDirection;

				_citizen.UpdateRenderDirection();
			}
		}
	}
}
