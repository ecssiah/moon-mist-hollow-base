namespace MMH
{
	public abstract class CitizenState
	{
		protected CitizenStateType citizenStateType;

		public CitizenStateType CitizenStateType => citizenStateType;

		public abstract void Tick();
	}
}
