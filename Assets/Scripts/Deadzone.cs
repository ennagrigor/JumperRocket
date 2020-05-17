using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deadzone : MonoBehaviour
{
    [SerializeField] private GameObject _respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false; // so it falls and respawns
            }

            other.transform.position = _respawnPoint.transform.position;

            StartCoroutine(CCEnableRoutine(cc));
        }
    }

    IEnumerator CCEnableRoutine(CharacterController controller) // access to charecter controller to reenable after fall 
    {
        yield return new WaitForSeconds(1.0f);
        controller.enabled = true;
    }
}
