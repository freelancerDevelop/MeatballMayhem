using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nametag : MonoBehaviour {

    public string nameText;
    public Text textObject;

	// Use this for initialization
	void Start ()
    {
        nameText = transform.parent.name;
        textObject = GetComponent<Text>();
        textObject.text = nameText;
    }
	
	// Update is called once per frame
	void Update ()
    {
	}
}
