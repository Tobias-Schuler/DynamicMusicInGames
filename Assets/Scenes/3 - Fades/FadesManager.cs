using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadesManager : MonoBehaviour {

    public MusicManager musicManager;
    public Slider slider;
    List<Track> tracks;

    // Start is called before the first frame update
    void Start () {
        tracks = musicManager.GetTracks();
        tracks[0].Loop("Hidden1", "Hidden2");
    }

    // Update is called once per frame
    void Update () {

    }

    public void Fade (int type) {
        tracks[0].Fade(slider.value, (FadeType) type);
    }
}
