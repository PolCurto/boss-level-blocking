using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject rockSpawner;
    [SerializeField] private GameObject hugeRock;
    [SerializeField] private RuneScript runeScript;
    [SerializeField] private BossLifeComponent bossLife;

    [Header("Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float minPlayerDistance;
    [SerializeField] private float dashCd;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private Vector3 secondPhasePosition;

    [SerializeField] private bool isSecondEncounter;

    private bool targetPlayer;
    public bool isDashing;
    private bool isAngry;

    private float timer;

    private void Update()
    {
        Debug.Log("Target player: " + targetPlayer);
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
        if (bossLife.isDead)
        {
            bossLife.isDead = false;
        } 
        else
        {
            Debug.Log("Stop stun");
            timer = 0;
            targetPlayer = true;
            isDashing = false;
        } 
    }


    public void Die()
    {
        Debug.Log("Boss is dead");
        GameManager.Instance.OnBossSecondPhase();

        targetPlayer = false;
        transform.position = secondPhasePosition;
        rb.velocity = Vector3.zero;

        if (!isSecondEncounter)
        {
            isSecondEncounter = true;
            bossLife.Heal();
        } 
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void AttackPlayer(bool attack)
    {
        Debug.Log("Attack player");
        targetPlayer = attack;
    }

    public void HalfLife()
    {
        Debug.Log("Boss half life");
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

        Instantiate(hugeRock, transform.position + new Vector3(-10, 30, 5), Quaternion.Euler(-90, 0, 225));

        yield return new WaitForSeconds(0.5f);

        Instantiate(hugeRock, transform.position + new Vector3(10, 30, 0), Quaternion.Euler(-90, 0, 225));

        yield return new WaitForSeconds(0.5f);

        Instantiate(hugeRock, transform.position + new Vector3(0, 30, 10), Quaternion.Euler(-90, 0, 225));

        yield return new WaitForSeconds(1.0f);
        targetPlayer = true;

    }

    public void OnPlayerFall()
    {
        targetPlayer = false;
        isAngry = false;
        bossLife.Heal();
    }
}
