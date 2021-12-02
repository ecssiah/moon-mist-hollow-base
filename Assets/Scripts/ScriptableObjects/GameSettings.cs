using UnityEngine;

namespace MMH
{
	[CreateAssetMenu(fileName = "New Game Settings", menuName = "Game Settings")]
	public class GameSettings : ScriptableObject
	{
		public int NumberOfCitizens;

		public int WorldMapSize;
	}
}
