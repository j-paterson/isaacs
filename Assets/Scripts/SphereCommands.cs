using UnityEngine;

public class SphereCommands : MonoBehaviour
{
    // Called by GazeGestureManager when the user performs a Select gesture
    public ROSBridgeScript rosScript;
    public TextMesh debug; 
    void OnSelect()
    {
        // If the sphere has no Rigidbody component, add one to enable physics.
        if (!this.GetComponent<Rigidbody>())
        {
            var rigidbody = this.gameObject.AddComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // Added code

        // Call on make connection.

        rosScript.runTurtle(); 
     
    }

    void Update()
    {
        GameObject camera = GameObject.Find("Main Camera");

        debug.text = camera.transform.position.ToString();
       
    }
}
