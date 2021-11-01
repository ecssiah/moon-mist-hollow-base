using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class WorldMap
	{
		public int Size;
		public int Width => 2 * Size + 1;
		public int HalfWidth => Width / 2;
		public int Area => Width * Width;

		public List<Cell> Cells;

		public WorldMap(int size)
		{
			Size = size;
			Cells = new List<Cell>(Area);
		}
	}
}

