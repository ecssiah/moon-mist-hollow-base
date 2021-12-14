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

            SetCell(+4, +4, StructureType.Wall2);
            SetCell(+4, +2, StructureType.Wall1);
            SetCell(+4, +0, StructureType.Wall2);
            SetCell(+4, -2, StructureType.Wall1);

            SetCell(+4, -4, StructureType.Wall2);
            SetCell(+2, -4, StructureType.Wall1);
            SetCell(+0, -4, StructureType.Wall2);
            SetCell(-2, -4, StructureType.Wall1);

            SetCell(-4, -4, StructureType.Wall2);
            SetCell(-4, -2, StructureType.Wall1);
            SetCell(-4, +0, StructureType.Wall2);
            SetCell(-4, +2, StructureType.Wall1);

            SetCell(-4, +4, StructureType.Wall2);
            SetCell(-2, +4, StructureType.Wall1);
            SetCell(+0, +4, StructureType.Wall2);
            SetCell(+2, +4, StructureType.Wall1);

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
