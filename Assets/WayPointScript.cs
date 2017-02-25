using UnityEngine;
using System.Collections;

public class WayPointScript : MonoBehaviour {

    /* When button is pressed the main script will place a waypoint. 
     */ 
    Main main; 
    
	void Start () {
	 main = GameObject.Find("GameManager").GetComponent<Main>();

    }
    

    void OnSelect()
    {
       Vector3 placement = transform.position - transform.forward * .5f;
       main.instantiateMarker(placement, new Quaternion(0, 0, 0, 0));
    }

}
