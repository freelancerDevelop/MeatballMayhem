using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour {

    public GameStateManager gsm;
    public AnnounceWinner aw;
    public Fired fired;
    public List<GameObject> playerList = new List<GameObject>();
    private GameObject player;
    private int roundLimit = 21;
    public int currentRound;
    private float timer;
    public float timeLeft;
    public string phase;
    public bool startTimer = false;
    public bool checkIfFired;

    // Use this for initialization
    void Start ()
    {
        gsm = GameObject.Find("GameManager").GetComponent<GameStateManager>();
        timer = 30f;
        timeLeft = 30f;
        phase = "Move";
    }

    // Update is called once per frame
    void Update()
    {
        //Begin the game loop
        //Before we start the timer, we have to make sure the game has started according to GSM
        //First we'll check if the game state isn't finished, because if it is, we can stop the timer
        //Then we'll check if the GSM started the move phase already by turning the timer on
        if (!gsm.finished && startTimer)
        {
            //Debug.Log(timer);
            timeLeft = timer -= Time.deltaTime;
            if (timeLeft <= 0.0f && currentRound != roundLimit)
            {
                if (phase == "Move")
                {
                    StartCoroutine(StartAttackPhase());
                    Debug.Log("Now in attack phase");
                    timeLeft = 30f;
                    timer = 30f;
                    phase = "Attack";
                }
                else
                {
                    StartCoroutine(StartMovePhase());
                    //Reset timer, change phase, and start the next round
                    Debug.Log("Now in move phase");
                    timeLeft = 30f;
                    timer = 30f;
                    phase = "Move";
                    currentRound += 1;
                }
            }
            else if (currentRound == roundLimit) // WIN CONDITION: || players left/team remaining is one)
            {
                startTimer = false;
                Debug.Log("Round limit reached!");
                StartCoroutine(aw.EndGameDraw());
            }
        }
    }

    public IEnumerator StartMovePhase()
    {
        //Loop through each player in our playerlist
        foreach (GameObject player in playerList)
        {
            //Set their fired boolean to false to prepare for next round
            player.GetComponentInChildren<Fired>().SetFiredFalse();
        }
        //Start the round timer
        yield return new WaitForSeconds(timeLeft);
    }

    //Once the timer has finished, we'll start the move phase again and the timer will, of course, reset
    public IEnumerator StartAttackPhase()
    {
        yield return new WaitForSeconds(timeLeft);
    }

    //After the game is over and it's time to hard reset, reloading the scene should (does it?) default all the values back to default.
    //This is the end of the game loop
    public IEnumerator ResetGame()
    {
        //Reset the game and start fresh
        Debug.Log("Resetting...");
        SceneManager.UnloadSceneAsync(3);
        SceneManager.LoadSceneAsync(3);
        yield return null;
    }
}
