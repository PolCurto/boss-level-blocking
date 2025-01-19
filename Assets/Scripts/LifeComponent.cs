using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    [SerializeField] private int maxLife;

    private int currentLife;
    private bool isDead;

    private void Awake()
    {
        currentLife = maxLife;
    }

    public void GetHit(int damage)
    {
        currentLife -= damage;
        if (currentLife <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
    }
}
