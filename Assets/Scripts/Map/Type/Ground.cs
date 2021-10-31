using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Ground Type", menuName = "MMH/Map/Type/Ground")]
public class Ground : ScriptableObject
{
	public Tile Tile;
}
