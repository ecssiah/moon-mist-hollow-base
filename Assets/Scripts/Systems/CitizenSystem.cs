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

        private GameSettings gameSettings;
        
        public static event EventHandler<OnCreateCitizenArgs> OnCreateCitizen;

        private List<Citizen> citizenList;
		public List<Citizen> CitizenList { get => citizenList; }

        public class OnCreateCitizenArgs : EventArgs
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

            gameSettings = Resources.Load<GameSettings>("ScriptableObjects/Game Settings");

            citizenList = new List<Citizen>(gameSettings.NumberOfCitizens);

            UISystem.OnUpdateRulesDropdown += OnUpdateRulesDropdown;

        }
		
        void Start()
        {
            CreateCitizens();
        }

        void CreateCitizens()
        {
            for (int i = 0; i < gameSettings.NumberOfCitizens; i++)
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

                OnCreateCitizen?.Invoke(this, new OnCreateCitizenArgs { Citizen = newCitizen });
            }
        }
        
        public void OnUpdateRulesDropdown(object sender, UISystem.OnUpdateRulesDropownArgs eventArgs)
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
