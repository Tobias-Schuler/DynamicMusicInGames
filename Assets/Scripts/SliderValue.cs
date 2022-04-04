using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour {

    public Slider slider;
    Text textComponent;

    public string defaultMessage;
    int value = 0;

    // Start is called before the first frame update
    void Start () {
        value = (int) slider.value;
        textComponent = this.GetComponent<Text>();
        textComponent.text = defaultMessage + value;
        slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public void OnValueChanged () {
        value = (int) slider.value;
        textComponent.text = defaultMessage + value;
    }

    // Update is called once per frame
    void Update () {

    }


}
