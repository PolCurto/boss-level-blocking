using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speed = 5;

    private Vector3 input;


    void Awake()
    {
        // Get the rigidbody on this.
        rb = GetComponent<Rigidbody>();
    }


    private void Update()
    {
        GatherInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    void Move()
    {
        Vector3 direction = Quaternion.Euler(0, 45.0f, 0) * input;

        Vector3 valueToMove = direction * speed;

        rb.MovePosition(transform.position + valueToMove * Time.deltaTime);
    }
}