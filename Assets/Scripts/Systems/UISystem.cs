using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    private static UISystem _instance;
    public static UISystem Instance { get { return _instance; } }

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
    }

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
