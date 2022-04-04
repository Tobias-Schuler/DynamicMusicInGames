using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    [Tooltip("Attached to tracks to send messages about audio files to be played back.")]
    public AudioVisualizer visualizer;

    //the maximum amount of tracks to use
    public const int maxTracks = 5;

    [Tooltip("The sound library to be used.")]
    public SoundWrapper[] sounds;

    [Tooltip("The amount of tracks available.")]
    [Range(1, maxTracks)]
    public int trackCount = 1;

    //list of all tracks as a list of script references
    List<Track> tracks = new List<Track>();

    void Awake () {
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
            trackScript.SetTrackNumber(i);
            trackScript.SetVisualizer(visualizer);
            tracks.Add(trackScript);
        }
    }

    public List<Track> GetTracks () {
        return tracks;
    }

    // Update is called once per frame
    void Update () {
    }
}
