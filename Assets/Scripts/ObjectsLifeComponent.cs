using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsLifeComponent : LifeComponent
{
    [SerializeField] private BreakableObjectScript objScript;

    public override void GetHit(int damage)
    {
        base.GetHit(damage);
    }


    override protected void Die()
    {
        objScript.Break();
    }

}