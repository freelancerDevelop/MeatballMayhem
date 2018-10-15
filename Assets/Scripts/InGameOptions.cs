using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameOptions : MonoBehaviour {

    public bool optionsMenuActive;
    public GameObject optionsMenu;
    public Button backToGameButton;
    public Button mainMenuButton;

	// Use this for initialization
	void Start ()
    {
        optionsMenuActive = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!optionsMenuActive)
            {
                optionsMenuActive = true;
                optionsMenu.SetActive(true);

            }
            else
            {
                optionsMenuActive = false;
                optionsMenu.SetActive(false);
            }
        }
	}
}
