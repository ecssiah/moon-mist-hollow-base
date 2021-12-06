using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace MMH
{
    public class Citizen
    {
		public static event EventHandler<OnCitizenEventArgs> OnUpdateCitizenDirection;
		public static event EventHandler<OnCitizenEventArgs> OnUpdateCitizenPosition;

		private Dictionary<CitizenMovementStateType, CitizenMovementState> movementStates;

		public int Id;
		public int2 Position;
		public Direction Direction;
		public Nation Nation;
		public CitizenAttributes Attributes;
		
		private CitizenMovementState currentMovementState;

		public int cooldown;

		public Citizen()
		{
			TimeSystem.OnTick += OnTick;

			movementStates = new Dictionary<CitizenMovementStateType, CitizenMovementState>
			{
				[CitizenMovementStateType.Idle] = new CitizenIdle(this),
				[CitizenMovementStateType.Wander] = new CitizenWander(this)
			};

			currentMovementState = movementStates[CitizenMovementStateType.Idle];
		}

		public CitizenMovementStateType GetCitizenMovementStateType()
		{
			return currentMovementState.CitizenMovementStateType;
		}

		public bool CanAct()
		{
			return cooldown <= 0;
		}

		public int GetCooldown()
		{
			return cooldown;
		}

		public void SetCooldown(int ticks)
		{
			cooldown = ticks;
		}

		public void SetDirection(Direction direction)
		{
			Direction = direction;

			OnUpdateCitizenDirection?.Invoke(this, new OnCitizenEventArgs { Citizen = this });
		}

		public void SetPosition(int2 position)
		{
			Position = position;

			OnUpdateCitizenPosition?.Invoke(this, new OnCitizenEventArgs { Citizen = this });
		}

		public void SetMovementState(CitizenMovementStateType citizenMovementStateType)
		{
			currentMovementState = movementStates[citizenMovementStateType];
		}

		private void OnTick(object sender, TimeSystem.OnTickArgs eventArgs)
		{
			cooldown--;

			currentMovementState.Tick();
		}
	}
}
