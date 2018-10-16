using System.Collections;
using UnityEngine;

public class KillWinner : MonoBehaviour {

    public GameObject bombPrefab;
    public Transform playerLocation;
    private Vector2 bombSpawnPosition;
    private Vector2 aboveHeadPosition;
    public RoundManager rm;

    private void Start()
    {
        rm = GetComponent<RoundManager>();
    }

    public IEnumerator KillTheWinner()
    {
        playerLocation = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //Set the initial bomb position on the player
        bombSpawnPosition.x = playerLocation.transform.position.x;
        bombSpawnPosition.y = playerLocation.transform.position.y;
        //Set the horizontal position of the bombs around the player at different distances
        aboveHeadPosition.x = aboveHeadPosition.x + Random.Range(0, 0.5f);
        //Set the vertical position of the bombs above the player's head at different altitudes
        aboveHeadPosition.y = aboveHeadPosition.y + Random.Range(10, 15);

        Debug.Log("Spawning bomb 1");
        //Instantiate the explosive on the player's transform above their head
        GameObject bombInstance1 = Instantiate(bombPrefab, bombSpawnPosition + aboveHeadPosition, Quaternion.identity);
        bombInstance1.GetComponent<Rigidbody2D>().AddForce(-transform.up * 2, ForceMode2D.Impulse);

        //Give some time for the bombs to drop and kill the player
        StartCoroutine(WaitForDeath());
        yield return null;
    }

    public IEnumerator WaitForDeath()
    {
        Debug.Log("Waiting for 10 seconds before resetting...");
        yield return new WaitForSeconds(10);
        yield return null;
        StartCoroutine(rm.ResetGame());
    }
}
