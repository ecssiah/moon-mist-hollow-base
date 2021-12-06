using System;
using System.Collections.Generic;

namespace MMH
{
	public partial class EntitySystem : GameSystem<EntitySystem>
    {
        public static event EventHandler<OnCitizenEventArgs> OnCreateCitizen;

        private int nextCitizenId;

        private List<Citizen> citizenList;
		public List<Citizen> CitizenList { get => citizenList; }

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
            nextCitizenId = 1;

            citizenList = new List<Citizen>();

			for (int i = 0; i < ManagerSystem.Settings.NumberOfCitizens; i++)
			{
                Citizen newCitizen = new Citizen()
                {
                    Id = nextCitizenId++,
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

				OnCreateCitizen?.Invoke(this, new OnCitizenEventArgs { Citizen = newCitizen });
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
