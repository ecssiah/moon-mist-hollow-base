namespace MMH
{
	public abstract class CitizenMovementState
	{
		protected Citizen _citizen;

		public CitizenMovementState(Citizen citizen)
		{
			_citizen = citizen;
		}

		public abstract void Tick();
	}
}
