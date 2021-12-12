using UnityEngine;

namespace MMH
{
	[CreateAssetMenu(fileName = "New Simulation Settings", menuName = "Simulation Settings")]
	public class SimulationSettings : ScriptableObject
	{
		[Header("Time")]
		public float TickDuration = 0.2f;

		[Header("Map")]
		public int WorldMapSize = 32;

		[Header("Citizen")]
		public int NumberOfCitizens = 800;
	}
}
