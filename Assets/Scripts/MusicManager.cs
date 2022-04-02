using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    //the maximum amount of tracks to use
    public const int maxTracks = 5;

    [Tooltip("The sound library to be used.")]
    public SoundWrapper[] sounds;

    [Tooltip("The amount of tracks available.")]
    [Range(1, maxTracks)]
    public int trackCount = 1;

    //list of all tracks as a list of script references
    List<Track> tracks = new List<Track>();

    double testing = 0;
    // Start is called before the first frame update
    void Start () {

        for(int i = 0; i < trackCount; i++) {
            GameObject t = new GameObject("Track " + i);
            t.transform.parent = this.gameObject.transform;
            Track trackScript = t.AddComponent<Track>();
            //add sounds to specific tracks
            foreach(SoundWrapper s in sounds) {
                if(s.trackNumber == i) {
                    trackScript.AddSound(s);
                }
            }
            tracks.Add(trackScript);
        }

        //tracks[0].Loop(1.5d, "Hidden", "PianoStart", "PianoEnd");
        tracks[0].Loop("Waltz");

        testing = AudioSettings.dspTime;
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(AudioSettings.dspTime - testing);

        if(Input.GetKeyDown(KeyCode.W)) {
            tracks[0].Stop();
        }

        if(Input.GetKeyDown(KeyCode.R)) {
            tracks[0].Restart();
        }

        if(Input.GetKeyDown(KeyCode.E)) {
            tracks[0].Fade(5, FadeType.LogIn);
        }

        if(Input.GetKeyDown(KeyCode.D)) {
            tracks[0].Fade(5, FadeType.LogOut);
        }

        if(Input.GetKeyDown(KeyCode.Q)) {
            double nextMeasureTiming = tracks[0].GetNextMeasureTiming();
            tracks[1].PlayWithTiming(nextMeasureTiming, "PianoWhole");
        }
    }
}
