using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private ParticleSystem particles;

    [SerializeField] private float fallingSpeed;

    private bool isPlayingParticles;


    // TODO Blob shadow that grows bigger as it falls down

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // TODO: Despawn and particle effect
            meshRenderer.enabled = false;
            particles.gameObject.SetActive(true);
            particles.Play();
            isPlayingParticles = true;
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // TODO: Damage player (and maybe particles)
        }
        else if (collision.gameObject.CompareTag("Water"))
        {
            // TODO: Water splash particles
        }
    }

    void Start()
    {
        rb.AddForce(Vector3.down * fallingSpeed, ForceMode.VelocityChange);
    }

    void Update()
    {
        if (isPlayingParticles && particles.isStopped)
        {
            gameObject.SetActive(false);
        }
    }
}
