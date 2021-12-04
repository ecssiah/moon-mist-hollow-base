using System;
using System.Collections.Generic;
using UnityEngine;

namespace MMH
{
    public class UISystem : GameSystem<UISystem>
    {
        public static event EventHandler<OnUpdateRulesDropownArgs> OnUpdateRulesDropdown;

        private TMPro.TMP_Dropdown rulesDropdown;

        public class OnUpdateRulesDropownArgs
        {
            public CitizenStateType StateType;
        }

        protected override void Awake()
	    {
            base.Awake();

            rulesDropdown = GameObject.Find("Rules Dropdown").GetComponent<TMPro.TMP_Dropdown>();

            List<string> rules = new List<string>
            {
                CitizenStateType.CitizenIdle.ToString(),
                CitizenStateType.CitizenWander.ToString(),
            };

            rulesDropdown.AddOptions(rules);
            rulesDropdown.value = 0;

            rulesDropdown.onValueChanged.AddListener(
                delegate { OnRuleChange(); }
            );
        }

	    void Start()
        {
        
        }

        private void OnRuleChange()
		{
            OnUpdateRulesDropownArgs eventArgs = new OnUpdateRulesDropownArgs
            {
                StateType = (CitizenStateType)rulesDropdown.value
            };

            OnUpdateRulesDropdown?.Invoke(this, eventArgs);
		}
    }

	
}

