using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using ROSBridgeLib.IsaacsCommand;

public class SynchronizeScript : MonoBehaviour {


    /* This script is attached to the Start sync countdown panel. When selected, you must double tap to put a 
     * calibration frame and press End sync to complete calibration. When hasPlacedFrame is true rotationMatrix
     * and transformation vector will be calculated and will be used during the change of coordinates. OnSelect
     * also sends a message to ros to initiate the yaw calibration for the drone. 
     */ 

    Main main;
    TextMesh debugText;
    TextMesh calibrateText;
    public ROSBridgeWebSocketConnection ros;
    

    public float[,] rotationMatrix;
    public Vector3 transformationVector;
    void Start () {
        main = GameObject.Find("GameManager").GetComponent<Main>();
        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        calibrateText = GameObject.Find("CalibratePanel").GetComponent<TextMesh>();
        ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;

    }

    void Update () {
        if (ros == null)
        {
            ros = GameObject.Find("ROSBridge").GetComponent<ROSBridgeScript>().ros;
        }
    }
    
    void OnSelect()
    {
        IsaacsMsg calibrateMsg = new IsaacsMsg(10, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        ros.Publish(IsaacsPublisher.GetMessageTopic(), calibrateMsg);


        if (main.inCalibrationMode && main.hasPlacedFrame)
        {
            main.inCalibrationMode = false;
            main.hasPlacedFrame = false; 
            calibrateText.text = "Start Sync";

            GameObject calibrationFrame = main.calibrationFrame;

            GameObject rightAxis = GameObject.Find("RightAxis");
            GameObject topAxis = GameObject.Find("TopAxis");
            GameObject frontAxis = GameObject.Find("FrontAxis");

            Vector3 viconOrigin = calibrationFrame.transform.position; //cooresponds to vicon space (0, 0, 0)
            Vector3 viconX = viconOrigin + rightAxis.transform.rotation * transform.up; //cooresponds to vicon space (1, 0, 0)
            Vector3 viconY = viconOrigin + frontAxis.transform.rotation * transform.up; //cooresponds to vicon space (0, 0, 1)
            Vector3 viconZ = viconOrigin + topAxis.transform.rotation * transform.up; //cooresponds to vicon space (0, 1, 0)

            Vector3 s_viconX = viconX - viconOrigin;
            Vector3 s_viconY = viconY - viconOrigin;
            Vector3 s_viconZ = viconZ - viconOrigin;

            /*
             * Solve linear system x_1 = r*( x_0 - t0) 
             */ 

            float[] x1 = { 1, 0, 0, 0, 0, 1, 0, 1, 0 };
            string coord1 = "";
            foreach (float value in x1)
            {
                coord1 += " " + value;
            }

            float[,] r = { {s_viconX.x, s_viconX.y, s_viconX.z, 0       , 0       , 0       , 0       , 0       , 0       , x1[0]},
                       {0       , 0       , 0       , s_viconX.x, s_viconX.y, s_viconX.z, 0       , 0       , 0       , x1[1]},
                       {0       , 0       , 0       , 0       , 0       , 0       , s_viconX.x, s_viconX.y, s_viconX.z, x1[2]},
                       {s_viconY.x, s_viconY.y, s_viconY.z, 0       , 0       , 0       , 0       , 0       , 0       , x1[3]},
                       {0       , 0       , 0       , s_viconY.x, s_viconY.y, s_viconY.z, 0       , 0       , 0       , x1[4]},
                       {0       , 0       , 0       , 0       , 0       , 0       , s_viconY.x, s_viconY.y, s_viconY.z, x1[5]},
                       {s_viconZ.x, s_viconZ.y, s_viconZ.z, 0       , 0       , 0       , 0       , 0       , 0       , x1[6]},
                       {0       , 0       , 0       , s_viconZ.x, s_viconZ.y, s_viconZ.z, 0       , 0       , 0       , x1[7]},
                       {0       , 0       , 0       , 0       , 0       , 0       , s_viconZ.x, s_viconZ.y, s_viconZ.z, x1[8]}};
            float[][] multi_r = Main.convertToJaggedArray(r);
            float[] solution = Main.SolveLinearEquations(multi_r);
            rotationMatrix = new float[,]{ {solution[0], solution[1], solution[2]},
                                    {solution[3], solution[4], solution[5] },
                                    {solution[6], solution[7], solution[8]}};
            Destroy(calibrationFrame);
            main.isCalibrated = true;

        } else
        {
            calibrateText.text = "End Sync";
            main.inCalibrationMode = true;
        }
       
    }
}
