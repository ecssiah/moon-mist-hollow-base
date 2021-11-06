using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class MapSystem : MonoBehaviour
    {
        private WorldMap worldMap;
        
        private Dictionary<string, OverlayType> overlayTypes;
        private Dictionary<string, StructureType> structureTypes;
        private Dictionary<string, GroundType> groundTypes;

		private void Awake()
		{
            worldMap = new WorldMap(40);

            overlayTypes = new Dictionary<string, OverlayType>();
            structureTypes = new Dictionary<string, StructureType>();
            groundTypes = new Dictionary<string, GroundType>();

            SetupCellResources();

            GenerateWorldMap();
        }

        void Start()
        {
        }

        void Update()
        {

        }

        private void SetupCellResources()
		{
            DirectoryInfo overlayDirectoryInfo = new DirectoryInfo("Assets/Resources/Map/Type/Overlay");
            FileInfo[] overlayFileInfoList = overlayDirectoryInfo.GetFiles("*.asset");

            foreach (FileInfo fileInfo in overlayFileInfoList)
            {
                string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

                overlayTypes[basename] = Resources.Load<OverlayType>($"Map/Type/Overlay/{basename}");
            }

            DirectoryInfo structureDirectoryInfo = new DirectoryInfo("Assets/Resources/Map/Type/Structure");
            FileInfo[] structureFileInfoList = structureDirectoryInfo.GetFiles("*.asset");

            foreach (FileInfo fileInfo in structureFileInfoList)
            {
                string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

                structureTypes[basename] = Resources.Load<StructureType>($"Map/Type/Structure/{basename}");
            }

            DirectoryInfo groundDirectoryInfo = new DirectoryInfo("Assets/Resources/Map/Type/Ground");
            FileInfo[] groundFileInfoList = groundDirectoryInfo.GetFiles("*.asset");

			foreach (FileInfo fileInfo in groundFileInfoList)
			{
				string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

				groundTypes[basename] = Resources.Load<GroundType>($"Map/Type/Ground/{basename}");
			}
        }

		private void GenerateWorldMap()
		{
            for (int id = 0; id < worldMap.Area; id++)
			{
				Cell cell = new Cell
				{
					Id = id,
                    Position = IdToPosition(id),
					OverlayType = null,
					StructureType = null,
					GroundType = groundTypes["Floor1"]
				};

                worldMap.Cells.Add(cell);
            }

            Cell cellCenter = GetCell(0, 0);
            cellCenter.OverlayType = overlayTypes["Outline2"];

            Cell cellNW = GetCell(0, 20);
            cellNW.StructureType = structureTypes["Wall1"];

            Cell cellNN = GetCell(14, 14);
            cellNN.StructureType = structureTypes["Wall2"];

            Cell cellNE = GetCell(20, 0);
            cellNE.StructureType = structureTypes["Wall1"];

            Cell cellWW = GetCell(14, -14);
            cellWW.StructureType = structureTypes["Wall2"];

            Cell cellSW = GetCell(0, -20);
            cellSW.StructureType = structureTypes["Wall1"];

            Cell cellSS = GetCell(-14, -14);
            cellSS.StructureType = structureTypes["Wall2"];

            Cell cellSE = GetCell(-20, 0);
            cellSE.StructureType = structureTypes["Wall1"];

            Cell cellEE = GetCell(-14, 14);
            cellEE.StructureType = structureTypes["Wall2"];

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
