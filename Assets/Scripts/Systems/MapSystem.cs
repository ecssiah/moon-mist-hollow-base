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

        void Update()
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
            Cell cellCenter = GetCell(position);
            cellCenter.OverlayType = overlayType;
        }

        private void SetCell(int2 position, StructureType structureType)
        {
            Cell cellCenter = GetCell(position);
            cellCenter.StructureType = structureType;
        }

        private void SetCell(int2 position, GroundType groundType)
        {
            Cell cellCenter = GetCell(position);
            cellCenter.GroundType = groundType;
        }

        public WorldMap GetWorldMap()
		{
            return worldMap;
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
