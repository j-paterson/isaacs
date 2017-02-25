using ROSBridgeLib;
using ROSBridgeLib.IsaacsCommand;
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

public class IsaacsPublisher: ROSBridgePublisher {
	
	public static string GetMessageTopic() {
		return "IsaacsCommand/IsaacsChatter";
	}
    
    public static string GetMessageType()
    {
        return "IsaacsCommand/Isaacs";
    }

    public static string ToYAMLString(IsaacsMsg msg)
    {
        return msg.ToYAMLString();
    }
}
