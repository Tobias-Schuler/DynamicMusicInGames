using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitApplication : MonoBehaviour {

    Button button;
    // Start is called before the first frame update
    void Start () {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(Exit);
    }

    void Exit () {
        Application.Quit();
    }
}
