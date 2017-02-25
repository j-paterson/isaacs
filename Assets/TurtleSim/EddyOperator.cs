using UnityEngine;
using System.Collections;
using ROSBridgeLib;
using System.Reflection;
using System;
using ROSBridgeLib.geometry_msgs;
using System.Collections.Generic;


/**
 * Talk to Eddy
 * 
 * @author Michael Jenkin, Robert Codd-Downey and Andrew Speers
 * @version 3.0
 **/

public class EddyOperator : MonoBehaviour
{


    public GameObject marker;

    public Queue<destinationpoint> markers = new Queue<destinationpoint>();
    public struct destinationpoint
    {
        public GameObject marker;
        public float x;
        public float z;
    }

    void Start()
    {

    }

    void OnApplicationQuit()
    {

    }

    public void removeMarker()
    {
        destinationpoint trash = markers.Dequeue();
        Destroy(trash.marker);
    }

    void Update()
    {
        /**
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log(ray);
            GameObject clone = Instantiate(marker);

            clone.transform.position = new Vector3(ray.x, 2, ray.z);
            destinationpoint point = new destinationpoint();
            point.marker = clone;
            point.x = ray.x;
            point.z = ray.z;
            markers.Enqueue(point);
        }
        **/
    }

}