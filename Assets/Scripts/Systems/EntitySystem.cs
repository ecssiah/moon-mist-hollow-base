using System;
using System.Collections.Generic;

namespace MMH
{
	public class EntitySystem : GameSystem
    {
        public static event EventHandler<OnCitizenEventArgs> OnCreateCitizen;

        private List<Citizen> _citizenList;

		public override void Init()
		{
            UserInterface.OnUpdateMovementState += UpdateMovementState;
        
            CreateCitizens();
        }

        void CreateCitizens()
        {
            _citizenList = new List<Citizen>();

			for (int i = 0; i < GameManager.Instance.SimulationSettings.NumberOfCitizens; i++)
			{
				Citizen newCitizen = new Citizen()
				{
                    Nation = Utils.RandomEnumValue<Nation>(),
                    Direction = Utils.RandomEnumValue<Direction>(),
					Position = GameManager.Instance.MapSystem.GetOpenCellPosition()
				};

				_citizenList.Add(newCitizen);

				OnCreateCitizen?.Invoke(this, new OnCitizenEventArgs { Citizen = newCitizen });
			}
		}
        
        public void UpdateMovementState(object sender, OnUpdateMovementStateArgs eventArgs)
		{
            foreach (Citizen citizen in _citizenList)
			{
                citizen.SetMovementState(eventArgs.StateType);
            }
		}
    }
}
