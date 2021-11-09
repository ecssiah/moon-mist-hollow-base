using System;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class EntitySystem : MonoBehaviour
    {
        private MapSystem mapSystem;

        private int numberOfCitizens;

        private List<Citizen> citizenList;
		public List<Citizen> CitizenList { get => citizenList; }

        public class OnCreateCitizenEventArgs : EventArgs
        {
            public Citizen citizen;
        }

        public static event EventHandler<OnCreateCitizenEventArgs> OnCreateCitizen;

        private void Awake()
		{
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            numberOfCitizens = 20;
            
            citizenList = new List<Citizen>(numberOfCitizens);

            TimeSystem.OnTurn += OnTurn;
        }

        void CreateEntities()
		{
			Citizen testGuysCitizen = new Citizen
			{
				Position = new int2(2, 2),
				Direction = Direction.EE,
				Nation = Nation.Guys,
                WorldMap = mapSystem.GetWorldMap(),
			};

			OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { citizen = testGuysCitizen });

            citizenList.Add(testGuysCitizen);

			Citizen testTaylorCitizen = new Citizen
			{
				Position = new int2(-2, -2),
				Direction = Direction.SE,
				Nation = Nation.Taylor,
			};

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
        }
    }
}
