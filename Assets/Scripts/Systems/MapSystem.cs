using System;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
	public class MapSystem : SimulationSystem
	{
		public static EventHandler<OnMapEventArgs> OnUpdateMapRender;

		private WorldMap _worldMap;

		public override void Init()
		{
			SetupEvents();

			GenerateWorldMap();
		}

		private void SetupEvents()
		{
			SimulationManager.OnTick += Tick;
		}

		private void GenerateWorldMap()
		{
			_worldMap = new WorldMap(MapInfo.WorldMapSize);

			for (int id = 0; id < _worldMap.Area; id++)
			{
				Cell cell = new Cell(id)
				{
					Solid = false,
					Position = IdToPosition(id),
					OverlayType = OverlayType.None,
					StructureType = StructureType.None,
					GroundType = GroundType.Floor1,
				};

				_worldMap.Cells.Add(cell);
			}

			SetCell(+0, +0, GroundType.Floor2);

			SetCell(+6, +6, StructureType.Wall2);
			SetCell(+6, -6, StructureType.Wall2);
			SetCell(-6, -6, StructureType.Wall2);
			SetCell(-6, +6, StructureType.Wall2);

			SetCellBox(+3, +3, -3, -3, GroundType.Floor2, true);
			SetCellBox(-3, -3, +3, +3, StructureType.Wall1);

			OnUpdateMapRender?.Invoke(this, new OnMapEventArgs { WorldMap = _worldMap });
		}

		protected override void Tick(object sender, OnTickArgs eventArgs)
		{

		}

		public override void Quit()
		{
			SimulationManager.OnTick -= Tick;
		}

		public Cell GetCell(int id)
		{
			if (id >= _worldMap.Area) return null;

			return _worldMap.Cells[id];
		}

		public Cell GetCell(int x, int y)
		{
			int cellId = PositionToId(new int2(x, y));

			return GetCell(cellId);
		}

		public Cell GetCell(int2 position)
		{
			return GetCell(position.x, position.y);
		}

		private void SetCell(int x, int y, OverlayType overlayType)
		{
			if (OnMap(x, y))
			{
				Cell cell = GetCell(x, y);
				cell.OverlayType = overlayType;
			}
		}

		private void SetCell(int x, int y, StructureType structureType)
		{
			if (OnMap(x, y))
			{
				Cell cell = GetCell(x, y);
				cell.Solid = true;
				cell.StructureType = structureType;
			}
		}

		private void SetCell(int x, int y, GroundType groundType)
		{
			if (OnMap(x, y))
			{
				Cell cell = GetCell(x, y);
				cell.GroundType = groundType;
			}
		}

		private void SetCell(int2 position, OverlayType overlayType)
		{
			SetCell(position.x, position.y, overlayType);
		}

		private void SetCell(int2 position, StructureType structureType)
		{
			SetCell(position.x, position.y, structureType);
		}

		private void SetCell(int2 position, GroundType groundType)
		{
			SetCell(position.x, position.y, groundType);
		}

		private void SetCellBox(int x1, int y1, int x2, int y2, GroundType groundType, bool fill = false)
		{
			if (x1 > x2)
			{
				var temp = x1;
				x1 = x2;
				x2 = temp;
			}

			if (y1 > y2)
			{
				var temp = y1;
				y1 = y2;
				y2 = temp;
			}

			for (int x = x1; x <= x2; x++)
			{
				for (int y = y1; y <= y2; y++)
				{
					bool onEdge = x == x1 || y == y1 || x == x2 || y == y2;

					if (fill || onEdge)
					{
						SetCell(x, y, groundType);
					}
				}
			}
		}

		private void SetCellBox(int x1, int y1, int x2, int y2, StructureType structureType, bool fill = false)
		{
			if (x1 > x2)
			{
				var temp = x1;
				x1 = x2;
				x2 = temp;
			}

			if (y1 > y2)
			{
				var temp = y1;
				y1 = y2;
				y2 = temp;
			}

			for (int x = x1; x <= x2; x++)
			{
				for (int y = y1; y <= y2; y++)
				{
					bool onEdge = x == x1 || y == y1 || x == x2 || y == y2;

					if (fill || onEdge)
					{
						SetCell(x, y, structureType);
					}
				}
			}
		}

		private void SetCellBox(int x1, int y1, int x2, int y2, OverlayType overlayType, bool fill = false)
		{
			if (x1 > x2)
			{
				var temp = x1;
				x1 = x2;
				x2 = temp;
			}

			if (y1 > y2)
			{
				var temp = y1;
				y1 = y2;
				y2 = temp;
			}

			for (int x = x1; x <= x2; x++)
			{
				for (int y = y1; y <= y2; y++)
				{
					bool onEdge = x == x1 || y == y1 || x == x2 || y == y2;

					if (fill || onEdge)
					{
						SetCell(x, y, overlayType);
					}
				}
			}
		}

		private void SetCellBox(int2 position1, int2 position2, GroundType groundType, bool fill = false)
		{
			SetCellBox(position1.x, position1.y, position2.x, position2.y, groundType, fill);
		}

		private void SetCellBox(int2 position1, int2 position2, StructureType structureType, bool fill = false)
		{
			SetCellBox(position1.x, position1.y, position2.x, position2.y, structureType, fill);
		}

		private void SetCellBox(int2 position1, int2 position2, OverlayType overlayType, bool fill = false)
		{
			SetCellBox(position1.x, position1.y, position2.x, position2.y, overlayType, fill);
		}

		private void SetSolid(int x, int y, bool solid)
		{
			if (OnMap(x, y))
			{
				Cell cell = GetCell(x, y);
				cell.Solid = solid;
			}
		}

		private void SetSolid(int2 position, bool solid)
		{
			SetSolid(position.x, position.y, solid);
		}

		public bool IsSolid(int x, int y)
		{
			if (OnMap(x, y))
			{
				Cell cell = GetCell(x, y);

				return cell.Solid;
			}
			else
			{
				return true;
			}
		}

		public bool IsSolid(int2 position)
		{
			return IsSolid(position.x, position.y);
		}

		public bool OnMap(int x, int y)
		{
			bool insideHorizontalBounds = x >= -_worldMap.Size && x <= _worldMap.Size;
			bool insideVerticalBounds = y >= -_worldMap.Size && y <= _worldMap.Size;

			return insideHorizontalBounds && insideVerticalBounds;
		}

		public bool OnMap(int2 position)
		{
			return OnMap(position.x, position.y);
		}

		public bool IsPassable(int2 startPosition, Direction direction)
		{
			int2 endPosition = startPosition + MapInfo.DirectionVectors[direction];

			if (IsSolid(endPosition)) return false;

			bool cardinalMove = (
				direction == Direction.EE ||
				direction == Direction.NN ||
				direction == Direction.WW ||
				direction == Direction.SS
			);

			if (cardinalMove) return true;

			bool eastPassable = !IsSolid(startPosition + MapInfo.DirectionVectors[Direction.EE]);
			bool northPassable = !IsSolid(startPosition + MapInfo.DirectionVectors[Direction.NN]);
			bool westPassable = !IsSolid(startPosition + MapInfo.DirectionVectors[Direction.WW]);
			bool southPassable = !IsSolid(startPosition + MapInfo.DirectionVectors[Direction.SS]);

			if (direction == Direction.NE)
			{
				return northPassable && eastPassable;
			}
			else if (direction == Direction.NW)
			{
				return northPassable && westPassable;
			}
			else if (direction == Direction.SE)
			{
				return southPassable && eastPassable;
			}
			else if (direction == Direction.SW)
			{
				return southPassable && westPassable;
			}

			return false;
		}

		public int2 IdToPosition(int id)
		{
			int x = id % _worldMap.Width - _worldMap.Size;
			int y = id / _worldMap.Width - _worldMap.Size;

			return new int2(x, y);
		}

		public int PositionToId(int x, int y)
		{
			return (x + _worldMap.Size) + _worldMap.Width * (y + _worldMap.Size);
		}

		public int PositionToId(int2 position)
		{
			return PositionToId(position.x, position.y);
		}

		public int2 GetOpenCellPosition()
		{
			int2 cellPosition;

			do
			{
				cellPosition = new int2(
					Utils.RandomRange(-_worldMap.Size, _worldMap.Size),
					Utils.RandomRange(-_worldMap.Size, _worldMap.Size)
				);
			}
			while (IsSolid(cellPosition));

			return cellPosition;
		}
	}
}
