using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class WorldMapSystem : MonoBehaviour
    {
        private WorldMap worldMap;
        public WorldMap WorldMap => worldMap;

        private Dictionary<string, Overlay> overlayTypes;
        private Dictionary<string, Structure> structureTypes;
        private Dictionary<string, Ground> groundTypes;

        private Tilemap overlayTilemap;
        private Tilemap structureTilemap;
        private Tilemap groundTilemap;

		private void Awake()
		{
            overlayTypes = new Dictionary<string, Overlay>();
            structureTypes = new Dictionary<string, Structure>();
            groundTypes = new Dictionary<string, Ground>();

            overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            worldMap = gameObject.AddComponent<WorldMap>();

            SetupCellResources();
        }

		void Start()
        {
            GenerateWorldMap();

            UpdateRenderData();
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

                overlayTypes[basename] = Resources.Load<Overlay>($"Map/Type/Overlay/{basename}");
            }

            DirectoryInfo structureDirectoryInfo = new DirectoryInfo("Assets/Resources/Map/Type/Structure");
            FileInfo[] structureFileInfoList = structureDirectoryInfo.GetFiles("*.asset");

            foreach (FileInfo fileInfo in structureFileInfoList)
            {
                string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

                structureTypes[basename] = Resources.Load<Structure>($"Map/Type/Structure/{basename}");
            }

            DirectoryInfo groundDirectoryInfo = new DirectoryInfo("Assets/Resources/Map/Type/Ground");
            FileInfo[] groundFileInfoList = groundDirectoryInfo.GetFiles("*.asset");

			foreach (FileInfo fileInfo in groundFileInfoList)
			{
				string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

				groundTypes[basename] = Resources.Load<Ground>($"Map/Type/Ground/{basename}");
			}
        }

		private void GenerateWorldMap()
		{
            worldMap.InitMap(40);

            for (int id = 0; id < worldMap.Area; id++)
			{
				Cell cell = new Cell
				{
					Id = id,
					OverlayType = null,
					StructureType = null,
					GroundType = groundTypes["Floor1"]
				};

				worldMap.Cells.Add(cell);
            }

            Cell cellCenter = worldMap.GetCell(0, 0);
            cellCenter.OverlayType = overlayTypes["Outline2"];

            Cell cellNW = worldMap.GetCell(0, 20);
            cellNW.StructureType = structureTypes["Wall1"];

            Cell cellNN = worldMap.GetCell(14, 14);
            cellNN.StructureType = structureTypes["Wall2"];

            Cell cellNE = worldMap.GetCell(20, 0);
            cellNE.StructureType = structureTypes["Wall1"];

            Cell cellWW = worldMap.GetCell(14, -14);
            cellWW.StructureType = structureTypes["Wall2"];

            Cell cellSW = worldMap.GetCell(0, -20);
            cellSW.StructureType = structureTypes["Wall1"];

            Cell cellSS = worldMap.GetCell(-14, -14);
            cellSS.StructureType = structureTypes["Wall2"];

            Cell cellSE = worldMap.GetCell(-20, 0);
            cellSE.StructureType = structureTypes["Wall1"];

            Cell cellEE = worldMap.GetCell(-14, 14);
            cellEE.StructureType = structureTypes["Wall2"];

        }

        private void UpdateRenderData()
		{
            foreach (Cell cell in worldMap.Cells)
			{
                int2 cellPosition = worldMap.IdToPosition(cell.Id);

                Vector3Int tilemapPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);

                Overlay overlayType = worldMap.Cells[cell.Id].OverlayType;

                if (overlayType)
                {
                    overlayTilemap.SetTile(tilemapPosition, overlayType.Tile);
                }

                Structure structureType = worldMap.Cells[cell.Id].StructureType;

                if (structureType)
                {
                    structureTilemap.SetTile(tilemapPosition, structureType.Tile);
                }

                Ground groundType = worldMap.Cells[cell.Id].GroundType;

                if (groundType)
                {
                    groundTilemap.SetTile(tilemapPosition, groundType.Tile);
                }
            }
		}
    }
}
