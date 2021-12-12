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
            _rulesDropdown = GameObject.Find("Movement Rules/Dropdown").GetComponent<TMP_Dropdown>();

            var rulesOptions = new List<string>
            {
                CitizenMovementStateType.Idle.ToString(),
                CitizenMovementStateType.Wander.ToString(),
            };

            _ruleChangeAction += OnRuleChange;

            _rulesDropdown.AddOptions(rulesOptions);
            _rulesDropdown.onValueChanged.AddListener(_ruleChangeAction);
        }

        private void OnRuleChange(int value)
		{
            var eventArgs = new OnUpdateMovementStateArgs
            {
                CitizenMovementStateType = (CitizenMovementStateType)value
            };

            OnUpdateMovementState?.Invoke(this, eventArgs);
		}
    }
}

