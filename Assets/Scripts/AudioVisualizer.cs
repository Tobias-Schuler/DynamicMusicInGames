using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVisualizer : MonoBehaviour {


    public float clockX = -600; //x position of the visualizer playhead
    public int startY = 450; //top y position for track 0 to place audio blocks, every further track is placed below
    public int blockHeight = 80;
    public int spacing = 100; //y-space between tracks

    public Text audioTimer;
    public GameObject audioBlock;
    public GameObject fadeBlock;
    public GameObject marker;

    public float unitsPerSecond = 50;
    double audioStartTime;
    double prevAudioTime;

    Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, Color.magenta };

    List<RectTransform> blocks = new List<RectTransform>();
    List<RectTransform> timings = new List<RectTransform>();

    public void SpawnNextAudioTimingLine (int trackNumber, double timing) {
        GameObject g = Instantiate(marker, this.transform);
        float blockPositionX = (float) (unitsPerSecond * (timing - AudioSettings.dspTime));
        g.transform.localPosition += new Vector3(clockX + blockPositionX, startY - (trackNumber * spacing));
        SpriteRenderer s = g.GetComponent<SpriteRenderer>();
        s.color = colors[trackNumber];
        s.sortingOrder = -(trackNumber + 1);

        timings.Add(g.GetComponent<RectTransform>());
    }

    public void SpawnAudioBlock (int trackNumber, string info, double duration, double timing) {
        GameObject g = Instantiate(audioBlock, this.transform); //create new audio block that represents an audio clip

        float blockPositionX = (float) (unitsPerSecond * (timing - AudioSettings.dspTime)); //
        float blockLength = (float) duration * unitsPerSecond;  //length of a block based on its duration

        g.transform.localPosition += new Vector3(clockX + blockPositionX + (blockLength * 0.5f), startY - (trackNumber * spacing));  //spawn position

        Vector2 adjustedSize = new Vector2(blockLength, blockHeight);
        SpriteRenderer s = g.GetComponent<SpriteRenderer>();
        s.size = adjustedSize;       //set sprite render width and height
        s.color = colors[trackNumber];

        //set available text space and text itself
        Text textComponent = g.GetComponentInChildren<Text>();
        textComponent.rectTransform.sizeDelta = adjustedSize;
        textComponent.text = info;

        blocks.Add(g.GetComponent<RectTransform>()); //add transform to list for later access

        SpawnNextAudioTimingLine(trackNumber, timing);
        SpawnNextAudioTimingLine(trackNumber, timing + duration);
    }


    string[] fadeTypes = { "None", "Linear In", "Linear Out", "Log In", "Log Out", "Exp In", "Exp Out" };

    public void SpawnFadeBlock (int trackNumber, FadeType type, double duration, double timing) {
        GameObject g = Instantiate(fadeBlock, this.transform);

        float blockPositionX = (float) (unitsPerSecond * (timing - AudioSettings.dspTime)); //
        float blockLength = (float) duration * unitsPerSecond;

        g.transform.localPosition += new Vector3(clockX + blockPositionX + (blockLength * 0.5f), startY - (trackNumber * spacing));

        Vector2 adjustedSize = new Vector2(blockLength, blockHeight);
        SpriteRenderer s = g.GetComponent<SpriteRenderer>();
        s.size = adjustedSize;

        //set available text space and text itself
        Text textComponent = g.GetComponentInChildren<Text>();
        textComponent.rectTransform.sizeDelta = adjustedSize;
        textComponent.text = "Fade - " + fadeTypes[(int) type];

        blocks.Add(g.GetComponent<RectTransform>()); //add transform to list for later access
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

        Vector3 movementVector = new Vector3(-unitsPerSecond * (float) (currentAudioTime - prevAudioTime), 0);

        for(int i = 0; i < blocks.Count; i++) {
            RectTransform t = blocks[i];
            //calculate the movement vector to move a block by the specified units per second

            t.localPosition += movementVector;

            //clean up when not visible anymore, < -1000 as safeguard as the object is briefly not visible on startup
            if(t.localPosition.x < -1000 && !t.gameObject.GetComponent<SpriteRenderer>().isVisible) {
                blocks.RemoveAt(i--);
                Destroy(t.gameObject);
            }
        }

        for(int i = 0; i < timings.Count; i++) {
            RectTransform t = timings[i];

            t.localPosition += movementVector;

            if(t.localPosition.x < -1000 && !t.gameObject.GetComponent<SpriteRenderer>().isVisible) {
                timings.RemoveAt(i--);
                Destroy(t.gameObject);
            }
        }

        prevAudioTime = currentAudioTime;
    }
}
