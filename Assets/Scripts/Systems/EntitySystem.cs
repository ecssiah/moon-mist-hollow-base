using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class EntitySystem : MonoBehaviour
    {
        private int numberOfCitizens;
        private List<Citizen> citizenList;

        private Dictionary<string, Direction> directions;
        private Dictionary<string, NationType> nationTypes;
        private Dictionary<string, Behavior> behaviorTypes;

		public List<Citizen> CitizenList { get => citizenList; }

        public class OnCreateCitizenEventArgs : EventArgs
        {
            public Citizen citizen;
        }

        public static event EventHandler<OnCreateCitizenEventArgs> OnCreateCitizen;

        private void Awake()
		{
            numberOfCitizens = 20;
            
            citizenList = new List<Citizen>(numberOfCitizens);

            SetupMapResources();
            SetupEntityResources();

            TimeSystem.OnTurn += OnTurn;
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
            nationTypes = new Dictionary<string, NationType>
            {
                ["Guys"] = Resources.Load<NationType>("Entity/Type/Nation/Guys"),
                ["Kailt"] = Resources.Load<NationType>("Entity/Type/Nation/Kailt"),
                ["Taylor"] = Resources.Load<NationType>("Entity/Type/Nation/Taylor")
            };

            behaviorTypes = new Dictionary<string, Behavior>();

            behaviorTypes["Simple Wander"] = Resources.Load<Wander>($"Entity/Type/Behavior/Simple Wander");
        }

        void CreateEntities()
		{
            Citizen testGuysCitizen = new Citizen();
            testGuysCitizen.Position = new int2(2, 2);
            testGuysCitizen.Direction = directions["SE"];
            testGuysCitizen.NationType = nationTypes["Guys"];

            OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { citizen = testGuysCitizen });

            citizenList.Add(testGuysCitizen);

            Citizen testTaylorCitizen = new Citizen();
            testTaylorCitizen.Position = new int2(-2, -2);
            testTaylorCitizen.Direction = directions["EE"];
            testTaylorCitizen.NationType = nationTypes["Taylor"];

            OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { citizen = testTaylorCitizen });

            citizenList.Add(testTaylorCitizen);
        }

        void Start()
        {
            CreateEntities();
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
