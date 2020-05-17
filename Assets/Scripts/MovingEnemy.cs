using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    [SerializeField] private Transform _targetA, _targetB;
    [SerializeField] private float _speed = 10.0f;
    private bool _switching = false;
    [SerializeField] private GameObject _respawnPoint;

    private void OnTriggerEnter(Collider other) // Player hits enemy
    {
        if (other.tag == "Player")
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

    void FixedUpdate()
    {
        if (_switching == false)
        {
            // move to taget B
            transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
        }
        else if (_switching == true)
        {
            // move to target A
            transform.position = Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
        }

        if (transform.position == _targetB.position)
        {
            _switching = true;
        }
        else if (transform.position == _targetA.position)
        {
            _switching = false;
        }
    }

}
