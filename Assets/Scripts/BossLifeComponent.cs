using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLifeComponent : LifeComponent
{
    [SerializeField] private BossScript bossScript;
    override protected void Die()
    {
        bossScript.Die();
    }
}
