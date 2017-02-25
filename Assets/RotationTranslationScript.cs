using UnityEngine;
using System.Collections;

public class RotationTranslationScript : MonoBehaviour {

    private Vector3 manipulationPreviousPosition;
    private Vector3 cameraPreviousPosition;
    private Vector3 cameraRightVector; 
    float TranslationSensitivity = 1.75f;
    float RotationSensitivity = 225.0f; 

    private float rotationFactor;

    float boundarySize = .01f;
    Vector3 oldPosition;
    // Use this for initialization
    bool canMove = true;
    public bool ignoreSurfaceMesh;
    public GameObject calibrationFrame; 
    void Start()
    {

        oldPosition = transform.position;
        calibrationFrame = GameObject.Find("CalibrationFrame");

    }

    // Update is called once per frame
    void Update()
    {

    }
    

    void OnManipulationStart(Vector3 position)
    {
        GameObject camera = GameObject.Find("Main Camera");

        manipulationPreviousPosition = position;
        cameraPreviousPosition = camera.transform.position;
        cameraRightVector = camera.transform.right;
       calibrationFrame.GetComponent<MovementScript>().disableMovement = true;

    }

    void OnManipulationCompleted()
    {
        calibrationFrame.GetComponent<MovementScript>().disableMovement = false;
        GameObject.Find("Debug").GetComponent<TextMesh>().text = "Disabled rotation";


    }
    void OnManipulationUpdate(Vector3 position)
    {
        Vector3 moveVector = position - manipulationPreviousPosition;
        manipulationPreviousPosition = position;

        float rotationFactor = CalculateRotationFactor(moveVector);
        
        calibrationFrame.transform.Rotate(new Vector3(0, -rotationFactor * RotationSensitivity, 0));
        GameObject.Find("Debug").GetComponent<TextMesh>().text = "" + rotationFactor * RotationSensitivity;
        
    }

    float CalculateRotationFactor(Vector3 moveVector)
    {
        GameObject camera = GameObject.Find("Main Camera");
        Vector3 cameraMoveVector = camera.transform.position - cameraPreviousPosition;
        cameraPreviousPosition = camera.transform.position;
        Vector3 rotationMoveVector = moveVector - cameraMoveVector;
        return Vector3.Dot(rotationMoveVector, cameraRightVector);

    }


    bool canMoveThere(Vector3 position)
    {
        Collider[] collider = Physics.OverlapSphere(position, transform.localScale.x, SpatialMapping.PhysicsRaycastMask);
        if (collider.Length > 0)
        {

            return false;
        }
        else
        {
            return true;

        }

    }



}
