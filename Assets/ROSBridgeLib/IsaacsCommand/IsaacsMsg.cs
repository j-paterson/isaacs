using System.Collections;
using System.Text;
using SimpleJSON;

namespace ROSBridgeLib {
	namespace IsaacsCommand {
		public class IsaacsMsg : ROSBridgeMsg {
			private double _commandType;
            private double _wayPointID; 
            private double _receiverIPAddress;
            private double _positionX;
            private double _positionY;
            private double _positionZ;
            private double _quaternionX;
            private double _quaternionY;
            private double _quaternionZ;
            private double _quaternionW; 


            public IsaacsMsg(JSONNode msg) {
                _commandType = double.Parse(msg["commandType"]); 
                _wayPointID = double.Parse(msg["wayPointID"]);
                _receiverIPAddress = double.Parse(msg["receiverIPAddress"]);
                _positionX = double.Parse(msg["positionX"]);
                _positionY = double.Parse(msg["positionY"]);
                _positionZ = double.Parse(msg["positionZ"]);
                _quaternionX = double.Parse(msg["quaternionX"]);
                _quaternionY = double.Parse(msg["quaternionY"]);
                _quaternionZ = double.Parse(msg["quaternionZ"]);
                _quaternionW = double.Parse(msg["quaternionW"]);

            }
            
            public IsaacsMsg(double commandType, double wayPointID, double receiverIPAddress, double positionX, 
                double positionY, double positionZ, double quaternionX, double quaternionY, double quaternionZ, double quaternionW)
            {
                _commandType = commandType;
                _wayPointID = wayPointID;
                _receiverIPAddress = receiverIPAddress;
                _positionX = positionX;
                _positionY = positionY;
                _positionZ = positionZ;
                _quaternionX = quaternionX;
                _quaternionY = quaternionY;
                _quaternionZ = quaternionZ;
                _quaternionW = quaternionW;
            }



            public static string GetMessageType() {
				return "IsaacsCommand/Isaacs";
			}
			
		
			
			public override string ToString() {
				return "Isaacs [commandType=" + _commandType + ", wayPointID=" + _wayPointID + ", receiverIPAddress=" + _receiverIPAddress + 
                    ", positionX=" + _positionX + ", positionY=" + _positionY + ", positionZ=" + _positionZ + ", quaternionX=" + _quaternionX +
                    ", quaternionY=" + _quaternionY + ", quaternionZ=" + _quaternionZ + ", quaternionW=" + _quaternionW + "]";
			}
			
			public override string ToYAMLString() {
                return "{\"commandType\" : " + _commandType + ", \"wayPointID\" : " + _wayPointID + ", \"receiverIPAddress\" : " + _receiverIPAddress +
                    ", \"positionX\" : " + _positionX + ", \"positionY\" : " + _positionY + ", \"positionZ\" : " + _positionZ + ", \"quaternionX\" : " + _quaternionX +
                    ", \"quaternionY\" : " + _quaternionY + ", \"quaternionZ\" : " + _quaternionZ + ", \"quaternionW\" : " + _quaternionW + "}";
            }
		}
	}
}