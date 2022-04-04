using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicsManager : MonoBehaviour {

    public MusicManager musicManager;
    public Slider slider;
    List<Track> tracks;

    // Start is called before the first frame update
    void Start () {
        tracks = musicManager.GetTracks();
    }

    public void PlaySplitNote () {
        tracks[0].PlayOnce(slider.value, "Piano1");
        tracks[0].PlayOnce("Piano2");
    }

    public void PlayWholeNote () {
        tracks[1].PlayOnce(slider.value, "PianoWhole");
    }

    public void PlayBoth () {
        //prevent synchronized play if either track is still playing
        if(!tracks[0].IsPlaying() && !tracks[1].IsPlaying()) {
            double timing = AudioSettings.dspTime + slider.value;
            tracks[0].PlayWithTiming(timing, "Piano1", "Piano2");
            tracks[1].PlayWithTiming(timing, "PianoWhole");
        }

    }

}
