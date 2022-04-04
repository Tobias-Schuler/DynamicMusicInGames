using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossfadeManager : MonoBehaviour {

    public MusicManager musicManager;
    public Slider slider;
    List<Track> tracks;
    bool fadeDirection = true;
    // Start is called before the first frame update
    void Start () {
        tracks = musicManager.GetTracks();
        tracks[0].Loop("Hidden1", "Hidden2");
        tracks[0].SetVolume(0);
        tracks[1].Loop("Other1", "Other2");

    }

    // Update is called once per frame
    void Update () {

    }

    public void CrossFade () {
        Track one = tracks[0], two = tracks[1];
        //check if fade is still ongoing
        if(one.GetCurrentFadeType() == FadeType.None && two.GetCurrentFadeType() == FadeType.None) {
            if(fadeDirection) {
                one.Fade(slider.value, FadeType.ExpIn);
                two.Fade(slider.value, FadeType.ExpOut);
            } else {
                one.Fade(slider.value, FadeType.ExpOut);
                two.Fade(slider.value, FadeType.ExpIn);
            }
            fadeDirection = !fadeDirection;
        }
    }
}
