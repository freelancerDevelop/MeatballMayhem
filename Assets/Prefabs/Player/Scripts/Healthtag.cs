using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthtag : MonoBehaviour {

    public Text healthValue;
    public Health health;

    // Use this for initialization
    void Start ()
    {
        healthValue = GetComponentInChildren<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        healthValue.text = health.currentHealth.ToString();
    }
}
