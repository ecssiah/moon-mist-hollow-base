using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class RenderSystem : MonoBehaviour
    {
        private MapSystem mapSystem;
        private EntitySystem entitySystem;

        private Tilemap overlayTilemap;
        private Tilemap structureTilemap;
        private Tilemap groundTilemap;

        private void Awake()
	    {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
            entitySystem = GameObject.Find("EntitySystem").GetComponent<EntitySystem>();

            overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();
        }

	    void Start()
        {
            UpdateMapRenderData();
            //UpdateEntityRenderData();
        }

        void Update()
        {
        
        }

        private void UpdateMapRenderData()
        {
            foreach (Cell cell in mapSystem.GetCells())
            {
                int2 cellPosition = mapSystem.IdToPosition(cell.Id);

                Vector3Int tilemapPosition = new Vector3Int(cellPosition.x, cellPosition.y, 0);

                Overlay overlayType = mapSystem.GetCell(cell.Id).OverlayType;

                if (overlayType)
                {
                    overlayTilemap.SetTile(tilemapPosition, overlayType.Tile);
                }

                Structure structureType = mapSystem.GetCell(cell.Id).StructureType;

                if (structureType)
                {
                    structureTilemap.SetTile(tilemapPosition, structureType.Tile);
                }

                Ground groundType = mapSystem.GetCell(cell.Id).GroundType;

                if (groundType)
                {
                    groundTilemap.SetTile(tilemapPosition, groundType.Tile);
                }
            }
        }

        private void UpdateEntityRenderData()
		{
            //citizen.RenderData.WorldGameObject.transform.parent = citizensObject.transform;
        }
    }
}
