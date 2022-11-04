using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pusher : MonoBehaviour
{
    private Rigidbody body;
    [SerializeField] private float _moveForce = 2f;
    [SerializeField] private float _maxSpeed = 5f;

    private void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //if (Input.GetAxisRaw("Vertical") != 0)
        //{
        var v = Camera.main.transform.up * Input.GetAxisRaw("Vertical");
        //var h = Camera.main.transform.right * Input.GetAxisRaw("Horizontal");

        //Vector3 force = h * v;
        if (body.velocity.magnitude < _maxSpeed)
        {
            body.AddForce(v * _moveForce, ForceMode.Force);
        }
        //}
    }
}
