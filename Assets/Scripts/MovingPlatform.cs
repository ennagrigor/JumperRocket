using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _targetA, _targetB;
    [SerializeField] private float _speed = 1.0f;
    private bool _switching = false;
   

    // Update is called once per frame
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
        else if(transform.position == _targetA.position)
        {
            _switching = false;
        }
    }

    private void OnTriggerEnter(Collider other) //in order to move with the platform become child of platform
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }
    private void OnTriggerExit(Collider other) //exiting platform -> no longer child of platform
    {
        if (other.tag =="Player")
        {
            other.transform.parent = null;
        }
    }
}
