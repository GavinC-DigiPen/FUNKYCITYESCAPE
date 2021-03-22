using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    // Time between footstep sounds
    public float TimeBetweenSteps = 0.5f;

    // List of audio clips to choose from
    public List<AudioClip> audioClips = new List<AudioClip>();

    // Components
    private AudioSource audioSource = null;
    private PlayerAnimationManager animationManager = null;
    
    // Step timer
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        animationManager = GetComponent<PlayerAnimationManager>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // Don't play sounds if not running
        if (animationManager.CurrentState != PlayerAnimationStates.Run)
            return;

        // Don't play sounds if a sound was played recently
        if (timer < TimeBetweenSteps)
            return;

        PlaySound();
    }

    void PlaySound()
    {
        int numClips = audioClips.Count;

        // Choose clip to play
        int randomClipIndex = Random.Range(0, numClips);

        // Play clip!
        audioSource.clip = audioClips[randomClipIndex];
        audioSource.Play();

        // Reset timer
        timer = 0.0f;
    }
}
