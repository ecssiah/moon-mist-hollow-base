namespace MMH
{
	public abstract class CitizenMovementState
	{
		protected Citizen _citizen;

		protected CitizenMovementStateType _citizenMovementStateType;
		public CitizenMovementStateType CitizenMovementStateType => _citizenMovementStateType;

		public abstract void Tick();
	}
}
