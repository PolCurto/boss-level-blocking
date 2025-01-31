using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneScript : MonoBehaviour
{
    [SerializeField] private Material myMat;

    private float currentFill;
    [SerializeField] private Color defaultColor;

    private void Awake()
    {
        currentFill = 0.5f;
        myMat.SetFloat("_CutoffHeight", currentFill);
        myMat.SetColor("_BaseColor", defaultColor);
    }

    public IEnumerator FillRune(float duration)
    {
        Debug.Log("Fill rune");
        float startFill = currentFill;
        float desiredFill = startFill - 0.27f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            currentFill = Mathf.Lerp(startFill, desiredFill, elapsedTime / duration);
            myMat.SetFloat("_CutoffHeight", currentFill);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentFill = desiredFill;
        myMat.SetFloat("_CutoffHeight", currentFill);

        if (currentFill <= -0.3f)
        {
            float factor = Mathf.Pow(2, 4);
            Color startColor = myMat.GetColor("_BaseColor");
            Color desiredColor = startColor * factor;
            elapsedTime = 0;

            while (elapsedTime < 0.5f)
            {
                Color tempColor = Color.Lerp(startColor, desiredColor, elapsedTime / duration);
                myMat.SetColor("_BaseColor", tempColor);

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            myMat.SetColor("_BaseColor", desiredColor);
        }
    }
}
