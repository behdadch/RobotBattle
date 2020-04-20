using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSfx : MonoBehaviour {
    

    public List<AudioClip> clips;

 

    private void Start() {
        GetComponent<AudioSource>().pitch = Random.Range(.85f, 1.15f);
        GetComponent<AudioSource>().PlayOneShot(clips[Random.Range(0, clips.Count - 1)]);
    }
}
