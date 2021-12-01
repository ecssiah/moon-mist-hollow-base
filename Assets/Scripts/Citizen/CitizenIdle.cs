using UnityEngine;

namespace MMH
{
	public class CitizenIdle : CitizenState
	{
		private Citizen citizen;

		public CitizenIdle(Citizen citizen)
		{
			this.citizen = citizen;
			citizenStateType = CitizenStateType.CitizenIdle;
		}

		public override void Tick()
		{
		}
	}
}
