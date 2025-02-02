using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LifeComponent : MonoBehaviour
{
    [SerializeField] protected int maxLife;

    protected int currentLife;
    private bool isDead;

    private void Awake()
    {
        currentLife = maxLife;
    }

    virtual public void GetHit(int damage)
    {
        currentLife -= damage;
        if (currentLife <= 0) Die();
    }

    virtual protected void Die()
    {
        isDead = true;
    }
}
