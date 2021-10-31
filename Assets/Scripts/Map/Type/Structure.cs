using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Structure Type", menuName = "MMH/Map/Type/Structure")]
public class Structure : ScriptableObject
{
	public Tile Tile;
}
