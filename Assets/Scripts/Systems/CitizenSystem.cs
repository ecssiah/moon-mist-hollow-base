using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class CitizenSystem : MonoBehaviour
    {
        private static CitizenSystem _instance;
        public static CitizenSystem Instance { get { return _instance; } }
        
        public static event EventHandler<OnCreateCitizenEventArgs> OnCreateCitizen;

        private int numberOfCitizens;

        private List<Citizen> citizenList;
		public List<Citizen> CitizenList { get => citizenList; }

        public class OnCreateCitizenEventArgs : EventArgs
        {
            public Citizen Citizen;
        }

        private void Awake()
		{
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            numberOfCitizens = 20;
            
            citizenList = new List<Citizen>(numberOfCitizens);
        }
		
        void Start()
        {
            CreateCitizens();
        }

        void CreateCitizens()
        {
            Citizen testGuysCitizen = new Citizen()
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
            
            citizenList.Add(testGuysCitizen);

            OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { Citizen = testGuysCitizen });

            Citizen testTaylorCitizen = new Citizen()
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
            
            citizenList.Add(testTaylorCitizen);

            OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { Citizen = testTaylorCitizen });
        }
    }
}
