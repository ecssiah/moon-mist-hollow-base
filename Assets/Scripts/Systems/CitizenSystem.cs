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

            numberOfCitizens = 1000;
            
            citizenList = new List<Citizen>(numberOfCitizens);

            UISystem.OnUpdateRulesDropdown += OnUpdateRulesDropdown;
        }
		
        void Start()
        {
            CreateCitizens();
        }

        void CreateCitizens()
        {
            for (int i = 0; i < numberOfCitizens; i++)
			{
                Citizen newCitizen = new Citizen()
                {
                    Position = MapSystem.Instance.GetRandomCell(),
                    Direction = Utils.RandomEnumValue<Direction>(),
                    Nation = Utils.RandomEnumValue<Nation>(),
                    Attributes = new CitizenAttributes
                    {
                        Strength = 1,
                        Intelligence = 1,
                        Speed = 1,
                    }
                };

                citizenList.Add(newCitizen);

                OnCreateCitizen?.Invoke(this, new OnCreateCitizenEventArgs { Citizen = newCitizen });
            }
        }
        
        public void OnUpdateRulesDropdown(object sender, UISystem.OnUpdateRulesDropownEventArgs eventArgs)
		{
            foreach (Citizen citizen in citizenList)
			{
                if (eventArgs.RuleName == "CitizenIdle") 
                { 
                    citizen.SetState(CitizenStateType.CitizenIdle);
                } 
                else if (eventArgs.RuleName == "CitizenWander")
				{
                    citizen.SetState(CitizenStateType.CitizenWander);
				}
            }
		}
    }
}
