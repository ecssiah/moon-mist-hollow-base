using UnityEngine;

namespace MMH
{
	public class CitizenWander : CitizenState
	{
		private MapSystem mapSystem;

		public CitizenWander(MapSystem mapSystem)
		{
			this.mapSystem = mapSystem;
		}

		public override void Tick()
		{
			Debug.Log("Wander Ticking");
		}
	}
}
