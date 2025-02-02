using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static GameManager Instance;

    [SerializeField] private GameObject firstArena;
    [SerializeField] private GameObject secondArena;

    [SerializeField] private CameraShaker shaker;
    [SerializeField] private RockSpawner[] rockSpawners;

    [SerializeField] private float stageTransitionTime;
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        secondArena.transform.position -= Vector3.up * 10;
    }

    public void OnBossSecondPhase()
    {
        StartCoroutine(ModifyStage());
        shaker.StartShaking();
        for (int i = 0; i < rockSpawners.Length; i++)
        {
            rockSpawners[i].gameObject.SetActive(true);
        }

        playerController.SetSpawnPos(new Vector3(60, 1.5f, 2));
    }

    public void StopShake()
    {
        shaker.StopShaking();
        for (int i = 0; i < rockSpawners.Length; i++)
        {
            rockSpawners[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator ModifyStage()
    {
        StartCoroutine(MoveArena(secondArena.transform, secondArena.transform.position, secondArena.transform.position + Vector3.up * 10f, 4));

        yield return new WaitForSeconds(5);

        StartCoroutine(MoveArena(firstArena.transform, firstArena.transform.position, firstArena.transform.position - Vector3.up * 2.5f, 4));
    }

    private IEnumerator MoveArena(Transform target, Vector3 startPos, Vector3 endPos, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            target.position = Vector3.Lerp(startPos, endPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.position = endPos;
    }
}
