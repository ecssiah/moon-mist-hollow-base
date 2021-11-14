using System.Collections.Generic;

namespace MMH
{
	public class WorldMap
	{
		public int Size;
		public int Width => 2 * Size + 1;
		public int Area => Width * Width;

		public List<Cell> Cells;

		public WorldMap(int size)
		{
			Size = size;
			Cells = new List<Cell>(Area);
		}
	}
}

