using System;

namespace MMH
{
	public abstract class CitizenMovementState
	{
		protected CitizenMovementStateType citizenMovementStateType;
		public CitizenMovementStateType CitizenMovementStateType => citizenMovementStateType;

		public abstract void Tick();
	}
}
