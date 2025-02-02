using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopShakeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Stop Shake");
            GameManager.Instance.StopShake();
        }
    }
}
