using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class EntityManager : MonoBehaviour
    {
        private GameObject citizensObject;

        private int numberOfCitizens;
        private List<Citizen> citizens;

        private GameObject guyPrefab;
        private GameObject kailtPrefab;
        private GameObject taylorPrefab;

        private Dictionary<string, NationType> nationTypes;

        private WorldMap worldMap;

		private void Awake()
		{
            citizensObject = GameObject.Find("Citizens");

            numberOfCitizens = 20;
            citizens = new List<Citizen>(numberOfCitizens);

            SetupCitizenResources();
		}

        void SetupCitizenResources()
		{
            guyPrefab = Resources.Load<GameObject>("Prefabs/Guy");
            kailtPrefab = Resources.Load<GameObject>("Prefabs/Kailt");
            taylorPrefab = Resources.Load<GameObject>("Prefabs/Taylor");

            nationTypes = new Dictionary<string, NationType>
            {
                ["Guy"] = Resources.Load<NationType>("Types/EntityTypes/Guy"),
                ["Kailt"] = Resources.Load<NationType>("Types/EntityTypes/Kailt"),
                ["Taylor"] = Resources.Load<NationType>("Types/EntityTypes/Taylor")
            };
        }

		void Start()
        {
            worldMap = GameObject.Find("World Map").GetComponent<WorldMap>();

            Citizen testGuyCitizen = ScriptableObject.CreateInstance<Citizen>();
            testGuyCitizen.Position = new int2(2, 2);
            testGuyCitizen.NationType = nationTypes["Guy"];

            testGuyCitizen.EntityRenderData = ScriptableObject.CreateInstance<EntityRenderData>();
            testGuyCitizen.EntityRenderData.WorldGameObject = Instantiate(
                guyPrefab,
                worldMap.GridToWorld(testGuyCitizen.Position),
                Quaternion.identity
            );

            citizens.Add(testGuyCitizen);

            Citizen testTaylorCitizen = ScriptableObject.CreateInstance<Citizen>();
            testTaylorCitizen.Position = new int2(-2, -2);
            testTaylorCitizen.NationType = nationTypes["Taylor"];

            testTaylorCitizen.EntityRenderData = ScriptableObject.CreateInstance<EntityRenderData>();
            testTaylorCitizen.EntityRenderData.WorldGameObject = Instantiate(
                taylorPrefab,
                worldMap.GridToWorld(testTaylorCitizen.Position),
                Quaternion.identity
            );

            citizens.Add(testTaylorCitizen);

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

        void Update()
        {
        
        }
        
        private void UpdateRenderData()
		{

		}
    }
}
