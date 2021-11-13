using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class CitizenSystem : MonoBehaviour
    {
        
        public static Dictionary<CitizenStateType, CitizenState> States;
        
        public static event EventHandler<OnCreateCitizenEventArgs> OnCreateCitizen;

        private MapSystem mapSystem;

        private int numberOfCitizens;

        private List<Citizen> citizenList;
		public List<Citizen> CitizenList { get => citizenList; }

        public class OnCreateCitizenEventArgs : EventArgs
        {
            public Citizen citizen;
        }

        private void Awake()
		{
            mapSystem = GameObject.Find("MapSystem").GetComponent<MapSystem>();

            numberOfCitizens = 20;
            
            citizenList = new List<Citizen>(numberOfCitizens);

            States = new Dictionary<CitizenStateType, CitizenState>
            {
                [CitizenStateType.CitizenIdle] = new CitizenIdle(mapSystem),
                [CitizenStateType.CitizenWander] = new CitizenWander(mapSystem)
            };
        }
		
        void Start()
        {
            CreateCitizens();
        }

        void CreateCitizens()
        {
            Citizen testGuysCitizen = new Citizen
            {
                Position = new int2(2, 2),
                Direction = Direction.EE,
                Nation = Nation.Guys,
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

    }
}
