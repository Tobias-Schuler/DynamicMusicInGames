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

    //list of all game objects that represent a track
    List<GameObject> tracks = new List<GameObject>();

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
            tracks.Add(t);
        }

        tracks[0].GetComponent<Track>().Loop(3, "PianoEnd", "PianoStart");
    }

    // Update is called once per frame
    void Update () {

    }
}
