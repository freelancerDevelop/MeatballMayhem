using UnityEngine;

public class BackToGame : MonoBehaviour {

    public InGameOptions igo;

    public void GoBackToGame()
    {
        igo.optionsMenuActive = false;
        igo.optionsMenu.SetActive(false);
    }
}
