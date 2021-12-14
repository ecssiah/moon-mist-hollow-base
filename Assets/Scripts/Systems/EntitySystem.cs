using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{

	public class EntitySystem : SimulationSystem
    {
        public static event EventHandler<OnCitizenEventArgs> OnCreateCitizen;

        private List<Citizen> _citizenList;

		public override void Init()
		{
            SetupEvents();
            
            CreateCitizens();
        }

        private void SetupEvents()
		{
            SimulationManager.OnTick += Tick;
            Interface.OnUpdateMovementState += UpdateMovementState;
		}

        private void CreateCitizens()
        {
            _citizenList = new List<Citizen>();

			for (int i = 0; i < EntityInfo.TotalCitizens; i++)
			{
				var newCitizen = new Citizen()
				{
                    Nation = Utils.RandomEnumValue<Nation>(),
                    Direction = Utils.RandomEnumValue<Direction>(),
					Position = SimulationManager.Instance.MapSystem.GetOpenCellPosition()
				};

				_citizenList.Add(newCitizen);

				OnCreateCitizen?.Invoke(this, new OnCitizenEventArgs { Citizen = newCitizen });
			}
		}

        protected override void Tick(object sender, OnTickArgs eventArgs)
        {
            foreach (Citizen citizen in _citizenList)
            {
                citizen.Tick();
            }
        }

        public override void Quit()
        {
            SimulationManager.OnTick -= Tick;
            Interface.OnUpdateMovementState -= UpdateMovementState;
        }
        
        private void UpdateMovementState(object sender, OnUpdateMovementStateArgs eventArgs)
		{
            foreach (Citizen citizen in _citizenList)
			{
                citizen.SetMovementState(eventArgs.CitizenMovementStateType);
            }
		}
    }
}
