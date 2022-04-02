using UnityEngine;

public enum BPMBases {
    Default,
    HalfNote,
    QuarterNote,
    EighthNote,
    HalfNoteDotted,
    QuarterNoteDotted,
    EighthNoteDotted
}

public enum Denominators {
    Two = 2,
    Four = 4,
    Eight = 8,
    Sixteen = 16,
}

//holds audio files and data in regards to them like track to be on, BPM and time signature
[System.Serializable]
public class SoundWrapper {
    /** numeric values for the following notes: 
     * Half Note
     * Quarter Note
     * Eighth Note
     * Half Note Dotted
     * Quarter Note Dotted
     * Eighth Note Dotted
   */
    private double[] noteValues = new double[] { 1 / 2, 1 / 4, 1 / 8, 3 / 4, 3 / 8, 3 / 16 };

    [Tooltip("The audio file itself.")]
    public AudioClip clip;

    [Tooltip("An abitrary name given to this audio file to be used later as reference")]
    public string name;

    [Tooltip("The track onto which this audio file should run.")]
    [Range(0, MusicManager.maxTracks - 1)]
    public int trackNumber;

    [Header("Tempo")]
    public int BPM = 120;
    [Tooltip("The note to use as the BPM basis. Default refers to the time signature denominator.")]
    public BPMBases basis;
    [Tooltip("The pitch to alter playback speed.")]
    [Range(0.1f, 3)]
    public float pitch = 1;

    [Header("Time Signature")]
    public int numerator = 4;
    public Denominators denominator = Denominators.Four;

    //returns the time between 2 notes in seconds
    public double NoteFrequency () {
        return (double) 60 / BPM;
    }

    //returns the time for one measure according to specified note basis and time signature in seconds
    public double MeasureFrequency () {
        if(basis == 0) {                                //for common case of denominator and bpm basis are the same
            return numerator * NoteFrequency();
        } else {                                        //for uncommon cases where the bpm basis can be ambiguous like 6/8
            double noteBase = noteValues[(int) basis - 1];
            return noteBase * NoteFrequency();
        }
    }

}


