using UnityEngine;

namespace MMH
{
	public class CitizenIdle : CitizenState
	{
		private Citizen citizen;

		public CitizenIdle(Citizen citizen)
		{
			this.citizen = citizen;
		}

		public override void Tick()
		{
		}
	}
}
