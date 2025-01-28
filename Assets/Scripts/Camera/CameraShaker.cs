using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float shakeFactor;
    [SerializeField] private float startDuration;
    [SerializeField] private float endDuration;

    private Vector3 startPos;
    private float currentShakeFactor;
    private bool isShaking;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartShaking();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            StopShaking();
        }

        if (isShaking)
        {
            Shake();
        }
    }

    public void StartShaking()
    {
        Debug.Log("Start shaking");
        startPos = cameraTransform.localPosition;
        StartCoroutine(SmoothShakeTransition(shakeFactor, startDuration));
    }

    public void StopShaking()
    {
        Debug.Log("Stop shaking");
        isShaking = false;
        StartCoroutine(SmoothShakeTransition(0, endDuration));
    }

    private void Shake()
    {
        Vector3 displacement = startPos + (Random.insideUnitSphere * currentShakeFactor);
        displacement.z = startPos.z;
        cameraTransform.localPosition = displacement;
    }

    private IEnumerator SmoothShakeTransition(float desiredShake, float duration)
    {
        float elapsedTime = 0;
        float startShake = currentShakeFactor;

        while (elapsedTime < duration)
        {
            currentShakeFactor = Mathf.Lerp(startShake, desiredShake, elapsedTime / duration);
            Shake();
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentShakeFactor = desiredShake;

        if (desiredShake == 0) 
        { 
            cameraTransform.localPosition = startPos;
        } else
        {
            isShaking = true;
        }
    }
}
