using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class Entity
	{
		private static int currentId = 0;

		public int Id;

		public int2 Position;
		public Direction Direction;

		public Entity()
		{
			Id = currentId++;
		}
	}
}