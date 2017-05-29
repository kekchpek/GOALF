using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MySoundSource : MonoBehaviour {

    public float volume;
    public AudioSource audioSource;

	void Start () {
        audioSource = MainController.controller.GenerateAudioSource();
        audioSource.volume = volume;
	}

}
