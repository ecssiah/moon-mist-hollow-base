using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class UISystem : GameSystem<UISystem>
    {
        public static event EventHandler<OnUpdateRulesDropownArgs> OnUpdateRulesDropdown;
        
        public class OnUpdateRulesDropownArgs
        {
            public CitizenMovementStateType StateType;
        }

        private TMPro.TMP_Dropdown rulesDropdown;

        protected override void Awake()
	    {
            base.Awake();

            rulesDropdown = GameObject.Find("Rules Dropdown").GetComponent<TMPro.TMP_Dropdown>();

            List<string> rules = new List<string>
            {
                CitizenMovementStateType.Idle.ToString(),
                CitizenMovementStateType.Wander.ToString(),
            };

            rulesDropdown.AddOptions(rules);

            rulesDropdown.onValueChanged.AddListener(
                delegate { OnRuleChange(); }
            );
        }

        private void OnRuleChange()
		{
            OnUpdateRulesDropownArgs eventArgs = new OnUpdateRulesDropownArgs
            {
                StateType = (CitizenMovementStateType)rulesDropdown.value
            };

            OnUpdateRulesDropdown?.Invoke(this, eventArgs);
		}
    }
}

