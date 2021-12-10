using UnityEngine;

namespace MMH
{
	[CreateAssetMenu(fileName = "New Simulation Settings", menuName = "Simulation Settings")]
	public class SimulationSettings : ScriptableObject
	{
		[Header("Time")]
		public float TickDuration;

		[Header("Map")]
		public int WorldMapSize;

		[Header("Citizen")]
		public int NumberOfCitizens;
	}
}
