using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    [SerializeField] private Rigidbody rb;

    [Header("Movement parameters")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCd;

    [Header("Combat parameters")]
    [SerializeField] private Transform attackRef;
    [SerializeField] private float attackRadius;
    [SerializeField] private int attackDamage;

    private Vector3 input;
    private Vector3 direction;
    private bool desiredDash;
    private bool isDashing;
    private float dashTimer;
    private bool canDash = true;

    private void Update()
    {
        GatherInput();
    }

    void FixedUpdate()
    {
        HandleDash();
        Move();
    }

    void GatherInput()
    {
        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.LeftShift)) desiredDash = true;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Hit();
        }
    }

    void HandleDash()
    {
        if (desiredDash && canDash)
        {
            isDashing = true;
            dashTimer = dashDuration;
            desiredDash = false;
            canDash = false;
        }
    }

    void Move()
    {
        Vector3 valueToMove;

        if (isDashing)
        {
            //Dashing
            valueToMove = direction * dashSpeed;
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0) StopDashing();
        }
        else
        {
            // Moving
            direction = Quaternion.Euler(0, 45.0f, 0) * input;
            valueToMove = direction * movementSpeed;
        }
       
        rb.MovePosition(transform.position + valueToMove * Time.deltaTime);
    }

    void StopDashing()
    {
        isDashing = false;
        Invoke(nameof(ResetDash), dashCd);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void Hit()
    {
        Collider[] colliders = Physics.OverlapSphere(attackRef.position, attackRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<LifeComponent>(out LifeComponent life))
            {
                life.GetHit(1);
            }
        }
    }
}