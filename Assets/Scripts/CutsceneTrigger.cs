using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayableDirector cutscene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player.DisablePlayer();
            cutscene.Play();

            Invoke(nameof(EnablePlayer), 9.0f);
        }
    }

    private void EnablePlayer()
    {
        player.EnablePlayer();
        gameObject.SetActive(false);
    }
}
