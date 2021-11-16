using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class RenderSystem : MonoBehaviour
    {
        private static RenderSystem _instance;
        public static RenderSystem Instance { get { return _instance; } }

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
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

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

            CitizenSystem.OnCreateCitizen += OnCreateCitizen;

            CitizenWander.OnUpdateCitizenPosition += OnUpdateCitizenPosition;
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
            CitizenSystem.OnCreateCitizen -= OnCreateCitizen;
            
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

        private void OnCreateCitizen(object sender, CitizenSystem.OnCreateCitizenEventArgs eventArgs)
        {
            RenderData renderData = new RenderData();

            renderData.WorldGameObject = Instantiate(
                nationPrefabs[eventArgs.Citizen.Nation],
                GridToWorld(eventArgs.Citizen.Position),
                Quaternion.identity
            );

            renderData.Animator = renderData.WorldGameObject.GetComponent<Animator>();
            renderData.Animator.Play(
                $"Base Layer.{eventArgs.Citizen.Nation}-Idle-{eventArgs.Citizen.Direction}"
            );

            citizenRenderData[eventArgs.Citizen.Id] = renderData;
        }

        private void OnUpdateCitizenPosition(object sender, OnUpdateCitizenPositionEventArgs eventArgs)
		{
            RenderData renderData = citizenRenderData[eventArgs.Citizen.Id];
            
            renderData.Animator.Play(
                $"Base Layer.{eventArgs.Citizen.Nation}-Walk-{eventArgs.Citizen.Direction}"
            );

            StartCoroutine(MoveCitizen(renderData, eventArgs));
        }

        private IEnumerator MoveCitizen(RenderData renderData, OnUpdateCitizenPositionEventArgs eventArgs)
		{
            float timer = 0;
            float duration = TimeSystem.TICK_DURATION * eventArgs.Ticks;

            Vector3 startPosition = GridToWorld(eventArgs.PreviousPosition);
            Vector3 endPosition = GridToWorld(eventArgs.Citizen.Position);

            while (timer < duration)
			{
                renderData.WorldGameObject.transform.position = Vector3.Lerp(startPosition, endPosition, timer / duration);

                timer += Time.deltaTime;

                yield return null;
			}

            renderData.WorldGameObject.transform.position = endPosition;

            renderData.Animator.Play(
                $"Base Layer.{eventArgs.Citizen.Nation}-Idle-{eventArgs.Citizen.Direction}"
            );
        }

        private Vector3 GridToWorld(int2 gridPosition)
        {
            Vector3 screenPosition = new Vector3
            {
                x = (gridPosition.x - gridPosition.y) * 1/2f,
                y = (gridPosition.x + gridPosition.y) * 1/4f + 1/4f,
                z = 0,
            };

            return screenPosition;
        }
    }
}
