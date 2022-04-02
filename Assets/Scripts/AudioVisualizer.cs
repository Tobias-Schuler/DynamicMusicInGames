using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVisualizer : MonoBehaviour {

    public Text audioTimer;
    public GameObject audioBlock;
    public float unitsPerSecond = 50;
    double audioStartTime;
    double prevAudioTime;

    List<RectTransform> blocks = new List<RectTransform>();

    void Awake () {
        GameObject g = Instantiate(audioBlock, this.transform);
        blocks.Add(g.GetComponent<RectTransform>());
    }

    // Start is called before the first frame update
    void Start () {
        audioStartTime = AudioSettings.dspTime;
        prevAudioTime = audioStartTime;
    }


    // Update is called once per frame
    void Update () {
        double currentAudioTime = AudioSettings.dspTime;
        audioTimer.text = (currentAudioTime - audioStartTime).ToString("F2") + " s";


        foreach(RectTransform t in blocks) {
            //calculate the movement vector to move a block by the specified units per second
            Vector3 movementVector = new Vector3(-unitsPerSecond * (float) (currentAudioTime - prevAudioTime), 0);
            t.localPosition += movementVector;

        }

        prevAudioTime = currentAudioTime;
    }
}
