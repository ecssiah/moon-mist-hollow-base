using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class CitizenSystem : MonoBehaviour
    {
        public static event EventHandler<OnCreateCitizenEventArgs> OnCreateCitizen;
        public static event EventHandler<OnUpdateCitizenEventArgs> OnUpdateCitizen;

        private MapSystem mapSystem;

        private int numberOfCitizens;

        private List<Citizen> citizenList;
		public List<Citizen> CitizenList { get => citizenList; }

        public class OnCreateCitizenEventArgs : EventArgs
        {
            public Citizen citizen;
        }

        public class OnUpdateCitizenEventArgs : EventArgs
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
            Citizen testGuysCitizen = new Citizen(this, mapSystem)
            {
                Position = new int2(0, 2),
                Direction = Direction.SS,
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

            Citizen testTaylorCitizen = new Citizen(this, mapSystem)
            {
                Position = new int2(2, 0),
                Direction = Direction.WW,
                Nation = Nation.Taylor,
                Attributes = new CitizenAttributes
                {
                    Strength = 1,
                    Intelligence = 1,
                    Speed = 3,
                }
            };

            OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { citizen = testTaylorCitizen });

            citizenList.Add(testTaylorCitizen);
        }

        public void UpdateCitizen(Citizen citizen)
		{
            OnUpdateCitizen?.Invoke(this, new OnUpdateCitizenEventArgs { citizen = citizen });
		}
    }
}
