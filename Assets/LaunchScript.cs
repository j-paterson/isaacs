using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.IsaacsCommand;

public class LaunchScript : MonoBehaviour {

    /* Sends a ros message command to launch the drone. The command type number is 2. 
     */ 

    Main main;
    TextMesh debugText;

    public  ROSBridgeWebSocketConnection ros;

    void Start () {
        main = GameObject.Find("GameManager").GetComponent<Main>();
        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;
        

    }

    void Update () {
        if (ros == null ) { 
            ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;   
        } 
    }

    void OnSelect()
    {
        debugText.text = "start launch";
        IsaacsMsg launchMsg = new IsaacsMsg(2, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        ros.Publish(IsaacsPublisher.GetMessageTopic(), launchMsg);
        debugText.text = "launch end";
    }
}
