using UnityEngine;
using System.Collections;

public class MovementScript: MonoBehaviour {

    /* Objects attached with this script will move when the user focuses on the object and drags with their hand. 
     * Everytime the user's hand moves, the OnManipulationUpdate will be called with the new position of the hand. 
     * Translation sensitivity can be adjusted. The canMove variable can be toggled to disable/enable movement. 
     * IgnoreSurfaceMesh means that the object can be placed beyond the boundaries of the walls and floors. EnableLookAt 
     * means that the object must always stare at the hololens.
     */ 

    private Vector3 manipulationPreviousPosition;
    float TranslationSensitivity = 1.00f;

    public TextMesh debugText;

    bool canMove = true;
    public bool ignoreSurfaceMesh;
    public bool enableLookAt = false;

    private Vector3 cameraPreviousPosition;
    private Vector3 cameraRightVector;
    public bool disableMovement; 

    void Start () {
    }

    /* Initializes the position of the camera when the hand first starts dragging. 
     */ 

    void OnManipulationStart(Vector3 position)
    {
        GameObject camera = GameObject.Find("Main Camera");

        manipulationPreviousPosition = position;
        cameraPreviousPosition = camera.transform.position;
        cameraRightVector = camera.transform.right;
    }

    /* Moves the object by computing and moveVector and rotating the object to stare at the hololens
     */ 
    void OnManipulationUpdate(Vector3 position)
    {
        Vector3 moveVector = position - manipulationPreviousPosition;
        manipulationPreviousPosition = position;

        Vector3 newPosition = transform.position + moveVector * TranslationSensitivity;
        if (ignoreSurfaceMesh || canMoveThere(newPosition))
        {
            if (enableLookAt)
            {
                transform.LookAt(GameObject.Find("Main Camera").transform.position, Vector3.up);
                if (gameObject.name != "Menu")
                {
                    transform.Rotate(new Vector3(0, 90, 0));
                } 
            }
            if (!disableMovement)
            {
                transform.position = newPosition;
            } 
        }
    }
    
    /* Used to compute how much rotation should happen
     */ 
    float CalculateRotationFactor(Vector3 moveVector)
    {
        GameObject camera = GameObject.Find("Main Camera");
        Vector3 cameraMoveVector = camera.transform.position - cameraPreviousPosition;
        cameraPreviousPosition = camera.transform.position;
        Vector3 rotationMoveVector = moveVector - cameraMoveVector;
        return Vector3.Dot(rotationMoveVector, cameraRightVector);

    }

    /* Used the check if the gameobject will collide with the wall or floor, which is any objects 
     * with layer of type SpatialMapping.PhysicsRaycastMask
     */
    bool canMoveThere(Vector3 position)
    {
        SphereCollider sphere = GetComponent<SphereCollider>(); 
        Collider[] collider = Physics.OverlapSphere(position, sphere.radius*1.1f, SpatialMapping.PhysicsRaycastMask);
        if (collider.Length > 0)
        {
            return false;
        } else
        {
            return true; 
        }
    }
}
