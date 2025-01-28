using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDetectorController : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    private int zoomOutsCont = 0;

    private void Start()
    {
        zoomOutsCont = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && zoomOutsCont == 0)
        {
            camera.GetComponent<CameraController>().CameraZoomOut();
            zoomOutsCont++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            camera.GetComponent<CameraController>().CameraZoomIn();
        }
    }
}
