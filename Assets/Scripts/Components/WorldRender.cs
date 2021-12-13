using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace MMH
{
    public class WorldRender : MonoBehaviour
    {
		private Grid _grid;

        private Tilemap _overlayTilemap;
        private Tilemap _structureTilemap;
        private Tilemap _groundTilemap;

        private Dictionary<OverlayType, Tile> _overlayTiles;
        private Dictionary<StructureType, Tile> _structureTiles;
        private Dictionary<GroundType, Tile> _groundTiles;
		
        private GameObject _citizenGameObject;

        private Dictionary<int, CitizenRenderData> _citizenRenderData;

        private Dictionary<Nation, GameObject> _nationPrefabs;

        void Awake()
	    {
            SetupEvents();

            SetupTilemapResources();
            SetupCitizenResources();
        }
        
        private void SetupEvents()
        {
            MapSystem.OnUpdateMapRender += UpdateMapRender;
            EntitySystem.OnCreateCitizen += CreateCitizenRenderData;

            Citizen.OnUpdateCitizenRenderDirection += UpdateCitizenRenderDirection;
            Citizen.OnUpdateCitizenRenderPosition += UpdateCitizenRenderPosition;
        }

        private void SetupTilemapResources()
		{
            _grid = GameObject.Find("Grid").GetComponent<Grid>();

            _overlayTilemap = GameObject.Find("Overlay").GetComponent<Tilemap>();
            _structureTilemap = GameObject.Find("Structures").GetComponent<Tilemap>();
            _groundTilemap = GameObject.Find("Ground").GetComponent<Tilemap>();

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

        private void SetupCitizenResources()
		{
            _citizenGameObject = GameObject.Find("World/Citizens");

            _citizenRenderData = new Dictionary<int, CitizenRenderData>();

            _nationPrefabs = new Dictionary<Nation, GameObject>
            {
                [Nation.Guys] = Resources.Load<GameObject>("Prefabs/Guys"),
                [Nation.Kailt] = Resources.Load<GameObject>("Prefabs/Kailt"),
                [Nation.Taylor] = Resources.Load<GameObject>("Prefabs/Taylor"),
            };

            Animator guysAnimator = _nationPrefabs[Nation.Guys].GetComponent<Animator>();

            foreach (AnimationClip clip in guysAnimator.runtimeAnimatorController.animationClips)
            {
                clip.frameRate = 16;
            }

            Animator kailtAnimator = _nationPrefabs[Nation.Kailt].GetComponent<Animator>();

            foreach (AnimationClip clip in kailtAnimator.runtimeAnimatorController.animationClips)
            {
                clip.frameRate = 16;
            }

            Animator taylorAnimator = _nationPrefabs[Nation.Taylor].GetComponent<Animator>();

            foreach (AnimationClip clip in taylorAnimator.runtimeAnimatorController.animationClips)
            {
                clip.frameRate = 16;
            }
        }

        private void OnDisable()
        {
            MapSystem.OnUpdateMapRender -= UpdateMapRender;
            EntitySystem.OnCreateCitizen -= CreateCitizenRenderData;

            Citizen.OnUpdateCitizenRenderDirection -= UpdateCitizenRenderDirection;
            Citizen.OnUpdateCitizenRenderPosition -= UpdateCitizenRenderPosition;
        }

		private void UpdateMapRender(object sender, OnMapEventArgs eventArgs)
        {
            foreach (Cell cell in eventArgs.WorldMap.Cells)
            {
                Vector3Int tilemapPosition = new Vector3Int(cell.Position.x, cell.Position.y, 0);

                _overlayTilemap.SetTile(tilemapPosition, _overlayTiles[cell.OverlayType]);
                _structureTilemap.SetTile(tilemapPosition, _structureTiles[cell.StructureType]);
                _groundTilemap.SetTile(tilemapPosition, _groundTiles[cell.GroundType]);
            }
        }
		
        private void CreateCitizenRenderData(object sender, OnCitizenEventArgs eventArgs)
        {
            Citizen citizen = eventArgs.Citizen;
            CitizenRenderData citizenRenderData = new CitizenRenderData();

            Vector3 startPosition = GridToWorld(citizen.Position);
            startPosition.z = citizen.Id * RenderInfo.CitizenZSpacing;

            citizenRenderData.WorldGameObject = Instantiate(
                _nationPrefabs[citizen.Nation], 
                startPosition, 
                Quaternion.identity, 
                _citizenGameObject.transform
            );
            
            citizenRenderData.Animator = citizenRenderData.WorldGameObject.GetComponent<Animator>();

            _citizenRenderData[citizen.Id] = citizenRenderData;

            PlayAnimation(citizen, CitizenAnimationType.Idle);
        }

        private void UpdateCitizenRenderDirection(object sender, OnCitizenEventArgs eventArgs)
		{
            PlayAnimation(eventArgs.Citizen, CitizenAnimationType.Idle);
        }

        private void UpdateCitizenRenderPosition(object sender, OnCitizenEventArgs eventArgs)
		{
            StartCoroutine(MoveCitizen(eventArgs.Citizen));
        }

		private IEnumerator MoveCitizen(Citizen citizen)
		{
            float timer = 0;
            float duration = citizen.Cooldown * SimulationManager.Instance.SimulationSettings.TickDuration;

            CitizenRenderData citizenRenderData = _citizenRenderData[citizen.Id];

            Vector3 startPosition = citizenRenderData.WorldGameObject.transform.position;

            Vector3 endPosition = GridToWorld(citizen.Position);
            endPosition.z = citizen.Id * RenderInfo.CitizenZSpacing;

            PlayAnimation(citizen, CitizenAnimationType.Walk);
            
            while (timer < duration)
			{
                timer += Time.deltaTime;

                Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, timer / duration);
                
                citizenRenderData.WorldGameObject.transform.position = newPosition;

                yield return null;
			}

            citizenRenderData.WorldGameObject.transform.position = endPosition;
        }

        private void PlayAnimation(Citizen citizen, CitizenAnimationType animationType)
		{
            CitizenRenderData citizenRenderData = _citizenRenderData[citizen.Id];

            citizenRenderData.Animator.Play($"Base Layer.{citizen.Nation}-{animationType}-{citizen.Direction}");
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
    }
}
