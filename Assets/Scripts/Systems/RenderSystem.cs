using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class RenderSystem : GameSystem<RenderSystem>
    {
		private const float Z_OFFSET = 0.001f;

		private Grid _grid;

        private Tilemap _overlayTilemap;
        private Tilemap _structureTilemap;
        private Tilemap _groundTilemap;

        private Dictionary<OverlayType, Tile> _overlayTiles;
        private Dictionary<StructureType, Tile> _structureTiles;
        private Dictionary<GroundType, Tile> _groundTiles;

        private GameObject _citizenGameObject;

        private Dictionary<Nation, GameObject> _nationPrefabs;

        private Dictionary<int, RenderData> _citizenRenderData;

        protected override void Awake()
	    {
            base.Awake();

            SetupResources();
            SetupEvents();
        }

        void Start()
        {
            UpdateMapRenderData();
        }

		private void OnDisable()
		{
			EntitySystem.OnCreateCitizen -= OnCreateCitizen;

            Citizen.OnUpdateCitizenRenderDirection -= OnUpdateCitizenRenderDirection;
            Citizen.OnUpdateCitizenRenderPosition -= OnUpdateCitizenRenderPosition;
		}

        private void SetupResources()
		{
            _grid = GameObject.Find("Grid").GetComponent<Grid>();

            _overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            _structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            _groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

            _citizenGameObject = GameObject.Find("Citizens");

            _citizenRenderData = new Dictionary<int, RenderData>();

            _nationPrefabs = new Dictionary<Nation, GameObject>
            {
                [Nation.Guys] = Resources.Load<GameObject>("Prefabs/Guys"),
                [Nation.Kailt] = Resources.Load<GameObject>("Prefabs/Kailt"),
                [Nation.Taylor] = Resources.Load<GameObject>("Prefabs/Taylor"),
            };

            _overlayTiles = new Dictionary<OverlayType, Tile>
            {
                [OverlayType.None] = null,
                [OverlayType.Outline1] = Resources.Load<Tile>("Tiles/outline-1"),
                [OverlayType.Outline2] = Resources.Load<Tile>("Tiles/outline-2"),
            };

            _structureTiles = new Dictionary<StructureType, Tile>
            {
                [StructureType.None] = null,
                [StructureType.Wall1] = Resources.Load<Tile>("Tiles/wall-1"),
                [StructureType.Wall2] = Resources.Load<Tile>("Tiles/wall-2"),
            };

            _groundTiles = new Dictionary<GroundType, Tile>
            {
                [GroundType.None] = null,
                [GroundType.Floor1] = Resources.Load<Tile>("Tiles/floor-1"),
                [GroundType.Floor2] = Resources.Load<Tile>("Tiles/floor-2"),
            };
        }

        private void SetupEvents()
		{
            EntitySystem.OnCreateCitizen += OnCreateCitizen;

            Citizen.OnUpdateCitizenRenderDirection += OnUpdateCitizenRenderDirection;
            Citizen.OnUpdateCitizenRenderPosition += OnUpdateCitizenRenderPosition;
        }

		private void UpdateMapRenderData()
        {
            foreach (Cell cell in MapSystem.Instance.GetCells())
            {
                Vector3Int tilemapPosition = new Vector3Int(cell.Position.x, cell.Position.y, 0);

                _overlayTilemap.SetTile(tilemapPosition, _overlayTiles[cell.OverlayType]);
                _structureTilemap.SetTile(tilemapPosition, _structureTiles[cell.StructureType]);
                _groundTilemap.SetTile(tilemapPosition, _groundTiles[cell.GroundType]);
            }
        }

        private void OnCreateCitizen(object sender, OnCitizenEventArgs eventArgs)
        {
            RenderData renderData = new RenderData();

            _citizenRenderData[eventArgs.Citizen.Id] = renderData;

            Vector3 startPosition = GridToWorld(eventArgs.Citizen.Position);
            startPosition.z = eventArgs.Citizen.Id * Z_OFFSET;

            renderData.WorldGameObject = Instantiate(
                _nationPrefabs[eventArgs.Citizen.Nation], startPosition, Quaternion.identity
            );
            
            renderData.WorldGameObject.transform.parent = _citizenGameObject.transform;

            renderData.Animator = renderData.WorldGameObject.GetComponent<Animator>();

            PlayAnimation(eventArgs.Citizen, CitizenAnimationType.Idle);
        }

        private void OnUpdateCitizenRenderDirection(object sender, OnCitizenEventArgs eventArgs)
		{
            PlayAnimation(eventArgs.Citizen, CitizenAnimationType.Idle);
        }

        private void OnUpdateCitizenRenderPosition(object sender, OnCitizenEventArgs eventArgs)
		{
            StartCoroutine(MoveCitizen(eventArgs.Citizen));
        }

        private IEnumerator MoveCitizen(Citizen citizen)
		{
            RenderData renderData = _citizenRenderData[citizen.Id];

            float timer = 0;
            float duration = TimeSystem.TICK_DURATION * citizen.Cooldown;

            Vector3 startPosition = renderData.WorldGameObject.transform.position;

            Vector3 endPosition = GridToWorld(citizen.Position);
            endPosition.z = citizen.Id * Z_OFFSET;

            PlayAnimation(citizen, CitizenAnimationType.Walk);
            
            while (timer < duration)
			{
                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, timer / duration);
                
                renderData.WorldGameObject.transform.position = newPosition;
                
                timer += Time.deltaTime;

                yield return null;
			}

            renderData.WorldGameObject.transform.position = endPosition;
        }

        private Vector3 GridToWorld(int x, int y)
		{
            Vector3 worldPosition = _grid.CellToWorld(new Vector3Int(x, y, 0));
            worldPosition.y += 1 / 4f;

            return worldPosition;
        }

        private Vector3 GridToWorld(int2 position)
        {
            return GridToWorld(position.x, position.y);
        }

        private void PlayAnimation(Citizen citizen, CitizenAnimationType animationType)
		{
            RenderData renderData = _citizenRenderData[citizen.Id];

            renderData.Animator.Play(
                $"Base Layer.{citizen.Nation}-{animationType}-{citizen.Direction}"
            );
        }
    }
}
