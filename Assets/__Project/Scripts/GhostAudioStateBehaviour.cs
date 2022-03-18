using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAudioStateBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private AudioClip audioClip;
    [SerializeField]
    [Range(0f, 1f)]
    private float volume = 1;
    [SerializeField]
    private bool loop;

    private AudioSource audioSource;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource == null)
        {
            audioSource = animator.GetComponent<AudioSource>();
        }

        audioSource.clip = audioClip;
        audioSource.loop = loop;

        audioSource.Play();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource.Stop();
    }
}
