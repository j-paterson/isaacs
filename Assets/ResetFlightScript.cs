using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.IsaacsCommand;

public class ResetFlightScript : MonoBehaviour {

    /* Sends a ros message command to reset the flight path of the drone. The command type number is 8. 
    */
    Main main;
    TextMesh debugText;
    public ROSBridgeWebSocketConnection ros;

    void Start()
    {
        main = GameObject.Find("GameManager").GetComponent<Main>();
        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;


    }

    void Update()
    {
        if (debugText == null)
        {
            debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        }

        if (ros == null)
        {
            ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;
        }
    }


    void OnSelect()
    {
        debugText.text = "reset start";
        IsaacsMsg launchMsg = new IsaacsMsg(8, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        for (int i = 0; i < main.markerList.Count; i++)
        {
            Color blue = new Color(0, 0, 1);
            var material = main.markerList[i].GetComponent<Renderer>().material;
            material.color = blue;

        }
        ros.Publish(IsaacsPublisher.GetMessageTopic(), launchMsg);
        debugText.text = "reset end";
    }
}
