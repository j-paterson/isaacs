using UnityEngine;
using System.Collections;

public class CubeCommand : MonoBehaviour {


    /*  This stores the position and rotation data of the Hololens and sends it to the ROSBridgeScript gameObject 
     *  further processing
     * 
     */ 

    public ROSBridgeScript rosScript;
    public TextMesh debug;
    public Vector3 originPosition;
    public Vector3 relativePosition;
    public Quaternion direction;
    


    void Start()
    {
        originPosition = transform.position;
    }


    /* Trash method 
     * 
     */
    void changeColor()
    {
        System.Random rnd = new System.Random();
        Color whateverColor = new Color((float)rnd.NextDouble(), (float)rnd.NextDouble(), (float)rnd.NextDouble());
        MeshRenderer gameObjectRenderer = GetComponent<MeshRenderer>();
        Material newMaterial = new Material(Shader.Find("Diffuse"));
        newMaterial.color = whateverColor;
        gameObjectRenderer.material = newMaterial;

    }

    /* Toggles the canStart to send position and rotation data
     */ 
    void OnSelect()
    {
        GameObject camera = GameObject.Find("Main Camera");
        originPosition = camera.transform.position;
        rosScript.runTurtle();
        
    }

    void Update()
    {
        GameObject camera = GameObject.Find("Main Camera");
        relativePosition = camera.transform.position - originPosition;
        direction = camera.transform.rotation;
    }
}
