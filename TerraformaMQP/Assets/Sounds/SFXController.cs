using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour
{

    public static SFXController instance;

    [SerializeField] private AudioSource sfxObject;

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
    }

    public void PlaySFXClip(AudioClip audioClip, Transform spawnTransform, float volume) {

        //Spawn in gameObject
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip
        audioSource.clip = audioClip;

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sfx clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }

     public void PlayRandomSFXClip(AudioClip[] audioClip, Transform spawnTransform, float volume) {

        //assign a random index
        int rand = Random.Range(0, audioClip.Length);

        //Spawn in gameObject
        AudioSource audioSource = Instantiate(sfxObject, spawnTransform.position, Quaternion.identity);

        //Assign the audioClip
        audioSource.clip = audioClip[rand];

        //assign volume
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of sfx clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
    }


}
