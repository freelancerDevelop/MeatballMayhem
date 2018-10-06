using UnityEngine;

public class GameStateManager : MonoBehaviour {

    public RoundManager rm;
    public TwitchClient tc;
    public bool joinable;
    public bool starting;
    public bool inprogress;
    public bool finished;

	// Use this for initialization
	void Awake ()
    {
        joinable = true; //Checks whether the game is joinable.
        starting = false; //Checks whether the game is starting.
        inprogress = false; //Checks whether the game is currently in progress.
        finished = false; //Checks whether the game has finished met win condition and ended.
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Here we detect if the maxplayers variable was reached. If so, we start the game.
        if (starting && inprogress)
        {
            StartGame();
        }
	}

    public void StartGame()
    {
        //Set the starting bool in GSM back to false so we don't continuously start a new game.
        starting = false;
        inprogress = true;
        rm.startTimer = true;
        Debug.Log("Game started!");
    }
}
