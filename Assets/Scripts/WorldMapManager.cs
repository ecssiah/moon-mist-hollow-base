using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class WorldMapManager : MonoBehaviour
    {
        private WorldMap worldMap;

        private Dictionary<string, OverlayType> overlayTypes;
        private Dictionary<string, StructureType> structureTypes;
        private Dictionary<string, GroundType> groundTypes;

        private Tilemap overlayTilemap;
        private Tilemap structureTilemap;
        private Tilemap groundTilemap;

        void Start()
        {
            overlayTypes = new Dictionary<string, OverlayType>();
            structureTypes = new Dictionary<string, StructureType>();
            groundTypes = new Dictionary<string, GroundType>();

            overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            SetupCellResources();

            GenerateWorldMap();
            
            UpdateRenderData();
        }


        void Update()
        {

        }

        void SetupCellResources()
		{
            DirectoryInfo overlayDirectoryInfo = new DirectoryInfo("Assets/Resources/Types/OverlayTypes");
            FileInfo[] overlayFileInfoList = overlayDirectoryInfo.GetFiles("*.asset");

            foreach (FileInfo fileInfo in overlayFileInfoList)
            {
                string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

                overlayTypes[basename] = Resources.Load<OverlayType>($"Types/OverlayTypes/{basename}");
            }

            DirectoryInfo structureDirectoryInfo = new DirectoryInfo("Assets/Resources/Types/StructureTypes");
            FileInfo[] structureFileInfoList = structureDirectoryInfo.GetFiles("*.asset");

            foreach (FileInfo fileInfo in structureFileInfoList)
            {
                string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

                structureTypes[basename] = Resources.Load<StructureType>($"Types/StructureTypes/{basename}");
            }

            DirectoryInfo groundDirectoryInfo = new DirectoryInfo("Assets/Resources/Types/GroundTypes");
            FileInfo[] groundFileInfoList = groundDirectoryInfo.GetFiles("*.asset");

			foreach (FileInfo fileInfo in groundFileInfoList)
			{
				string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

				groundTypes[basename] = Resources.Load<GroundType>($"Types/GroundTypes/{basename}");
			}
        }

		void GenerateWorldMap()
		{
            worldMap = ScriptableObject.CreateInstance<WorldMap>();
            worldMap.InitMap(40);

            for (int id = 0; id < worldMap.Area; id++)
			{
                Cell cell = new Cell()
                {
                    Id = id,
                    OverlayType = null,
                    StructureType = null,
                    GroundType = groundTypes["Floor1"],
                };

                worldMap.Cells.Add(cell);
            }

            Cell cellNW = worldMap.GetCell(0, 20);
            cellNW.StructureType = structureTypes["Wall1"];

            Cell cellNN = worldMap.GetCell(15, 15);
            cellNN.StructureType = structureTypes["Wall2"];

            Cell cellNE = worldMap.GetCell(20, 0);
            cellNE.StructureType = structureTypes["Wall1"];

            Cell cellWW = worldMap.GetCell(15, -15);
            cellWW.StructureType = structureTypes["Wall2"];

            Cell cellSW = worldMap.GetCell(0, -20);
            cellSW.StructureType = structureTypes["Wall1"];

            Cell cellSS = worldMap.GetCell(-15, -15);
            cellSS.StructureType = structureTypes["Wall2"];

            Cell cellSE = worldMap.GetCell(-20, 0);
            cellSE.StructureType = structureTypes["Wall1"];

            Cell cellEE = worldMap.GetCell(-15, 15);
            cellEE.StructureType = structureTypes["Wall2"];
        }

        void UpdateRenderData()
		{
            foreach (Cell cell in worldMap.Cells)
			{
                int2 cellPosition = worldMap.IdToPosition(cell.Id);

                Vector3Int tilemapPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);

                OverlayType overlayType = worldMap.Cells[cell.Id].OverlayType;

                if (overlayType)
                {
                    Tile overlayTile = overlayType.Tile;
                    overlayTilemap.SetTile(tilemapPosition, overlayTile);
                }

                StructureType structureType = worldMap.Cells[cell.Id].StructureType;

                if (structureType)
                {
                    Tile structureTile = structureType.Tile;
                    structureTilemap.SetTile(tilemapPosition, structureTile);
                }

                GroundType groundType = worldMap.Cells[cell.Id].GroundType;

                if (groundType)
                {
                    Tile groundTile = groundType.Tile;
                    groundTilemap.SetTile(tilemapPosition, groundTile);
                }
            }
		}
    }
}
