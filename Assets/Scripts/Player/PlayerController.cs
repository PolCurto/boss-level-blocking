using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Global Variables
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform attackRef;
    [SerializeField] private BossScript boss;

    [Header("Movement parameters")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCd;
    [SerializeField] private float gravity;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Vector3 spawnPos;

    [Header("Combat parameters")]
    [SerializeField] private float attackRadius;
    [SerializeField] private int attackDamage;

    private Vector3 input;
    private Vector3 direction;

    private bool isGrounded;
    private bool isDisabled;

    private float currentGravity;
    private bool desiredDash;
    private bool isDashing;
    private float dashTimer;
    private bool canDash = true;
    #endregion

    #region Unity methods
    private void Awake()
    {
        currentGravity = gravity;
        //spawnPos = new Vector3(-50, 0, 0);
    }

    private void Update()
    {
        GatherInput();
        HandleDash();
        EnvironmentCheck();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(groundCheck.position, Vector3.down * groundCheckDistance);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackRef.position, attackRadius);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            ResetPosition();
        }
    }
    #endregion

    private void GatherInput()
    {
        if (isDisabled) return;

        input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.LeftShift)) desiredDash = true;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Hit();
        }
    }

    private void HandleDash()
    {
        if (desiredDash && canDash)
        {
            currentGravity = 0;
            isDashing = true;
            dashTimer = dashDuration;
            desiredDash = false;
            canDash = false;

            if (direction == Vector3.zero) direction = Vector3.forward;
        }
    }

    private void EnvironmentCheck()
    {
        if (Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, LayerMask.GetMask("Ground")))
        {
            isGrounded = true;
        } 
        else
        {
            isGrounded = false;
        }
    }

    private void Move()
    {
        Vector3 desiredVelocity;

        if (isDashing)
        {
            //Dashing
            desiredVelocity = dashSpeed * Time.deltaTime * direction;
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0) StopDashing();
        }
        else
        {
            // Moving
            direction = Quaternion.Euler(0, 45.0f, 0) * input;
            desiredVelocity = movementSpeed * Time.deltaTime * direction;
        }

        if (isGrounded)
        {
            desiredVelocity.y = 0;
        } 
        else
        {
            desiredVelocity.y -= currentGravity * Time.deltaTime;
        }

        if (direction != Vector3.zero) transform.forward = direction;

        rb.velocity = desiredVelocity * 100;
    }

    private void StopDashing()
    {
        currentGravity = gravity;
        isDashing = false;
        Invoke(nameof(ResetDash), dashCd);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void Hit()
    {
        //Debug.Log("Hit");
        Collider[] colliders = Physics.OverlapSphere(attackRef.position, attackRadius, LayerMask.GetMask("Enemy"));

        foreach (Collider collider in colliders)
        {
            //Debug.Log("Collider hit");
            if (collider.TryGetComponent<LifeComponent>(out LifeComponent life))
            {
                //Debug.Log("Collider has life");
                life.GetHit(1);
            }
        }

        Collider[] breakables = Physics.OverlapSphere(attackRef.position, attackRadius, LayerMask.GetMask("Obstacle"));

        foreach (Collider collider in breakables)
        {
            Debug.Log("breakables hit");
            if (collider.TryGetComponent<LifeComponent>(out LifeComponent life))
            {
                Debug.Log("breakables has life");
                life.GetHit(1);
            }
        }
    }

    private void ResetPosition()
    {
        rb.MovePosition(spawnPos);
        boss.OnPlayerFall();
    }

    public void SetSpawnPos(Vector3 newPos)
    {
        spawnPos = newPos;
    }

    public void DisablePlayer()
    {
        isDisabled = true;
        rb.velocity = Vector3.zero;
        input = Vector3.zero;
        direction = Vector3.zero;
    }

    public void EnablePlayer()
    {
        isDisabled = false;
        Debug.Log("Enable player");
    }

}