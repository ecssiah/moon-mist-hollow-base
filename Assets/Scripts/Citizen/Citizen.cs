using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH
{
    public class Citizen
    {
		private static int nextID = 0;

		private MapSystem mapSystem;

		private Dictionary<CitizenStateType, CitizenState> states;

		private CitizenState currentState;

		public int Id;
		public int2 Position;
		public Direction Direction;

		public Nation Nation;

		public CitizenAttributes Attributes;

        public Citizen(MapSystem mapSystem)
		{
			TimeSystem.OnTick += OnTick;

			Id = nextID++;

			states = new Dictionary<CitizenStateType, CitizenState>
			{
				[CitizenStateType.CitizenIdle] = new CitizenIdle(mapSystem),
				[CitizenStateType.CitizenWander] = new CitizenWander(mapSystem)
			};

			currentState = states[CitizenStateType.CitizenWander];
		}

		public void SetState(CitizenStateType citizenStateType)
		{
			currentState = states[citizenStateType];
		}

		private void OnTick(object sender, TimeSystem.OnTickEventArgs eventArgs)
		{
			currentState.Tick(this);
		}
	}
}
