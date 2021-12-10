using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace MMH
{
	public class Citizen
	{
		private static int _nextCitizenId = 1;

		public static event EventHandler<OnCitizenEventArgs> OnUpdateCitizenRenderDirection;
		public static event EventHandler<OnCitizenEventArgs> OnUpdateCitizenRenderPosition;

		private readonly Dictionary<CitizenMovementStateType, CitizenMovementState> _movementStates;

		private CitizenMovementState _currentMovementState;

		public int Id { get; }
		public Direction Direction { get; set; }
		public int2 Position { get; set; }
		public Nation Nation { get; set; }
		public int Cooldown { get; set; }
		public CitizenAttributes Attributes { get; }

		public Citizen()
		{
			Id = _nextCitizenId++;

			Attributes = new CitizenAttributes
			{
				Strength = 1,
				Intelligence = 1,
				Speed = 1,
			};

			_movementStates = new Dictionary<CitizenMovementStateType, CitizenMovementState>
			{
				[CitizenMovementStateType.Idle] = new CitizenIdle(this),
				[CitizenMovementStateType.Wander] = new CitizenWander(this)
			};

			_currentMovementState = _movementStates[CitizenMovementStateType.Idle];
		}
		public void Tick()
		{
			Cooldown--;

			_currentMovementState.Tick();
		}

		public bool CanAct()
		{
			return Cooldown <= 0;
		}

		public void UpdateRenderDirection()
		{
			OnUpdateCitizenRenderDirection?.Invoke(this, new OnCitizenEventArgs { Citizen = this });
		}

		public void UpdateRenderPosition()
		{
			OnUpdateCitizenRenderPosition?.Invoke(this, new OnCitizenEventArgs { Citizen = this });
		}

		public void SetMovementState(CitizenMovementStateType citizenMovementStateType)
		{
			_currentMovementState = _movementStates[citizenMovementStateType];
		}
	}
}
