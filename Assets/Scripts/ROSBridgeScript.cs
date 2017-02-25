using UnityEngine;
using ROSBridgeLib;
using ROSBridgeLib.geometry_msgs;


/* Used to handle networking streaming
 * 
 */ 

public class ROSBridgeScript : MonoBehaviour {



    /*cubeBoy references the CubeCommand script under Menu/UtilitiesBoard/ResetPanel
     * responsible for providing location of the Hololens 
     */

    public ROSBridgeWebSocketConnection ros;
    public bool canStart;
    public CubeCommand cubeBoy;
    public TextMesh debugText;
    public bool finish;
    

   /* Start method initializes the websocketConnection client. Simple publisher 
    * and QuaternionPublisher are used for the cube demo.IsaaacsPublisher is
    *  used for sending commands to HoloClient on the drone ROS system. 
    * Turtle1ServiceReponse is for receiving callback messages from the ROS server. 
    *
    */

    void Start () {
        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        debugText.text = "Starting Connection"; 
        ros = new  ROSBridgeWebSocketConnection("10.142.32.242", 9090);
        ros.AddPublisher(typeof(SimplePublisher));
        ros.AddPublisher(typeof(QuaternionPublisher));
        ros.AddPublisher(typeof(IsaacsPublisher));
        ros.AddServiceResponse(typeof(Turtle1ServiceResponse));
        ros.Connect();
        finish = true; 
    }

    public void runTurtle()
    {
        
        canStart = true; 
    }

    void OnApplicationQuit()
    {
        if (ros != null)
            ros.Disconnect();
    }

    

    /* canStart is toggled when we press the recenter button. Dictates whether or not to send data 
     * via the simpleCoordinates topic on the remote ros system
     * The update method sends the position and the quaternion of the hololens using Vector3Msg and TwistMsg
     * ros publish simplies converts message object to a data string and sends it 
     */


    void Update () {
        if (canStart)
        {
            Vector3 vector = cubeBoy.relativePosition *10;
            Vector3Msg positionMsg = new Vector3Msg(vector.x, vector.y, vector.z);
            Quaternion rotation = cubeBoy.direction;
            TwistMsg rotationMsg = new TwistMsg(new Vector3Msg(rotation.x, rotation.y, rotation.z), new Vector3Msg(rotation.w, 0.0, 0.0));
            ros.Publish(SimplePublisher.GetMessageTopic(), positionMsg);
            ros.Publish(QuaternionPublisher.GetMessageTopic(), rotationMsg);
            ros.Render();
        }
    }
}