using System;
using System.Collections.Generic;
using UnityEngine;

public enum FadeType {
    None = 0,
    LinIn,
    LinOut,
    LogIn,
    LogOut,
    ExpIn,
    ExpOut
}

//represents a single track that takes care of timing audio files after one another and applying fades 
public class Track : MonoBehaviour {

    List<SoundWrapper> sounds = new List<SoundWrapper>();
    List<AudioSource> sources = new List<AudioSource>();

    //duration in seconds to delay playback, cannot be 0 to seamlessly schedule and play back 2 files in sequence
    double playbackDelay = 0.1d;

    int indexToPlay = -1;
    int[] audioIndexSequence = new int[] { }; //sequence of indices to access sounds
    bool looping = false; //if true, use index sequence to loop audio (files)

    double nextAudioTime = 0; //at what absolute time the next audio file should be played
    double startAudioTime = 0; //absolute time when an audio clip started playback
    double currentNoteFrequency = 0;
    double currentMeasureFrequency = 0;

    double startFadeTime = 0; //absolute time when fade started
    double fadeDuration = 0;
    FadeType fadeState = FadeType.None;

    public void AddSound (SoundWrapper s) {
        sounds.Add(s);
        AudioSource a = this.gameObject.AddComponent<AudioSource>();
        a.clip = s.clip;
        a.pitch = s.pitch;
        sources.Add(a);
    }

    //returns the pitch to use for further calculations, the pitch dictates playback speed of an audio file: a pitch of 2 means a file is played twice as fast while 0.5 means it's half as fast, it can be a negative number to reverse playback but this feature will not be utilized in this project
    private float GetPitch (AudioSource a) {
        return Mathf.Abs(a.pitch);
    }

    void UpdateNextTiming (double duration) {
        if(duration >= 0) {
            nextAudioTime += duration;
        }
    }

    public void PlayOnce (string name) {
        PlayOnce(0, name);
    }

    //plays an audio file on next timing
    public void PlayOnce (double delay, string name) {
        AudioSource audio = GetAudioByName(name);
        audio.PlayScheduled(nextAudioTime + delay);
        UpdateNextTiming((audio.clip.length / GetPitch(audio)) + delay);
    }

    public void PlayWithTiming (double timing, string name) {
        AudioSource audio = GetAudioByName(name);
        audio.PlayScheduled(timing);
    }

    public void Loop (double delay, params string[] names) {
        PlaySequence(delay, names);
        looping = true;
    }

    public void Loop (params string[] names) {
        PlaySequence(0, names);
        looping = true;
    }

    public void LoopWithIntro (double delay, string name, params string[] names) {
        PlayOnce(delay, name);
        PlaySequence(0, names);
        looping = true;
    }

    public void LoopWithIntro (string name, params string[] names) {
        PlayOnce(0, name);
        PlaySequence(0, names);
        looping = true;
    }

    //abruptly stop all audio
    public void Stop () {
        foreach(AudioSource s in sources) {
            if(s.isPlaying) {
                s.Stop();
                indexToPlay = -1;
            }
        }
    }

    //restart playback sequence
    public void Restart () {
        Stop();
        indexToPlay = 0;
        nextAudioTime = AudioSettings.dspTime;
    }

    AudioSource GetAudioByName (string name) {
        return sources[sounds.FindIndex(s => s.name.Equals(name))];
    }

    //plays a sequence of audio files based on name order
    private void PlaySequence (double delay, params string[] names) {
        int[] sequence = new int[names.Length];
        for(int i = 0; i < names.Length; i++) {
            int audioIndex = sounds.FindIndex(s => s.name.Equals(names[i]));
            if(audioIndex != -1) sequence[i] = audioIndex;
        }
        audioIndexSequence = sequence;
        indexToPlay = 0;
        UpdateNextTiming(delay);
    }

    private void ScheduleNextLoop () {
        double currentAudioTime = AudioSettings.dspTime;

        if(indexToPlay >= 0 && indexToPlay < audioIndexSequence.Length) {
            if(currentAudioTime + playbackDelay >= nextAudioTime) {
                int soundIndex = audioIndexSequence[indexToPlay];
                SoundWrapper s = sounds[soundIndex];

                //scheduling next audio playback and updating timing
                AudioSource audioToPlay = sources[soundIndex];
                audioToPlay.PlayScheduled(nextAudioTime);


                startAudioTime = nextAudioTime;

                float pitch = GetPitch(audioToPlay);
                //update note and measure frequency based on next scheduled audio file
                currentNoteFrequency = s.NoteFrequency() / pitch;
                currentMeasureFrequency = s.MeasureFrequency() / pitch;

                UpdateNextTiming(audioToPlay.clip.length / pitch); //set next time for another audio file to play when the newly scheduled one ends

                //loop handling
                if(!looping) indexToPlay++;
                else indexToPlay = (indexToPlay + 1) % audioIndexSequence.Length;
            }
        }
    }

    //https://johnleonardfrench.com/ultimate-guide-to-playscheduled-in-unity/
    public double GetNextNoteTiming () {
        double currentAudioTime = AudioSettings.dspTime;
        double remainder = (currentAudioTime - startAudioTime) % currentNoteFrequency;
        return currentAudioTime + (currentNoteFrequency - remainder);
    }

    public double GetNextMeasureTiming () {
        double currentAudioTime = AudioSettings.dspTime;
        double remainder = (currentAudioTime - startAudioTime) % currentMeasureFrequency;
        return currentAudioTime + (currentMeasureFrequency - remainder);
    }

    private void UpdateFade () {
        if(fadeState != FadeType.None) {
            double currentAudioTime = AudioSettings.dspTime;
            float x = (float) ((currentAudioTime - startFadeTime) / fadeDuration);  //the normalized value of how much time elapsed since start of fade, value close to 0 means it just started, value close to 1 means it's enaring completion
            float y = 0; // the computed volume at a specific point in time
            y = ComputeVolume(x);

            UpdateVolume(y);
            //fade has finished when y < 0 or y > 1 , therefore reset fade state
            if(y < 0 || y > 1) {
                fadeState = FadeType.None;
            }
        }

    }

    private float ComputeVolume (float x) {
        float x2 = 1 - x;
        switch(fadeState) {
            case (FadeType.LinIn):
                return x;
            case (FadeType.LinOut):
                return x2;
            case (FadeType.LogIn):
                return 1 - (x2 * x2 * x2);
            case (FadeType.LogOut):
                return 1 - (x * x * x);
            case (FadeType.ExpIn):
                return x * x * x;
            case (FadeType.ExpOut):
                return x2 * x2 * x2;
        }
        throw new Exception("Invalid Fade State.");
    }

    //updates volume of all audio files on this track
    private void UpdateVolume (float newVolume) {
        foreach(AudioSource s in sources) {
            s.volume = newVolume;
        }
    }

    public void Fade (double duration, FadeType type) {
        //prevent issuing a fade when one is already happening
        if(fadeState == FadeType.None) {
            startFadeTime = AudioSettings.dspTime;
            fadeState = type;
            fadeDuration = duration;
        }
    }

    // Start is called before the first frame update
    void Start () {
        UpdateNextTiming(AudioSettings.dspTime);
    }

    // Update is called once per frame
    void Update () {
        UpdateFade();
        ScheduleNextLoop();
    }
}
