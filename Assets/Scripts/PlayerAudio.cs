using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] stepSounds;
    
    private CharacterController controller;
    private AudioSource audioSource;
    private bool stepsPlaying = false;
    private float stepSpeed;
    private Coroutine stepCoroutine = null;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        controller = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        stepSpeed = controller.velocity.magnitude < 7 ? 0.4f : 0.33f;
        
        
        if (controller.velocity.magnitude > 0.1f && controller.isGrounded && !stepsPlaying)
        {
            stepsPlaying = true;
            stepCoroutine = StartCoroutine(PlayStepSounds());
        } else if (!controller.isGrounded || controller.velocity.magnitude < 0.1f)
        {
            stepsPlaying = false;
            if (stepCoroutine != null) StopCoroutine(stepCoroutine);
        }
    }

    private IEnumerator PlayStepSounds()
    {
        while (stepsPlaying)
        {
            audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);
            audioSource.pitch = Random.Range(0.8f, 1.1f);
            yield return new WaitForSeconds(stepSpeed);
        }
    }
}
