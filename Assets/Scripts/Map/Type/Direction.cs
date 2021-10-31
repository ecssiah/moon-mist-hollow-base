using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Direction", menuName = "MMH/Map/Type/Direction")]
public class Direction : ScriptableObject
{
	public int2 Vector;
}
