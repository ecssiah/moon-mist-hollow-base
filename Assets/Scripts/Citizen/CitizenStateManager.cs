using System;
using System.Collections.Generic;

namespace MMH
{
	public class CitizenStateManager
    {
		public static Dictionary<CitizenStateType, CitizenState> states;

		private Citizen citizen;
		private WorldMap worldMap;
        private CitizenState currentState;

        public CitizenStateManager(Citizen citizen, WorldMap worldMap)
		{
			this.citizen = citizen;
			this.worldMap = worldMap;

            TimeSystem.OnTick += OnTick;

			states = new Dictionary<CitizenStateType, CitizenState>
			{
				[CitizenStateType.CitizenIdle] = new CitizenIdle(),
				[CitizenStateType.CitizenWander] = new CitizenWander()
			};

			currentState = states[CitizenStateType.CitizenIdle];
		}

		public void SetState(CitizenStateType citizenStateType)
		{
			currentState = states[citizenStateType];
		}

		private void OnTick(object sender, TimeSystem.OnTickEventArgs eventArgs)
		{
			currentState.OnTick();
		}
	}
}
