using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.WinLevel();
            }

        }
    }

}
