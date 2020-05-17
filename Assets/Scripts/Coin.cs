using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") // if the coin collided with player
        {
            Player player = other.GetComponent<Player>(); //get player's components

            if(player != null) // if player is not null
            {
                player.AddCoins(); 
            }
            Destroy(this.gameObject);
        }
    }
}
