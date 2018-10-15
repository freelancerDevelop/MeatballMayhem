using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnnounceWinner : MonoBehaviour {

    public RoundManager rm;
    public PlayerManager pm;
    public GameStateManager gsm;
    public KillWinner kw;
    public Text winner;
    public Text draw;
    public List<string> playersAlive = new List<string>();
    public string winnerName;

    // Use this for initialization
    void Start ()
    {
        rm = GetComponent<RoundManager>();
        gsm = GetComponent<GameStateManager>();
        kw = GetComponent<KillWinner>();
        winner = GameObject.Find("Winner Text").GetComponent<Text>();
        draw = GameObject.Find("Draw Text").GetComponent<Text>();
        winner.enabled = false;
        draw.enabled = false;
	}

    public void CheckForWinStatus()
    {
        if (pm.atMaxPlayers)
        {
            if (playersAlive == null)
            {
                //The game is a draw
                StartCoroutine(EndGameDraw());
            }
            else if (playersAlive.Count == 1)
            {
                //The game has a winner
                winnerName = (playersAlive[0]);
                StartCoroutine(EndGameWinner());
            }
            else
            {
                Debug.Log("We should never get here! Exception made in playersAlive list!");
                Debug.Log("Player count is " + playersAlive.Count);
            }
        }
    }

    public IEnumerator EndGameDraw()
    {
        Debug.Log("Game has ended in a draw!");
        //Tell GSM the game has finished
        gsm.finished = true;
        //Show our draw text on screen for 15 seconds
        //Put it here.
        new WaitForSeconds(15);
        //Issue a hard reset to the game from Round Manager
        rm.ResetGame();
        yield return null;
    }

    private IEnumerator EndGameWinner()
    {
        Debug.Log("Game has ended with a winner!");
        //Tell GSM the game has finished
        gsm.finished = true;
        winner.enabled = true;
        //Show our winner on screen for 15 seconds
        winner.text = (winnerName + " has won the match!");
        yield return new WaitForSeconds(15);
        //Reset and remove the winner from the screen before resetting the round
        winner.text = "";
        winner.enabled = false;
        //Kill the winner off hilariously.
        StartCoroutine(kw.KillTheWinner());
        //Issue a hard reset to the game from Round Manager
        yield return null;
    }
}
