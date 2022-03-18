using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunAudioStateBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private AudioClip clip;
    
    private AudioSource audioSource;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audioSource == null)
        {
            audioSource = animator.GetComponent<AudioSource>();
        }

        audioSource.clip = clip;
        audioSource.loop = true;

        if (audioSource.isPlaying == false)
        {
            audioSource.Play();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        audioSource.Stop();
    }
}
