using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomsPicker : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pick Mushroom");
            gameObject.SetActive(false);
        }
    }
}
