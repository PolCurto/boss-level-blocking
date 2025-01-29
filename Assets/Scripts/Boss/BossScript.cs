using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speed;
    [SerializeField] private float minPlayerDistance;
    [SerializeField] private float dashCd;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    private bool targetPlayer;
    private bool isDashing;
    private bool isAngry;

    private float timer;

    private void Update()
    {
        if (targetPlayer)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        if (isDashing) return;

        if (Vector3.Distance(transform.position, playerTransform.position) > minPlayerDistance)
        {
            Vector3 dir = (playerTransform.position - transform.position).normalized;
            rb.velocity = speed * Time.deltaTime * dir * 100;
        }

        if (isAngry)
        {
            timer += Time.deltaTime;

            if (timer > dashCd)
            {
                DashAttack();
            }
        }
    }

    private void DashAttack() 
    {
        Debug.Log("Dash");
        isDashing = true;
        Vector3 dir = (playerTransform.position - transform.position).normalized;
        rb.AddForce(dir * dashSpeed, ForceMode.Impulse);
        Invoke(nameof(StopDash), dashDuration);
    }

    private void StopDash()
    {
        isDashing = false;
        timer = 0;
    }


    public void Die()
    {
        Debug.Log("Boss is dead");
        GameManager.Instance.OnBossSecondPhase();

        targetPlayer = false;
        transform.position = new Vector3(180, 2, -17.5f);
    }

    public void AttackPlayer(bool attack)
    {
        Debug.Log("Attack player");
        targetPlayer = attack;
    }

    public void HalfLife()
    {
        isAngry = true;
    }
}
