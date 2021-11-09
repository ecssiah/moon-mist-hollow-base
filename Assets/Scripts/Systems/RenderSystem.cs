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

        private Dictionary<OverlayType, Tile> overlayTiles;
        private Dictionary<StructureType, Tile> structureTiles;
        private Dictionary<GroundType, Tile> groundTiles;

        private Dictionary<Nation, GameObject> nationPrefabs;

        private Dictionary<int, RenderData> citizenRenderData;

        private void Awake()
	    {
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();
            entitySystem = GameObject.Find("EntitySystem").GetComponent<EntitySystem>();

            overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            citizenRenderData = new Dictionary<int, RenderData>();

			nationPrefabs = new Dictionary<Nation, GameObject>
			{
				[Nation.Guys] = Resources.Load<GameObject>("Prefabs/Guys"),
				[Nation.Kailt] = Resources.Load<GameObject>("Prefabs/Kailt"),
				[Nation.Taylor] = Resources.Load<GameObject>("Prefabs/Taylor"),
			};

            overlayTiles = new Dictionary<OverlayType, Tile>
            {
                [OverlayType.None] = null,
                [OverlayType.Outline1] = Resources.Load<Tile>("Tiles/outline-1"),
                [OverlayType.Outline2] = Resources.Load<Tile>("Tiles/outline-2"),
            };

            structureTiles = new Dictionary<StructureType, Tile>
            {
                [StructureType.None] = null,
                [StructureType.Wall1] = Resources.Load<Tile>("Tiles/wall-1"),
                [StructureType.Wall2] = Resources.Load<Tile>("Tiles/wall-2"),
            };

            groundTiles = new Dictionary<GroundType, Tile>
            {
                [GroundType.None] = null,
                [GroundType.Floor1] = Resources.Load<Tile>("Tiles/floor-1"),
                [GroundType.Floor2] = Resources.Load<Tile>("Tiles/floor-2"),
            };

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

                overlayTilemap.SetTile(tilemapPosition, overlayTiles[cell.OverlayType]);
                structureTilemap.SetTile(tilemapPosition, structureTiles[cell.StructureType]);
                groundTilemap.SetTile(tilemapPosition, groundTiles[cell.GroundType]);
            }
        }

        private void OnCreateCitizen(object sender, EntitySystem.OnCreateCitizenEventArgs eventArgs)
        {
            RenderData renderData = new RenderData();

            renderData.WorldGameObject = Instantiate(
                nationPrefabs[eventArgs.citizen.Nation],
                GridToWorld(eventArgs.citizen.Position),
                Quaternion.identity
            );

            renderData.Animator = renderData.WorldGameObject.GetComponent<Animator>();
            renderData.Animator.Play(
                $"Base Layer.{eventArgs.citizen.Nation}-Idle-{eventArgs.citizen.Direction}"
            );

            citizenRenderData[eventArgs.citizen.Id] = renderData;
        }

        private Vector3 GridToWorld(int2 gridPosition)
        {
            Vector3 screenPosition = new Vector3
            {
                x = (gridPosition.x - gridPosition.y) * 1,
                y = (gridPosition.x + gridPosition.y) * 1 / 2f + 1 / 4f,
                z = 0,
            };

            return screenPosition;
        }
    }
}
