using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuantizedManager : MonoBehaviour {

    public MusicManager musicManager;
    List<Track> tracks;

    // Start is called before the first frame update
    void Start () {
        tracks = musicManager.GetTracks();
        tracks[0].Loop("Waltz1", "Waltz2");
    }

    // Update is called once per frame
    void Update () {

    }

    public void PlayOnMeasure () {
        Track t = tracks[1];
        if(!t.IsPlaying()) {
            double nextMeasureTiming = tracks[0].GetNextMeasureTiming();
            t.PlayWithTiming(nextMeasureTiming, "ShortNote");
        }
    }

    public void PlayOnNote () {
        Track t = tracks[2];
        if(!t.IsPlaying()) {
            double nextNoteTiming = tracks[0].GetNextNoteTiming();
            t.PlayWithTiming(nextNoteTiming, "ShortNote");
        }
    }

    public void PlaySynthOnMeasure () {
        Track t = tracks[3];
        if(!t.IsPlaying()) {
            double nextMeasureTiming = tracks[0].GetNextMeasureTiming();
            t.PlayWithTiming(nextMeasureTiming, "Synth");
        }
    }

}
