using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAudio : MonoBehaviour {

    // Start is called before the first frame update
    void Start () {

    }

    // pause all music
    public void Pause () {
        AudioListener.pause = !AudioListener.pause;
    }
}
