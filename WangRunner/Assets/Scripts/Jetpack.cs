using UnityEngine;
using System.Collections;

public class Jetpack : MonoBehaviour {
    Renderer jetpack;
    PlayerController player;
    private ParticleSystem emission;

    // Use this for initialization
    void Start() {
        // Fixes sorting layer of jetpack
        jetpack = GetComponent<Renderer>();
        jetpack.sortingLayerName = "Player";
        jetpack.sortingOrder = -1;

        // Starts off without jetpack
        emission = GetComponent<ParticleSystem>();
        //emission.enableEmission = false;
        player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        if (player.CurrentJump == JumpAbility.Jetpack && emission.isStopped)
        {
            //jetpack.enabled = true;
            emission.Play();
        }
        if (player.CurrentJump != JumpAbility.Jetpack && emission.isPlaying)
        {
            //jetpack.enabled = false;
            emission.Stop();
        }
    }

    void FixedUpdate()
    {
        if (player.isJetpackActive)
        {
            emission.startLifetime = 0.5f;
            emission.startSpeed = 5.0f;
            emission.emissionRate = 300.0f;
        }
        else
        {
            emission.startLifetime = 0.3f;
            emission.startSpeed = 2.0f;
            emission.emissionRate = 20.0f;
        }
    }
}
