using UnityEngine;

namespace MMH
{
	public class CitizenIdle : CitizenState
	{
		private CitizenSystem citizenSystem;
		private MapSystem mapSystem;

		public CitizenIdle(CitizenSystem citizenSystem, MapSystem mapSystem)
		{
			this.citizenSystem = citizenSystem;
			this.mapSystem = mapSystem;
		}

		public override void Tick(Citizen citizen)
		{
			Debug.Log(citizen.Id);
		}
	}
}
