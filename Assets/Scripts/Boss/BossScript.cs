using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject rockSpawner;
    [SerializeField] private GameObject hugeRock;
    [SerializeField] private RuneScript runeScript;

    [SerializeField] private float speed;
    [SerializeField] private float minPlayerDistance;
    [SerializeField] private float dashCd;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;

    [SerializeField] private bool isSecondEncounter;

    private bool targetPlayer;
    public bool isDashing;
    private bool isAngry;

    private float timer;

    private void Update()
    {
        if (targetPlayer)
        {
            FollowPlayer();
        }

        Debug.Log("Target player: " + targetPlayer);
        Debug.Log("Is dashing: " + isDashing);
    }

    private void FollowPlayer()
    {
        if (isDashing) return;

        if (Vector3.Distance(transform.position, playerTransform.position) > minPlayerDistance)
        {
            Vector3 dir = (playerTransform.position - transform.position).normalized;
            rb.velocity = speed * Time.deltaTime * dir * 100;
        }

        if (isAngry || isSecondEncounter)
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
        //Debug.Log("Dash");
        isDashing = true;
        Vector3 dir = (playerTransform.position - transform.position).normalized;
        rb.AddForce(dir * dashSpeed, ForceMode.Impulse);
        Invoke(nameof(StopDash), dashDuration);
    }

    private void StopDash()
    {
        if (!targetPlayer) return;

        isDashing = false;
        timer = 0;
    }

    public void Stun()
    {
        Debug.Log("Stun");
        rb.velocity = Vector3.zero;
        targetPlayer = false;
        isDashing = true;

        if (isSecondEncounter)
        {
            StartCoroutine(runeScript.FillRune(1.0f));
        }

        Invoke(nameof(StopStun), 1.0f);
    }

    private void StopStun()
    {
        Debug.Log("Stop stun");
        timer = 0;
        targetPlayer = true;
        isDashing = false;
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
        //Debug.Log("Attack player");
        targetPlayer = attack;
    }

    public void HalfLife()
    {
        isAngry = true;

        if (isSecondEncounter)
        {
            StartCoroutine(SpawnRocks());
        }
    }

    private IEnumerator SpawnRocks()
    {
        Debug.Log("Spawn Rocks");
        rockSpawner.SetActive(true);

        targetPlayer = false;
        timer = 0;

        yield return new WaitForSeconds(5.0f);

        rockSpawner.SetActive(false);

        Instantiate(hugeRock, transform.position + new Vector3(5, 10, 1), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Instantiate(hugeRock, transform.position + new Vector3(1, 10, 8), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Instantiate(hugeRock, transform.position + new Vector3(10, 10, 5), Quaternion.identity);

        yield return new WaitForSeconds(1.0f);
        targetPlayer = true;

    }
}
