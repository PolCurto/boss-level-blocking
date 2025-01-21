using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public void Die()
    {
        Debug.Log("Boss is dead");
        GameManager.Instance.OnBossSecondPhase();

        transform.position = new Vector3(180, 2, -17.5f);
    }
}
