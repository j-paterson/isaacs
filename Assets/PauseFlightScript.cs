using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.IsaacsCommand;

public class PauseFlightScript : MonoBehaviour {


    /* Sends a ros message command to pause the drone. The command type number is 7. 
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
        debugText.text = "pause launch";
        IsaacsMsg launchMsg = new IsaacsMsg(7, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        ros.Publish(IsaacsPublisher.GetMessageTopic(), launchMsg);
        debugText.text = "pause end";
    }
}
