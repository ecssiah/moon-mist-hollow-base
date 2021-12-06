using System;
using System.Collections.Generic;

namespace MMH
{
    public class EntitySystem : GameSystem<EntitySystem>
    {
        public static event EventHandler<OnCreateCitizenArgs> OnCreateCitizen;

        private List<Citizen> citizenList;
		public List<Citizen> CitizenList { get => citizenList; }

        public class OnCreateCitizenArgs : EventArgs
        {
            public Citizen Citizen;
        }

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
            citizenList = new List<Citizen>();

			for (int i = 0; i < ManagerSystem.Settings.NumberOfCitizens; i++)
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
                citizen.SetMovementState(eventArgs.StateType);
            }
		}
    }
}
