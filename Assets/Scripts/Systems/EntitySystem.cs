using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class EntitySystem : MonoBehaviour
    {
        private RenderSystem renderSystem;

        private GameObject citizensObject;

        private int numberOfCitizens;
        private List<Citizen> citizenList;

        private GameObject guyPrefab;
        private GameObject kailtPrefab;
        private GameObject taylorPrefab;
        
        private Dictionary<string, Direction> directions;
        private Dictionary<string, Nation> nationTypes;
        private Dictionary<string, Behavior> behaviorTypes;

		private void Awake()
		{
            numberOfCitizens = 20;
            
            citizensObject = GameObject.Find("Citizens");
            citizenList = new List<Citizen>(numberOfCitizens);

            SetupMapResources();
            SetupEntityResources();

            CreateEntities();
		}

        void SetupMapResources()
		{
            directions = new Dictionary<string, Direction>();

            DirectoryInfo directoryInfo = new DirectoryInfo("Assets/Resources/Map/Type/Direction");
            FileInfo[] fileInfoList = directoryInfo.GetFiles("*.asset");

            foreach (FileInfo fileInfo in fileInfoList)
            {
                string basename = Path.GetFileNameWithoutExtension(fileInfo.Name);

                directions[basename] = Resources.Load<Direction>($"Map/Type/Direction/{basename}");
            }
        }

        void SetupEntityResources()
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

            behaviorTypes = new Dictionary<string, Behavior>();

            behaviorTypes["Simple Wander"] = Resources.Load<Wander>($"Entity/Type/Behavior/Simple Wander");
        }

        void CreateEntities()
		{
            Citizen testGuyCitizen = new Citizen();
            testGuyCitizen.Position = new int2(2, 2);
            testGuyCitizen.Direction = directions["SE"];
            testGuyCitizen.NationType = nationTypes["Guy"];

            citizenList.Add(testGuyCitizen);

            Citizen testTaylorCitizen = new Citizen();
            testTaylorCitizen.Position = new int2(-2, -2);
            testTaylorCitizen.Direction = directions["EE"];
            testTaylorCitizen.NationType = nationTypes["Taylor"];

            citizenList.Add(testTaylorCitizen);
        }

        void Start()
        {
            TimeSystem.OnTurn += OnTurn;
        }

        void Update()
        {
        
        }

		private void OnDisable()
		{
            TimeSystem.OnTurn -= OnTurn;
		}

        private void OnTurn(object sender, TimeSystem.OnTurnEventArgs eventArgs)
        {
            Debug.Log("Turn " + eventArgs.turn);
        }
    }
}
