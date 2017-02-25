﻿using ROSBridgeLib;
using ROSBridgeLib.std_msgs;
using ROSBridgeLib.turtlesim;
using System.Collections;
using SimpleJSON;
using UnityEngine;

/**
 * This is a toy example of the Unity-ROS interface talking to the TurtleSim 
 * tutorial (circa Groovy). Note that due to some changes since then this will have
 * to be slightly re-written. This defines the velocity message that we will publish
 * 
 * @author Michael Jenkin, Robert Codd-Downey and Andrew Speers
 * @version 3.0
 **/

public class SimplePublisher: ROSBridgePublisher {
	
	public static string GetMessageTopic() {
		return "simple_coordinates/chatter";
	}  
	
	public static string GetMessageType() {
		return "geometry_msgs/Vector3";
	}

	public static string ToYAMLString(VelocityMsg msg) {
		return msg.ToYAMLString ();
	}
}
