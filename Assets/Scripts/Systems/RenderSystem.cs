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
        }

        void Update()
        {
        
        }

        private void UpdateMapRenderData()
        {
            foreach (Cell cell in mapSystem.GetCells())
            {
                Vector3Int tilemapPosition = new Vector3Int(cell.Position.x, cell.Position.y, 0);

                Overlay overlayType = cell.OverlayType;

                overlayTilemap.SetTile(tilemapPosition, overlayType ? overlayType.Tile : null);

                Structure structureType = cell.StructureType;

                structureTilemap.SetTile(tilemapPosition, structureType ? structureType.Tile : null);

                Ground groundType = cell.GroundType;
                
                groundTilemap.SetTile(tilemapPosition, groundType ? groundType.Tile : null);
            }
        }

        private void UpdateEntityRenderData()
		{
            //citizen.RenderData.WorldGameObject.transform.parent = citizensObject.transform;
        }
    }
}
