using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rock;

    [SerializeField] private float height;
    [SerializeField] private float width;
    [SerializeField] private float rockSpawnRate;
    [SerializeField] private float rockSpawnOffset;

    private float timer;
    private float nextRockTime;

    private bool isEnabled;


    void Start()
    {
        timer = 0;
        nextRockTime = 0;

        isEnabled = true;
    }

    void Update()
    {
        if (isEnabled)
        {
            CheckTimers();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, 0, height));
    }

    private void CheckTimers()
    {
        timer += Time.deltaTime;

        if (timer >= nextRockTime)
        {
            SpawnRock();
            timer = 0;
            nextRockTime = Random.Range(rockSpawnRate - rockSpawnOffset, rockSpawnRate + rockSpawnOffset);
        }
    }

    private void SpawnRock()
    {
        Vector3 rockPos = new Vector3(
            Random.Range(-width/2, width/2) + transform.position.x, 
            transform.position.y, 
            Random.Range(-height / 2, height / 2) + transform.position.z);

        GameObject obj = Instantiate(rock, rockPos, Quaternion.identity);
    }

    public void Enable(bool enable)
    {
        isEnabled = enable;
    }
}
