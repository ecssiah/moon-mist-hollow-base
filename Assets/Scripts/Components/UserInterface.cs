using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace MMH
{
	public class UserInterface : MonoBehaviour
    {
        public static event EventHandler<OnUpdateMovementStateArgs> OnUpdateMovementState;

        private UnityAction<int> _ruleChangeAction;

        private TMP_Dropdown _rulesDropdown;

        void Awake()
	    {
            _ruleChangeAction += OnRuleChange;
            
            List<string> rulesOptions = new List<string>
            {
                CitizenMovementStateType.Idle.ToString(),
                CitizenMovementStateType.Wander.ToString(),
            };

            _rulesDropdown = GameObject.Find("Rules Dropdown").GetComponent<TMP_Dropdown>();
            
            _rulesDropdown.AddOptions(rulesOptions);
            _rulesDropdown.onValueChanged.AddListener(_ruleChangeAction);
        }

        private void OnRuleChange(int value)
		{
            OnUpdateMovementStateArgs eventArgs = new OnUpdateMovementStateArgs
            {
                StateType = (CitizenMovementStateType)value
            };

            OnUpdateMovementState?.Invoke(this, eventArgs);
		}
    }
}

