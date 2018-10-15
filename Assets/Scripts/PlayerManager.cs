using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public TwitchClient tc;
    public GameStateManager gsm;
    public MaxPlayers mp;
    public int currentPlayers = 0;
    public bool atMaxPlayers;
    public List<string> playerList = new List<string>();
    public Transform[] spawnPoints;
    public GameObject Meatball = null;
    public GameObject player;
    public string playerName;

    private void Start()
    {
        atMaxPlayers = false;
    }


    public IEnumerator CheckForMaxPlayers(bool atMaxplayers)
    {
        //If we reach the required maximum players and the game hasn't been started
        //Let the game manager know it should start.
        if (currentPlayers == MaxPlayers.maxPlayers && !gsm.starting && !gsm.inprogress)
        {
            //Set the bools on maxplayers and in GSM to true so we can start the game.
            atMaxPlayers = true;
            gsm.starting = true;
            gsm.inprogress = true;
            Debug.Log("Game is now full!");
        }
        //If someone tries to join the game while the game is already in progress
        else if (currentPlayers == MaxPlayers.maxPlayers && !gsm.starting && gsm.inprogress)
        {
            //We should ignore that request.
        }
        //If the game isn't full, it hasn't started, and/or we're not at max players
        // change the bool so chatters can join.
        else
        {
            atMaxPlayers = false;
        }
        yield return atMaxPlayers;
    }

    public void SpawnPlayer()
    {
        //Set up spawn points to spawn the Meatballs at
        //Later we need to compare the spawns we've used to the list and remove it from the list 
        //to prevent spawns in the same place
        int spawnPointsIndex = Random.Range(0, spawnPoints.Length);

        //Create a variable called player and instantiate the player into the game.
        var player = Instantiate(Meatball, spawnPoints[spawnPointsIndex].position, Quaternion.identity) as GameObject;

        //With the var we've created we can now name the player and set it up under the parent gameobject
        player.name = playerName;
        player.transform.SetParent(transform, true);
    }
}
