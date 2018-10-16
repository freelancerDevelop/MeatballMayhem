using UnityEngine;
using UnityEngine.UI;

public class PreRequisiteCheck : MonoBehaviour {

    public Button startGameButton;

	// Use this for initialization
	void Start ()
    {
        startGameButton = GameObject.Find("Start Game Button").GetComponent<Button>();
        startGameButton.interactable = false;

		if (!PlayerPrefs.HasKey("OAUTH_TOKEN"))
        {
            // Disable the Start Game button until the streamer sets up their stream.
            startGameButton.interactable = false;
        }
        else if (PlayerPrefs.GetString("OAUTH_TOKEN") == "" || PlayerPrefs.GetString("CLIENT_ID") == "" 
                || PlayerPrefs.GetString("CHANNEL_NAME") == "" || PlayerPrefs.GetString("CHANNEL_ID") == "")
        {
            startGameButton.interactable = false;
        }
        else
        {
            startGameButton.interactable = true;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
