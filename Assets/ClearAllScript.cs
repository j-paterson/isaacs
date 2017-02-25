using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClearAllScript : MonoBehaviour {

    // Use this for initialization

    Main main;
    TextMesh debugText;
    StartFlightScript startFlight; 
	void Start () {
        main = GameObject.Find("GameManager").GetComponent<Main>();
        startFlight = GameObject.Find("StartFlightPanel").GetComponent<StartFlightScript>();
        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update () {
        if (debugText == null)
        {
            debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        }

        if (startFlight == null)
        {
            startFlight = GameObject.Find("StartFlightPanel").GetComponent<StartFlightScript>();

        }
    }

    void OnSelect()
    {
     
        debugText.text = "Cleared! " + main.markerList.Count;

        int size = main.markerList.Count;

        for (int i = 0; i < size; i++)
        {
            GameObject marker = main.markerList[0];
            main.markerList.RemoveAt(0);
            Destroy(marker);
        }
        debugText.text = "Cleared! " + main.markerList.Count;
        
       
    }
}
