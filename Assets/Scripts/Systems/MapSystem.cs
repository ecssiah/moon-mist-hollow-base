using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class MapSystem : GameSystem<MapSystem>
    {
        public static Dictionary<Direction, int2> DirectionVectors;
        public static Dictionary<Direction, int> DirectionCosts;

        private WorldMap _worldMap;

        protected override void Awake()
        {
            base.Awake();

            DirectionVectors = new Dictionary<Direction, int2>
            {
                [Direction.EE] = new int2(+1, +0),
                [Direction.NE] = new int2(+1, +1),
                [Direction.NN] = new int2(+0, +1),
                [Direction.NW] = new int2(-1, +1),
                [Direction.WW] = new int2(-1, +0),
                [Direction.SW] = new int2(-1, -1),
                [Direction.SS] = new int2(+0, -1),
                [Direction.SE] = new int2(+1, -1),
            };

            DirectionCosts = new Dictionary<Direction, int>
            {
                [Direction.EE] = 10,
                [Direction.NE] = 14,
                [Direction.NN] = 10,
                [Direction.NW] = 14,
                [Direction.WW] = 10,
                [Direction.SW] = 14,
                [Direction.SS] = 10,
                [Direction.SE] = 14,
            };

            GenerateWorldMap();
        }

        void Start()
        {
        }

        private void GenerateWorldMap()
        {
            _worldMap = new WorldMap(ManagerSystem.Settings.WorldMapSize);

            for (int id = 0; id < _worldMap.Area; id++)
            {
                Cell cell = new Cell
                {
                    Id = id,
                    Solid = false,
                    Position = IdToPosition(id),
                    OverlayType = OverlayType.None,
                    StructureType = StructureType.None,
                    GroundType = GroundType.Floor1,
				};

                _worldMap.Cells.Add(cell);
            }

            SetCell(0, 0, OverlayType.Outline1);
            SetCell(0, 20, StructureType.Wall1);
            SetCell(14, 14, StructureType.Wall2);
            SetCell(20, 0, StructureType.Wall1);
            SetCell(14, -14, StructureType.Wall2);
            SetCell(0, -20, StructureType.Wall1);
            SetCell(-14, -14, StructureType.Wall2);
            SetCell(-20, 0, StructureType.Wall1);
            SetCell(-14, 14, StructureType.Wall2);

            SetCell(4, 4, StructureType.Wall1);
            SetCell(4, -4, StructureType.Wall1);
            SetCell(-4, 4, StructureType.Wall1);
            SetCell(-4, -4, StructureType.Wall1);

            SetCell(4, 0, StructureType.Wall2);
            SetCell(-4, 0, StructureType.Wall2);
            SetCell(0, 4, StructureType.Wall2);
            SetCell(0, -4, StructureType.Wall2);
        }

        public int PositionToId(int x, int y)
        {
            return (x + _worldMap.Size) + _worldMap.Width * (y + _worldMap.Size);
        }

        public int PositionToId(int2 position)
        {
            return PositionToId(position.x, position.y);
        }

        public int2 IdToPosition(int id)
        {
            return new int2(
                id % _worldMap.Width - _worldMap.Size, 
                id / _worldMap.Width - _worldMap.Size
            );
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
            if (!OnMap(x, y)) return true;

            Cell cell = GetCell(x, y);

            return cell.Solid;
        }

        public bool IsSolid(int2 position)
        {
            return IsSolid(position.x, position.y);
        }

        public int2 GetOpenCellPosition()
		{
            int2 cellPosition;

            do
            {
                cellPosition = new int2(
                    UnityEngine.Random.Range(-_worldMap.Size, _worldMap.Size + 1),
                    UnityEngine.Random.Range(-_worldMap.Size, _worldMap.Size + 1)
                );
            }
            while (IsSolid(cellPosition));

            return cellPosition;
		}
        
        public bool OnMap(int x, int y)
		{
            bool insideHorizontalLimits = x >= -_worldMap.Size && x <= _worldMap.Size;
            bool insideVerticalLimits = y >= -_worldMap.Size && y <= _worldMap.Size;

            return insideHorizontalLimits && insideVerticalLimits;
        }

        public bool OnMap(int2 position)
		{
            return OnMap(position.x, position.y);
		}

        public bool IsPassable(int2 startPosition, Direction direction)
        {
            int2 endPosition = startPosition + DirectionVectors[direction];

            if (IsSolid(endPosition)) return false;

            bool cardinalMove = (
                direction == Direction.EE || 
                direction == Direction.NN || 
                direction == Direction.WW || 
                direction == Direction.SS
            );

            if (cardinalMove)
			{
                return true;
			}
            else
			{
                bool eastCellPassable = !IsSolid(startPosition + DirectionVectors[Direction.EE]);
                bool northCellPassable = !IsSolid(startPosition + DirectionVectors[Direction.NN]);
                bool westCellPassable = !IsSolid(startPosition + DirectionVectors[Direction.WW]);
                bool southCellPassable = !IsSolid(startPosition + DirectionVectors[Direction.SS]);

                if (direction == Direction.NE)
				{
                    return northCellPassable && eastCellPassable;
				}
                else if (direction == Direction.NW)
				{
                    return northCellPassable && westCellPassable;
				}
                else if (direction == Direction.SE)
				{
                    return southCellPassable && eastCellPassable;
				}
                else if (direction == Direction.SW)
				{
                    return southCellPassable && westCellPassable;
				}
			}

            return false;
        }

        public List<Cell> GetCells()
		{
            return _worldMap.Cells;
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
    }
}
