using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.IsaacsCommand;

public class StartFlightScript : MonoBehaviour {


    /* ISAACS command message is always in the form of:
     * {
     *  commandType, 
     *  waypoint sequence number, 
     *  IP address of hololens, 
     *  position_x, 
     *  position_y, 
     *  position_z, 
     *  quaternion_x, 
     *  quaternion_y, 
     *  quaternion_z, 
     *  quaternion_w
     * } 
     * NOTE: IP address is not sent yet, because we are only focused on one-to-one communication as of now. 
     */

    /* This script prepares the waypoint message commands and passes the message to the RosBridge ros class
     * so it can stream it to the remote ros system. When button is selected the canDeploy will be true. After 
     * all waypoint messages have been deployed the canDeploy will be reset to false. Proceed is used to buffer 
     * messages so they get sent every .2 seconds. During the deploy message stage, the script extracts the
     * waypoint list from the main class and sends them one by one using the number 5 commandtype. Deploynumber 
     * cooresponds to the sequence of the waypoint. When all waypoint messages have been deployed, the script
     * will send a start flight messages of commandtype 6. 
     */

    Main main;
    TextMesh debugText;
    public ROSBridgeWebSocketConnection ros;
    ROSBridgeScript rosmanager;
    SynchronizeScript synchronizeScript;
    bool proceed = true;
    public bool canDeploy;
    int deployNumber;
    void Start()
    {
        main = GameObject.Find("GameManager").GetComponent<Main>();
        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;
        rosmanager = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>();
        synchronizeScript = GameObject.Find("CountDownPanel").GetComponent<SynchronizeScript>();
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

        if (synchronizeScript == null && GameObject.Find("CounDownPanel") != null)
        {
            synchronizeScript = GameObject.Find("CountDownPanel").GetComponent<SynchronizeScript>();
        }

        if (main.markerList.Count > 0 && deployNumber == main.markerList.Count && proceed)
        {
            IsaacsMsg goMessage = new IsaacsMsg(6, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            ros.Publish(IsaacsPublisher.GetMessageTopic(), goMessage);
            deployNumber = 0;
            canDeploy = false; 
        }

        if (deployNumber < main.markerList.Count && canDeploy && proceed)
        {

            debugText.text = synchronizeScript + "\n";
            proceed = false; 
            Vector3 calibratedPosition = Main.changeToVicon(synchronizeScript.rotationMatrix, synchronizeScript.transformationVector, main.markerList[deployNumber].transform.position);
            debugText.text = calibratedPosition.ToString();
            Quaternion calibratedQuaternion = main.markerList[deployNumber].transform.rotation * Quaternion.Inverse(main.quaternionDeviation);
            IsaacsMsg setWayPointMessage = new IsaacsMsg(5, deployNumber, 0, calibratedPosition.x, calibratedPosition.y, calibratedPosition.z, calibratedQuaternion.x,
                calibratedQuaternion.y, calibratedQuaternion.z, calibratedQuaternion.w);
            ros.Publish(IsaacsPublisher.GetMessageTopic(), setWayPointMessage);
            StartCoroutine(wait(.2f));
            deployNumber++;
            debugText.text += "SENT POINT! " + deployNumber;
        }
    }


    void OnSelect()
    {
        if (main.isCalibrated) {
            debugText.text = "candeploy! " + proceed;
            canDeploy = true;
        }

    }
    /* Used to buffer the messages being sent.
     */ 

    IEnumerator wait(float seconds)
    {

        yield return new WaitForSeconds(seconds);
        proceed = true;

    }


}
