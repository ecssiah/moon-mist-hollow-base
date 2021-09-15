using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class WorldMap : MonoBehaviour
	{
		private int size;
		public int Size { get => size; set => size = value; }
		public int Width => 2 * Size + 1;
		public int HalfWidth => Width / 2;
		public int Area => Width * Width;

		private List<Cell> cells;
		public List<Cell> Cells => cells;

		public void InitMap(int size)
		{
			this.size = size;
			cells = new List<Cell>(size);
		}

		public int PositionToId(int x, int y)
		{
			return (x + HalfWidth) + Width * (y + HalfWidth);
		}

		public int PositionToId(int2 position)
		{
			return PositionToId(position.x, position.y);
		}

		public int2 IdToPosition(int id)
		{
			return new int2(id % Width - HalfWidth, id / Width - HalfWidth);
		}

		public Vector3 GridToWorld(int2 gridPosition)
		{
			Vector3 screenPosition = new Vector3
			{
				x = (gridPosition.x - gridPosition.y) * 1,
				y = (gridPosition.x + gridPosition.y) / 2,
				z = 0,
			};

			return screenPosition;
		}

		public Cell GetCell(int x, int y)
		{
			int cellId = PositionToId(new int2(x, y));

			return Cells[cellId];
		}

		public Cell GetCell(int2 position)
		{
			return GetCell(position.x, position.y);
		}
	}
}

