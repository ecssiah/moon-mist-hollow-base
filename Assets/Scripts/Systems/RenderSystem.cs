using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class RenderSystem : GameSystem<RenderSystem>
    {
        private Tilemap overlayTilemap;
        private Tilemap structureTilemap;
        private Tilemap groundTilemap;

        private Dictionary<OverlayType, Tile> overlayTiles;
        private Dictionary<StructureType, Tile> structureTiles;
        private Dictionary<GroundType, Tile> groundTiles;

        private Dictionary<Nation, GameObject> nationPrefabs;

        private GameObject citizenGameObject;

        private Dictionary<int, RenderData> citizenRenderData;

        protected override void Awake()
	    {
            base.Awake();

            overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            citizenGameObject = GameObject.Find("Citizens");

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

            CitizenSystem.OnCreateCitizen += OnCreateCitizen;

            CitizenWander.OnUpdateCitizenDirection += OnUpdateCitizenDirection;
            CitizenWander.OnUpdateCitizenPosition += OnUpdateCitizenPosition;
        }

        void Start()
        {
            UpdateMapRenderData();
        }

		private void OnDisable()
		{
            CitizenSystem.OnCreateCitizen -= OnCreateCitizen;

            CitizenWander.OnUpdateCitizenDirection -= OnUpdateCitizenDirection;
            CitizenWander.OnUpdateCitizenPosition -= OnUpdateCitizenPosition;
		}

		private void UpdateMapRenderData()
        {
            foreach (Cell cell in MapSystem.Instance.GetCells())
            {
                Vector3Int tilemapPosition = new Vector3Int(cell.Position.x, cell.Position.y, 0);

                overlayTilemap.SetTile(tilemapPosition, overlayTiles[cell.OverlayType]);
                structureTilemap.SetTile(tilemapPosition, structureTiles[cell.StructureType]);
                groundTilemap.SetTile(tilemapPosition, groundTiles[cell.GroundType]);
            }
        }

        private void OnCreateCitizen(object sender, CitizenSystem.OnCreateCitizenArgs eventArgs)
        {
            RenderData renderData = new RenderData();

            citizenRenderData[eventArgs.Citizen.Id] = renderData;

            Vector3 startPosition = GridToWorld(eventArgs.Citizen.Position);
            startPosition.z = -eventArgs.Citizen.Id * 0.001f;

            renderData.WorldGameObject = Instantiate(
                nationPrefabs[eventArgs.Citizen.Nation], startPosition, Quaternion.identity
            );
            
            renderData.WorldGameObject.transform.parent = citizenGameObject.transform;

            renderData.Animator = renderData.WorldGameObject.GetComponent<Animator>();

            PlayAnimation(eventArgs.Citizen, CitizenAnimationType.Idle);
        }

        private void OnUpdateCitizenDirection(object sender, OnUpdateCitizenDirectionArgs eventArgs)
		{
            PlayAnimation(eventArgs.Citizen, CitizenAnimationType.Idle);
        }

        private void OnUpdateCitizenPosition(object sender, OnUpdateCitizenPositionArgs eventArgs)
		{
            StartCoroutine(MoveCitizen(eventArgs));
        }

        private IEnumerator MoveCitizen(OnUpdateCitizenPositionArgs eventArgs)
		{
            RenderData renderData = citizenRenderData[eventArgs.Citizen.Id];

            float timer = 0;
            float duration = TimeSystem.TICK_DURATION * eventArgs.Citizen.Cooldown;

            Vector3 startPosition = GridToWorld(eventArgs.StartPosition);
            startPosition.z = -eventArgs.Citizen.Id * 0.001f;
            Vector3 endPosition = GridToWorld(eventArgs.Citizen.Position);
            endPosition.z = -eventArgs.Citizen.Id * 0.001f;

            PlayAnimation(eventArgs.Citizen, CitizenAnimationType.Walk);

            while (timer < duration)
			{
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, timer / duration);
                
                renderData.WorldGameObject.transform.position = newPosition;
                
                timer += Time.deltaTime;

                yield return null;
			}

            PlayAnimation(eventArgs.Citizen, CitizenAnimationType.Idle);

            renderData.WorldGameObject.transform.position = endPosition;
		}

        private Vector3 GridToWorld(int x, int y)
		{
            Vector3 worldPosition = groundTilemap.layoutGrid.CellToWorld(new Vector3Int(x, y, 0));
            worldPosition.y += 1 / 4f;

            return worldPosition;
        }

        private Vector3 GridToWorld(int2 gridPosition)
        {
            return GridToWorld(gridPosition.x, gridPosition.y);
        }

        private void PlayAnimation(Citizen citizen, CitizenAnimationType animationType)
		{
            RenderData renderData = citizenRenderData[citizen.Id];

            renderData.Animator.Play(
                $"Base Layer.{citizen.Nation}-{animationType}-{citizen.Direction}"
            );
        }
    }
}
