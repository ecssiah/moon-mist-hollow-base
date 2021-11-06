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

        private Dictionary<int, RenderData> citizenRenderData;

        private void Awake()
	    {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
            entitySystem = GameObject.Find("EntitySystem").GetComponent<EntitySystem>();

            overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            citizenRenderData = new Dictionary<int, RenderData>();

            EntitySystem.OnCreateCitizen += OnCreateCitizen;
        }

        void Start()
        {
            UpdateMapRenderData();
        }

        void Update()
        {
        
        }

		private void OnDisable()
		{
            EntitySystem.OnCreateCitizen -= OnCreateCitizen;
		}

		private void UpdateMapRenderData()
        {
            foreach (Cell cell in mapSystem.GetCells())
            {
                Vector3Int tilemapPosition = new Vector3Int(cell.Position.x, cell.Position.y, 0);

                Tile overlayTile = cell.OverlayType ? cell.OverlayType.Tile : null;

                overlayTilemap.SetTile(tilemapPosition, overlayTile);

                Tile structureTile = cell.StructureType ? cell.StructureType.Tile : null;

                structureTilemap.SetTile(tilemapPosition, structureTile);

                Tile groundTile = cell.GroundType ? cell.GroundType.Tile : null;

                groundTilemap.SetTile(tilemapPosition, groundTile);
            }
        }

        private void OnCreateCitizen(object sender, EntitySystem.OnCreateCitizenEventArgs eventArgs)
        {
            RenderData renderData = new RenderData();

            renderData.WorldGameObject = Instantiate(
                eventArgs.citizen.NationType.Prefab,
                mapSystem.GridToWorld(eventArgs.citizen.Position),
                Quaternion.identity
            );

            renderData.Animator = renderData.WorldGameObject.GetComponent<Animator>();
            renderData.Animator.Play(
                $"Base Layer.{eventArgs.citizen.NationType.name.ToLower()}-idle-{eventArgs.citizen.Direction.name.ToLower()}"
            );

            citizenRenderData[eventArgs.citizen.Id] = renderData;
        }
    }
}
