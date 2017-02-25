using UnityEngine;
using System.Collections;

public class LookAtScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cameraPosition = GameObject.Find("Main Camera").transform.position;
        Vector3 levelPoint = new Vector3(cameraPosition.x, transform.position.y, cameraPosition.z); 
        transform.LookAt(levelPoint, Vector3.up);
    }
}
