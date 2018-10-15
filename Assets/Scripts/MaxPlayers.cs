using UnityEngine;
using UnityEngine.UI;

public class MaxPlayers : MonoBehaviour
{
    public static MaxPlayers Instance { get; private set; }
    public Text text;
    public Button btnDecreasePlayers; 
    public Button btnIncreasePlayers;

    public int max = 6;
    public int min = 2;

    public static int maxPlayers;
    int increasePerClick = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        //Check to see if player has ran the game before. If not, we set the maxplayers to 6, and let them adjust later.
        if (!PlayerPrefs.HasKey("isFirstTime") || PlayerPrefs.GetInt("isFirstTime") == 1)
        {
            //Set and save all your PlayerPrefs here.
            PlayerPrefs.SetInt("Max Players", 6);

            //Set the value of isFirstTime to be false in the PlayerPrefs.
            PlayerPrefs.SetInt("isFirstTime", 0);
            PlayerPrefs.Save();
        }
    }

    // from button click event, call AdjustValue(true) if want to increase or AdjustValue(false) to decrease
    public void AdjustValue(bool increase)
    {
        // Clamp current value between min-max
        maxPlayers = Mathf.Clamp(maxPlayers+ (increase ? increasePerClick : -increasePerClick), min, max);
        text.text = maxPlayers.ToString();
        PlayerPrefs.SetInt("Max Players", maxPlayers);
        PlayerPrefs.Save();

        // Disable buttons if cannot increase/decrease
        btnDecreasePlayers.interactable = maxPlayers > min;
        btnIncreasePlayers.interactable = maxPlayers < max;
    }
}