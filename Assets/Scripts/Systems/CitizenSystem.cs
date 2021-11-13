using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class CitizenSystem : MonoBehaviour
    {
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

            
        }
		
        void Start()
        {
            CreateCitizens();
        }

        void CreateCitizens()
        {
            Citizen testGuysCitizen = new Citizen(mapSystem)
            {
                Position = new int2(2, 2),
                Direction = Direction.EE,
                Nation = Nation.Guys,
                Attributes = new CitizenAttributes
                {
                    Strength = 1,
                    Intelligence = 1,
                    Speed = 1,
				}
            };

            OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { citizen = testGuysCitizen });

            citizenList.Add(testGuysCitizen);

            Citizen testTaylorCitizen = new Citizen(mapSystem)
            {
                Position = new int2(-2, -2),
                Direction = Direction.SE,
                Nation = Nation.Taylor,
                Attributes = new CitizenAttributes
                {
                    Strength = 1,
                    Intelligence = 1,
                    Speed = 1,
                }
            };

            OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { citizen = testTaylorCitizen });

            citizenList.Add(testTaylorCitizen);
        }

    }
}
