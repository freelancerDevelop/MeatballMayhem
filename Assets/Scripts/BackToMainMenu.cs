using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour {

    public InGameOptions igo;

    public void GoToMainMenu()
    {
        igo.optionsMenuActive = false;
        igo.optionsMenu.SetActive(false);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
