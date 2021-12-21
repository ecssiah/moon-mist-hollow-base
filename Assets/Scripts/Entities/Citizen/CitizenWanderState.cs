namespace MMH
{
	public class CitizenWanderState : CitizenMovementState
	{
		public CitizenWanderState(Citizen citizen) : base(citizen) { }

		public override void Tick()
		{
			if (_citizen.CanAct())
			{
				Direction newDirection = Utils.RandomEnumValue<Direction>();

				_citizen.Direction = newDirection;

				if (SimulationManager.Instance.MapSystem.IsPassable(_citizen.Position, newDirection))
				{
					_citizen.Cooldown = MapInfo.DirectionCosts[newDirection];
					_citizen.Position += MapInfo.DirectionVectors[newDirection];

					_citizen.UpdateRenderPosition();
				}
				else
				{
					_citizen.Cooldown = 4;

					_citizen.UpdateRenderDirection();
				}
			}
		}
	}
}
