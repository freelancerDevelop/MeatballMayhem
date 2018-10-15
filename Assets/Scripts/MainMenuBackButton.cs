using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuBackButton : MonoBehaviour {

    public Button backButton;

	// Use this for initialization
	void Start ()
    {
        backButton = GetComponent<Button>();
	}

    public void GoBack()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
