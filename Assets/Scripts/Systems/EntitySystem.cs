using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class EntitySystem : MonoBehaviour
    {
        private GameObject citizensObject;

        private int numberOfCitizens;
        private List<Citizen> citizenList;

        private GameObject guyPrefab;
        private GameObject kailtPrefab;
        private GameObject taylorPrefab;
        
        private Dictionary<string, Direction> directions;
        private Dictionary<string, Nation> nationTypes;
        private Dictionary<string, Behavior> behaviorTypes;

        private WorldMap worldMap;

		private void Awake()
		{
            numberOfCitizens = 20;
            
            citizensObject = GameObject.Find("Citizens");
            citizenList = new List<Citizen>(numberOfCitizens);

            SetupEntityResources();
            SetupCitizenResources();
		}

        void SetupEntityResources()
		{
            directions = new Dictionary<string, Direction>();

            DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Map/Type/Direction");
            FileInfo[] fileInfoList = directoryInfo.GetFiles("*.asset");

            foreach (FileInfo fileInfo in fileInfoList)
			{
                string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

                directions[basename] = Resources.Load<Direction>($"Map/Type/Direction/{basename}");
            }

            behaviorTypes = new Dictionary<string, Behavior>();

            behaviorTypes["Simple Wander"] = Resources.Load<Wander>($"Entity/Type/Behavior/Simple Wander");
        }

        void SetupCitizenResources()
		{
            guyPrefab = Resources.Load<GameObject>("Prefabs/Guy");
            kailtPrefab = Resources.Load<GameObject>("Prefabs/Kailt");
            taylorPrefab = Resources.Load<GameObject>("Prefabs/Taylor");

            nationTypes = new Dictionary<string, Nation>
            {
                ["Guy"] = Resources.Load<Nation>("Entity/Type/Nation/Guy"),
                ["Kailt"] = Resources.Load<Nation>("Entity/Type/Nation/Kailt"),
                ["Taylor"] = Resources.Load<Nation>("Entity/Type/Nation/Taylor")
            };
        }

		void Start()
        {
            TimeSystem.OnTurn += OnTurn;

            worldMap = GameObject.Find("World Map").GetComponent<WorldMap>();

            Citizen testGuyCitizen = new Citizen();
            testGuyCitizen.Position = new int2(2, 2);
            testGuyCitizen.Direction = directions["SE"];
            testGuyCitizen.NationType = nationTypes["Guy"];

            testGuyCitizen.RenderData = new RenderData();
            testGuyCitizen.RenderData.WorldGameObject = Instantiate(
                guyPrefab,
                worldMap.GridToWorld(testGuyCitizen.Position),
                Quaternion.identity
            );
            testGuyCitizen.RenderData.Animator = testGuyCitizen.RenderData.WorldGameObject.GetComponent<Animator>();
            testGuyCitizen.RenderData.Animator.Play($"Base Layer.guys-idle-{testGuyCitizen.Direction.name.ToLower()}");

            AddCitizen(testGuyCitizen);

            Citizen testTaylorCitizen = new Citizen();
            testTaylorCitizen.Position = new int2(-2, -2);
            testTaylorCitizen.Direction = directions["EE"];
            testTaylorCitizen.NationType = nationTypes["Taylor"];

            testTaylorCitizen.RenderData = new RenderData();
            testTaylorCitizen.RenderData.WorldGameObject = Instantiate(
                taylorPrefab,
                worldMap.GridToWorld(testTaylorCitizen.Position),
                Quaternion.identity
            );
            testTaylorCitizen.RenderData.Animator = testTaylorCitizen.RenderData.WorldGameObject.GetComponent<Animator>();
            testTaylorCitizen.RenderData.Animator.Play($"Base Layer.taylor-idle-{testTaylorCitizen.Direction.name.ToLower()}");

            AddCitizen(testTaylorCitizen);

            /*           for (int i = 0; i < numberOfCitizens; i++)
                       {
                           int choice = UnityEngine.Random.Range(0, 3);
                           int2 randomPosition = new int2(
                               UnityEngine.Random.Range(-10, 11), 
                               UnityEngine.Random.Range(-10, 11)
                           );

                           if (choice == 0)
                           {
                               Citizen guyCitizen = ScriptableObject.CreateInstance<Citizen>();
                               guyCitizen.Position = randomPosition;
                               guyCitizen.NationType = nationTypes["Guy"];

                               guyCitizen.EntityRenderData = ScriptableObject.CreateInstance<EntityRenderData>();
                               guyCitizen.EntityRenderData.WorldGameObject = Instantiate(
                                   guyPrefab, 
                                   worldMap.GridToWorld(guyCitizen.Position), 
                                   Quaternion.identity
                               );

                               citizens.Add(guyCitizen);
                           }
                           else if (choice == 1)
                           {
                               Citizen kailtCitizen = ScriptableObject.CreateInstance<Citizen>();
                               kailtCitizen.Position = randomPosition;
                               kailtCitizen.NationType = nationTypes["Kailt"];

                               kailtCitizen.EntityRenderData = ScriptableObject.CreateInstance<EntityRenderData>();
                               kailtCitizen.EntityRenderData.WorldGameObject = Instantiate(
                                   kailtPrefab,
                                   worldMap.GridToWorld(kailtCitizen.Position),
                                   Quaternion.identity
                               );

                               citizens.Add(kailtCitizen);
                           }
                           else if (choice == 2)
                           {
                               Citizen taylorCitizen = ScriptableObject.CreateInstance<Citizen>();
                               taylorCitizen.Position = randomPosition;
                               taylorCitizen.NationType = nationTypes["Taylor"];

                               taylorCitizen.EntityRenderData = ScriptableObject.CreateInstance<EntityRenderData>();
                               taylorCitizen.EntityRenderData.WorldGameObject = Instantiate(
                                   taylorPrefab,
                                   worldMap.GridToWorld(taylorCitizen.Position),
                                   Quaternion.identity
                               );

                               citizens.Add(taylorCitizen);
                           }
                       }*/
        }

        void AddCitizen(Citizen citizen)
		{
            citizen.RenderData.WorldGameObject.transform.parent = citizensObject.transform;

            citizenList.Add(citizen);
        }

        void Update()
        {
        
        }

		private void OnDisable()
		{
            TimeSystem.OnTurn -= OnTurn;
		}

		private void UpdateRenderData()
		{
            foreach (Citizen citizen in citizenList)
			{
                citizen.RenderData = new RenderData();
                citizen.RenderData.WorldGameObject.transform.SetPositionAndRotation(
                    worldMap.GridToWorld(citizen.Position), 
                    Quaternion.identity
                );



                citizen.RenderData.Animator = citizen.RenderData.WorldGameObject.GetComponent<Animator>();
                citizen.RenderData.Animator.Play($"Base Layer.taylor-idle-{citizen.Direction.name.ToLower()}");

            }
        }

        private void OnTurn(object sender, TimeSystem.OnTurnEventArgs eventArgs)
        {
            Debug.Log("Turn " + eventArgs.turn);
        }
    }
}
