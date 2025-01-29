using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeComponent : LifeComponent
{
    [SerializeField] private BossScript bossScript;

    public override void GetHit(int damage)
    {
        base.GetHit(damage);

        if (currentLife == 1)
        {
            bossScript.HalfLife();
        }
    }

    override protected void Die()
    {
        bossScript.Die();
    }
}
