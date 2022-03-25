using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//represents a single track
public class Track : MonoBehaviour {

    List<SoundWrapper> sounds = new List<SoundWrapper>();
    List<AudioSource> sources = new List<AudioSource>();

    int indexToPlay = -1;
    int[] indexSequence = new int[] { };
    bool looping = false;                                       //if true, use index sequence too loop audio

    //represents at what time the next audio file should be played back
    double nextAudioTiming = 0;

    public void AddSound (SoundWrapper s) {
        sounds.Add(s);
        AudioSource a = this.gameObject.AddComponent<AudioSource>();
        a.clip = s.clip;
        sources.Add(a);
    }

    public void PlayOnce (params string[] names) {
        Play(0, names);
    }

    public void PlayOnce (double delay, params string[] names) {
        Play(delay, names);
    }

    public void Loop (params string[] names) {
        Play(0, names);
        looping = true;
    }

    public void Loop (double delay, params string[] names) {
        Play(delay, names);
        looping = true;
    }

    //play audio after current audio has finished by name
    void Play (double delay, params string[] names) {
        int[] sequence = new int[names.Length];
        for(int i = 0; i < names.Length; i++) {
            sequence[i] = sounds.FindIndex(s => s.name.Equals(names[i]));
        }
        indexSequence = sequence;
        indexToPlay = 0;
        nextAudioTiming += delay;
    }

    // Start is called before the first frame update
    void Start () {
        nextAudioTiming += AudioSettings.dspTime;
    }

    // Update is called once per frame
    void Update () {

        double currentAudioTime = AudioSettings.dspTime;

        if(indexToPlay >= 0 && indexToPlay < indexSequence.Length) {
            if(currentAudioTime + 0.1d >= nextAudioTiming) {
                AudioSource audioToPlay = sources[indexToPlay];
                audioToPlay.PlayScheduled(nextAudioTiming);
                nextAudioTiming += audioToPlay.clip.length;

                if(!looping)
                    indexToPlay++;
                else
                    indexToPlay = (indexToPlay + 1) % indexSequence.Length;
            }

        }
    }

}
