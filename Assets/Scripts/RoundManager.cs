using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour {

    public GameStateManager gsm;
    public Fired fired;
    private int roundLimit = 20;
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
        //Begin the game loop.
        //Before we start the timer, we have to make sure the game has started according to GSM.
        //First we'll check if the game state isn't finished, because if it is, we can stop the timer.
        //Then we'll check if the GSM started the move phase already by turning the timer on.
        if (!gsm.finished && startTimer)
        {
            //Debug.Log(timer);
            timeLeft = timer -= Time.deltaTime;
            if (timeLeft <= 0.0f && currentRound != 41)
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
                    //At the end of the round, gather all players into an array and
                    //set their fired bools to false so they can fire again.
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                    foreach (GameObject player in players)
                    {
                        GetComponent<Fired>().fired = false;
                    }
                    Debug.Log("Now in move phase");
                    timeLeft = 30f;
                    timer = 30f;
                    phase = "Move";
                    currentRound += 1;
                }
            }
            else if (currentRound == roundLimit + 1) // WIN CONDITION: || players left/team remaining is one)
            {
                startTimer = false;
                Debug.Log("Round limit reached!");
                StartCoroutine(EndGame());
            }
        }
    }

    //Allow the player to fire once each round.
    public IEnumerator StartMovePhase()
    {
        checkIfFired = false;
        yield return new WaitForSeconds(timeLeft);
    }

    //Once the timer has finished, we'll start the move phase again and the timer will, of course, reset.
    public IEnumerator StartAttackPhase()
    {
        yield return new WaitForSeconds(timeLeft);
    }

    //When our win condition has been found above in our Update(), we'll end the game accordingly.
    //We'll wait 15 seconds so we can show who won on screen and then let GSM know the game is over.
    //Then we'll hard-reset the game.
    public IEnumerator EndGame()
    {
        Debug.Log("Game has ended!");
        gsm.finished = true;
        new WaitForSeconds(15);
        ResetGame();
        yield return null;
    }

    //After the game is over and it's time to hard reset, reloading the scene should (does it?) default all the values back to default.
    //This is the end of the game loop.
    public void ResetGame()
    {
        //Reset the game and start fresh.
        Debug.Log("Resetting...");
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
