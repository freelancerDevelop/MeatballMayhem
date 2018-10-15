using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {

    public Button startGameButton;
    public Button setupStreamButton;
    public Button optionsButton;
    public Button exitGameButton;

	// Use this for initialization
	void Start ()
    {
        startGameButton = GameObject.Find("Start Game Button").GetComponent<Button>();
        setupStreamButton = GameObject.Find("Setup Stream Button").GetComponent<Button>();
        setupStreamButton = GameObject.Find("Options Button").GetComponent<Button>();
        exitGameButton = GameObject.Find("Exit Game Button").GetComponent<Button>();
	}

    public void StartGame()
    {
        SceneManager.LoadScene(3, LoadSceneMode.Single);
    }

    public void SetupStream()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OptionsMenu()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
