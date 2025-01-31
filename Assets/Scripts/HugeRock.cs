using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HugeRock : MonoBehaviour
{
    private BossScript bossScript;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            if (bossScript == null) bossScript = other.transform.parent.GetComponent<BossScript>();
            bossScript.Stun();
            gameObject.SetActive(false);
        }
    }

}
