namespace MMH
{
	public class CitizenWander : CitizenMovementState
	{
		public CitizenWander(Citizen citizen)
		{
			_citizen = citizen;
		}

		public override void Tick()
		{
			if (_citizen.CanAct())
			{
				Direction newDirection = Utils.RandomEnumValue<Direction>();

				if (GameManager.Instance.MapSystem.IsPassable(_citizen.Position, newDirection))
				{
					_citizen.Cooldown = MapConstants.DirectionCosts[newDirection];

					_citizen.Direction = newDirection;
					_citizen.Position += MapConstants.DirectionVectors[newDirection];

					_citizen.UpdateRenderPosition();
				}
				else
				{
					_citizen.Cooldown = 4;

					_citizen.Direction = newDirection;

					_citizen.UpdateRenderDirection();
				}
			}
		}
	}
}
