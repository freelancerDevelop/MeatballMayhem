using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public int startingHealth;
    public bool alive;
    public float currentHealth;
    public string player;
    public Bomb bomb;
    public AnnounceWinner aw;

	// Use this for initialization
	void Start ()
    {
        aw = GetComponentInParent<AnnounceWinner>();
        player = gameObject.name;
        startingHealth = 100;
        currentHealth = startingHealth;
        alive = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Check to see if the player loses all their health
	    if (currentHealth <= 0)
        {
            //Set the player alive bool to false
            Debug.Log("Player died!");
            alive = false;
            // Remove them from the winner list
            RemoveFromAlive(player);
            // Check if there is only one player left so we can trigger the win condition
            aw.CheckForWinStatus();
            // Destroy the player
            Destroy(gameObject);
        }
    }

    public void ApplyDamage(int damage)
    {
        //The player takes damage.
        currentHealth -= damage;
        Debug.Log(gameObject + "took " + damage + " !");
    }

    private void RemoveFromAlive(string player)
    {
        aw.playersAlive.Remove(player);
    }
}
