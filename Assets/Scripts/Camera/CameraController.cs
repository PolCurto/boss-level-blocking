using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camera;

    Material obstacleMaterial;
    RaycastHit hit;

    bool aux = false;


    void LateUpdate()
    {
        transform.position = player.position;
    }

    private void Update()
    {
        CheckObstacles();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(camera.position, player.position);
    }

    private void CheckObstacles()
    {
        if (Physics.Raycast(camera.position, player.position - camera.position, out hit, float.MaxValue, LayerMask.GetMask("Obstacle")))
        {
            if (obstacleMaterial == null)
            {
                //obstacleMaterial = hit.transform.GetComponent<MeshRenderer>().material;
                //Color color = obstacleMaterial.color;
                //color.a = 0.2f;
                //Debug.Log(obstacleMaterial.color);
                //Debug.Log("Color: " + color);
                //obstacleMaterial.color = color;
                //Debug.Log("Final color: " + obstacleMaterial.color);
                StartCoroutine(LerpValue(0.5f, 0.3f, true));
                aux = true;
            }
        }
        else
        {
           if (aux == true)
           {
                //Color color = obstacleMaterial.color;
                //color.a = 1;
                //obstacleMaterial.color = color;
                //obstacleMaterial = null;
                StartCoroutine(LerpValue(0.5f, 1, false));
           }
        }
    }

    private IEnumerator LerpValue(float waitTime, float finalValue, bool enter)
    {
        if (enter) obstacleMaterial = hit.transform.GetComponent<MeshRenderer>().material;
        else aux = false;
        Color color = obstacleMaterial.color;

        float elapsedTime = 0;

        while (elapsedTime < waitTime)
        {
            color.a = Mathf.Lerp(color.a, finalValue, (elapsedTime / waitTime));
            obstacleMaterial.color = color;
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }

        // Make sure we got there
        color.a = finalValue;
        obstacleMaterial.color = color;
        if (!enter) obstacleMaterial = null;
        yield return null;
    }
}
