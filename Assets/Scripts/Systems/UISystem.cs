using System;
using System.Collections;
using System.Collections.Generic;
using MMH;
using UnityEngine;
using UnityEngine.UI;

namespace MMH
{
    public class UISystem : MonoBehaviour
    {
        private static UISystem _instance;
        public static UISystem Instance { get { return _instance; } }

        public static event EventHandler<OnUpdateRulesDropownArgs> OnUpdateRulesDropdown;

        private TMPro.TMP_Dropdown rulesDropdown;

        public class OnUpdateRulesDropownArgs
        {
            public string RuleName;
        }

        private void Awake()
	    {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }

            rulesDropdown = GameObject.Find("Rules Dropdown").GetComponent<TMPro.TMP_Dropdown>();

            List<string> rules = new List<string>
            {
                CitizenStateType.CitizenIdle.ToString(),
                CitizenStateType.CitizenWander.ToString(),
            };

            rulesDropdown.AddOptions(rules);
            rulesDropdown.value = 0;

            rulesDropdown.onValueChanged.AddListener(delegate {
                RulesDropdownChanged();
            });
        }

	    void Start()
        {
        
        }

        private void RulesDropdownChanged()
		{
            OnUpdateRulesDropdown?.Invoke(
                this, 
                new OnUpdateRulesDropownArgs { 
                    RuleName = rulesDropdown.options[rulesDropdown.value].text 
                }
            );
		}
    }

	
}

