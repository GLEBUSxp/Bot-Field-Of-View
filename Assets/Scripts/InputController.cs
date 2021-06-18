using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float speed;

    public Rigidbody rb;

    private void Start()
    {
     
    }

    void Update()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(horizontalAxis, 0, verticalAxis) * speed;

        rb.velocity = moveVector;
    }
}
