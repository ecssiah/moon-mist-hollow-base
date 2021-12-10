using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class UserInterface : MonoBehaviour
    {
        public static event EventHandler<OnUpdateRulesDropownArgs> OnUpdateRulesDropdown;
        
        public class OnUpdateRulesDropownArgs
        {
            public CitizenMovementStateType StateType;
        }

        private TMPro.TMP_Dropdown _rulesDropdown;

        void Awake()
	    {
            _rulesDropdown = GameObject.Find("Rules Dropdown").GetComponent<TMPro.TMP_Dropdown>();

            List<string> rules = new List<string>
            {
                CitizenMovementStateType.Idle.ToString(),
                CitizenMovementStateType.Wander.ToString(),
            };

            _rulesDropdown.AddOptions(rules);

            _rulesDropdown.onValueChanged.AddListener(
                delegate { OnRuleChange(); }
            );
        }

        private void OnRuleChange()
		{
            OnUpdateRulesDropownArgs eventArgs = new OnUpdateRulesDropownArgs
            {
                StateType = (CitizenMovementStateType)_rulesDropdown.value
            };

            OnUpdateRulesDropdown?.Invoke(this, eventArgs);
		}
    }
}

