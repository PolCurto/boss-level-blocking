using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform camera;

    Material obstacleMaterial;

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
        RaycastHit hit;
        if (Physics.Raycast(camera.position, player.position - camera.position, out hit, float.MaxValue, LayerMask.GetMask("Obstacle")))
        {
            if (obstacleMaterial == null)
            {
                obstacleMaterial = hit.transform.GetComponent<MeshRenderer>().material;
                Color color = obstacleMaterial.color;
                color.a = 0.2f;
                Debug.Log(obstacleMaterial.color);
                Debug.Log("Color: " + color);
                obstacleMaterial.color = color;
                Debug.Log("Final color: " + obstacleMaterial.color);
            }
        }
        else
        {
           if (obstacleMaterial != null)
           {
               Color color = obstacleMaterial.color;
               color.a = 1;
               obstacleMaterial.color = color;
               obstacleMaterial = null;
           }
        }
    }
}
