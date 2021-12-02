using UnityEngine;

namespace MMH
{
	[CreateAssetMenu(fileName = "New Game Settings", menuName = "Game Settings")]
	public class GameSettings : ScriptableObject
	{
		[Header("Map")]
		public int WorldMapSize;

		[Header("Citizen")]
		public int NumberOfCitizens;

		[Header("Camera")]
		public float PanSpeed;
		public float ZoomSpeed;
	}
}
