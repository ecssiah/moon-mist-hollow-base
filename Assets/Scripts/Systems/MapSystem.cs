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
            worldMap = new WorldMap(40);

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

            Cell cellCenter = GetCell(0, 0);
            cellCenter.OverlayType = OverlayType.Outline1;

            Cell cellNW = GetCell(0, 20);
            cellNW.StructureType = StructureType.Wall1;

            Cell cellNN = GetCell(14, 14);
            cellNN.StructureType = StructureType.Wall2;

            Cell cellNE = GetCell(20, 0);
            cellNE.StructureType = StructureType.Wall1;

            Cell cellWW = GetCell(14, -14);
            cellWW.StructureType = StructureType.Wall2;

            Cell cellSW = GetCell(0, -20);
            cellSW.StructureType = StructureType.Wall1;

            Cell cellSS = GetCell(-14, -14);
            cellSS.StructureType = StructureType.Wall2;

            Cell cellSE = GetCell(-20, 0);
            cellSE.StructureType = StructureType.Wall1;

            Cell cellEE = GetCell(-14, 14);
            cellEE.StructureType = StructureType.Wall2;
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

        public Vector3 GridToWorld(int2 gridPosition)
        {
            Vector3 screenPosition = new Vector3
            {
                x = (gridPosition.x - gridPosition.y) * 1,
                y = (gridPosition.x + gridPosition.y) * 1/2f + 1/4f,
                z = 0,
            };

            return screenPosition;
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
