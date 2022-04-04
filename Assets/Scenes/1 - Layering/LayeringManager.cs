using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeringManager : MonoBehaviour {

    public MusicManager musicManager;
    List<Track> tracks;

    // Start is called before the first frame update
    void Start () {
        tracks = musicManager.GetTracks();
        tracks[0].Loop("Piano1", "Piano2");
        tracks[1].Loop("Guitar1", "Guitar2");
        tracks[2].Loop("Drums1", "Drums2");
        MuteTracks(0, 1);
    }

    public void MuteTracks (params int[] trackNumbers) {
        foreach(int trackNumber in trackNumbers) {
            MuteTrack(trackNumber);
        }
    }

    public void MuteTrack (int trackNumber) {
        tracks[trackNumber].Mute();
    }

    public void UnmuteTrack (int trackNumber) {
        tracks[trackNumber].Unmute();
    }

    public void FadeTrack (int trackNumber, FadeType type, double duration) {
        tracks[trackNumber].Fade(duration, type);
    }

}
