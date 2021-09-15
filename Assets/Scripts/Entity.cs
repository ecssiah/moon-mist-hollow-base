using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class Entity : ScriptableObject
	{
		public int Id;

		public int2 Position;
		public Direction Direction;

		public EntityRenderData EntityRenderData;
	}
}