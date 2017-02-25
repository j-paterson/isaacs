using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.IsaacsCommand;

public class ArmScript : MonoBehaviour {

    /* Sends a ros message command to arm the drone. The command type number is 9. 
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
        if (ros == null)
        {
            ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;
        }
    }


    void OnSelect()
    {
        debugText.text = "start arm";
        IsaacsMsg launchMsg = new IsaacsMsg(9, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        ros.Publish(IsaacsPublisher.GetMessageTopic(), launchMsg);
        debugText.text = "arm end";
    }
}
