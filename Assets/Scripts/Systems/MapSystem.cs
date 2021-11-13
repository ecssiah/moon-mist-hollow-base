using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class MapSystem : MonoBehaviour
    {
        private WorldMap worldMap;

        private void Awake()
        {
            GenerateWorldMap();
        }

        void Start()
        {
        }

        private void GenerateWorldMap()
        {
            worldMap = new WorldMap(40);

            for (int id = 0; id < worldMap.Area; id++)
            {
                Cell cell = new Cell
                {
                    Id = id,
                    Position = IdToPosition(id),
                    Solid = false,
                    OverlayType = OverlayType.None,
                    StructureType = StructureType.None,
                    GroundType = GroundType.Floor1,
                };

                worldMap.Cells.Add(cell);
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
        }

        public int PositionToId(int x, int y)
        {
            return (x + worldMap.HalfWidth) + worldMap.Width * (y + worldMap.HalfWidth);
        }

        public int PositionToId(int2 position)
        {
            return PositionToId(position.x, position.y);
        }

        public int2 IdToPosition(int id)
        {
            return new int2(id % worldMap.Width - worldMap.HalfWidth, id / worldMap.Width - worldMap.HalfWidth);
        }
        
        private void SetCell(int x, int y, OverlayType overlayType)
        {
            SetCell(new int2(x, y), overlayType);
        }

        private void SetCell(int x, int y, StructureType structureType)
        {
            SetCell(new int2(x, y), structureType);
        }

        private void SetCell(int x, int y, GroundType groundType)
        {
            SetCell(new int2(x, y), groundType);
        }

        private void SetCell(int2 position, OverlayType overlayType)
		{
            Cell cell = GetCell(position);
            cell.OverlayType = overlayType;
        }

        private void SetCell(int2 position, StructureType structureType)
        {
            Cell cell = GetCell(position);
            cell.StructureType = structureType;
            cell.Solid = true;
        }

        private void SetCell(int2 position, GroundType groundType)
        {
            Cell cell = GetCell(position);
            cell.GroundType = groundType;
        }

        private void SetCellSolid(int x, int y, bool solid)
		{
            Cell cell = GetCell(x, y);
            cell.Solid = solid;
		}

        public List<Cell> GetCells()
		{
            return worldMap.Cells;
		}

        public Cell GetCell(int id)
        {
            return worldMap.Cells[id];
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
