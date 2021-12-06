using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
	public partial class EntitySystem : GameSystem<EntitySystem>
    {
        public static event EventHandler<OnCitizenEventArgs> OnCreateCitizen;

        private List<Citizen> _citizenList;

        protected override void Awake()
		{
            base.Awake();

            UISystem.OnUpdateRulesDropdown += OnUpdateRulesDropdown;
        }
		
        void Start()
        {
            CreateCitizens();
        }

        void CreateCitizens()
        {
            _citizenList = new List<Citizen>();

			for (int i = 0; i < ManagerSystem.Settings.NumberOfCitizens; i++)
			{
				Citizen newCitizen = new Citizen()
				{
                    Nation = Utils.RandomEnumValue<Nation>(),
                    Direction = Utils.RandomEnumValue<Direction>(),
					Position = MapSystem.Instance.GetRandomCell()
				};

				_citizenList.Add(newCitizen);

				OnCreateCitizen?.Invoke(this, new OnCitizenEventArgs { Citizen = newCitizen });
			}
		}
        
        public void OnUpdateRulesDropdown(object sender, UISystem.OnUpdateRulesDropownArgs eventArgs)
		{
            foreach (Citizen citizen in _citizenList)
			{
                citizen.SetMovementState(eventArgs.StateType);
            }
		}
    }
}
