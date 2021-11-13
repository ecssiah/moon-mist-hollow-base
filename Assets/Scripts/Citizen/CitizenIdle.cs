using UnityEngine;

namespace MMH
{
	public class CitizenIdle : CitizenState
	{
		private MapSystem mapSystem;

		public CitizenIdle(MapSystem mapSystem)
		{
			this.mapSystem = mapSystem;
		}

		public override void Tick(Citizen citizen)
		{
			Debug.Log(citizen.Id);
		}
	}
}
