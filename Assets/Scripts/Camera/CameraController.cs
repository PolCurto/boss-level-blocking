using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Camera camera;

    [SerializeField] private float zoomOutSize = 10f; 
    [SerializeField] private float zoomInSize = 5f;  
    [SerializeField] private float zoomDuration = 1f; 

    private Vector3 zoomOutPosition;
    private Vector3 initialPosition;

    private Coroutine zoomCoroutine; 
    

    Material obstacleMaterial;
    RaycastHit hit;

    bool aux = false;

    private void Start()
    {
       
    }

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
        Gizmos.DrawLine(camera.transform.position, player.position);
    }

    private void CheckObstacles()
    {
        if (Physics.Raycast(camera.transform.position, player.position - camera.transform.position, out hit, float.MaxValue, LayerMask.GetMask("Obstacle")))
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

    public void CameraZoomOut()
    {
        zoomInSize = camera.orthographicSize;
        zoomOutSize = camera.orthographicSize + zoomOutSize;
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(zoomOutSize));
    }

    public void CameraZoomIn()
    {
        // Inicia la transición al tamaño ortográfico de zoom in.
        if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(zoomInSize));
    }

    private IEnumerator SmoothZoom(float targetSize)
    {
        float startSize = camera.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < zoomDuration)
        {
            // Interpola suavemente entre el tamaño actual y el tamaño objetivo.
            camera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / zoomDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegúrate de llegar exactamente al tamaño objetivo.
        camera.orthographicSize = targetSize;
    }
}
