using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObjectScript : MonoBehaviour
{

    public void Break()
    {
        gameObject.SetActive(false);
    }
}
