using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour {

    public MusicManager musicManager;
    List<Track> tracks;
    bool allowTransition = true;

    // Start is called before the first frame update
    void Start () {
        tracks = musicManager.GetTracks();
        tracks[0].Loop("Hidden1", "Hidden2");
    }

    public void PlayTransition () {
        if(allowTransition) {
            tracks[0].Stop();
            tracks[1].LoopWithIntro("HiddenTransition", "HiddenFaster1", "HiddenFaster2");
            allowTransition = false;
        }

    }
}
