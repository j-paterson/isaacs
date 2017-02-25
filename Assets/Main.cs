using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if !UNITY_EDITOR
using Windows.Storage;
using System.Threading.Tasks;
using Windows.Foundation;
using System;

#endif
public class Main : MonoBehaviour {


    /* Main keeps state trackers that determins whether the calibration frame has been placed so that the hololens can perform a 
     * change of coordinate to match up with the coordinate space of any other coordinate system. It also keeps track of the waypoints 
     * that have been placed. SpatialMapping is used to map the environment of the room. ipText is used to keep a list of the ip 
     * addresses (incomplete). SynchronizeScript runs the algorithm to set the rotation matrix and the transformation offset for 
     * coordinate synchronization. Id gives a unique id for each waypoint marker. 
     */

    public Vector3 positionDeviation;
    public Quaternion quaternionDeviation; 
    public TextMesh debugText;
    public TextMesh debugText2;
    public bool isCalibrated;
    public bool inCalibrationMode;
    public bool hasPlacedFrame;
    public List<GameObject> markerList = new List<GameObject>();
    public SpatialMapping mapping;
    public GameObject calibrationFrame;
    string ipText;
    SynchronizeScript synchronizeScript;

    public List<string> ipAddressList = new List<string>();
    int id = 0; 

#if !UNITY_EDITOR
    Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

#endif
    
    /* gray code will only run on the Hololens. uncomment writeToFile(), loadIPAddresses(), storeIPAddresses() 
     * to save IP address data in the local directory of the hololens. The code is incomplete.
     */

    void Start () {
        

        debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
#if !UNITY_EDITOR
       //writeToFile();
       //  loadIPAddresses();
      //  storeIPAddresses(); 
#endif
        synchronizeScript = GameObject.Find("CountDownPanel").GetComponent<SynchronizeScript>();
       
    }

#if !UNITY_EDITOR

    public async void writeToFile() {

          Windows.Storage.StorageFile textFile = await localFolder.CreateFileAsync("test.txt",  CreationCollisionOption.FailIfExists);
       //   await Windows.Storage.FileIO.AppendTextAsync(textFile, "Swift as a shadow\n"); 
        //  ipText = await  Windows.Storage.FileIO.ReadTextAsync(textFile);
          

        await Windows.Storage.FileIO.WriteTextAsync(textFile, "192.168.0.202\n192.168.0.203\n192.168.0.160\n192.168.0.133\n192.168.43.173");
        
    }
#endif
    
    /* checks if synchronizeScript is initialized yet 
     */
    void Update () {
        if (synchronizeScript == null && GameObject.Find("CounDownPanel") != null)
        {
            synchronizeScript = GameObject.Find("CountDownPanel").GetComponent<SynchronizeScript>();
        }
    }

    /* creates a marker using the followMe voice command system 
     */ 
    void OnFollowMe()
    {
        GameObject camera = GameObject.Find("Main Camera");
        instantiateMarker(camera.transform.position, new Quaternion(0, 0, 0, 0));
    }

    /* Unfinished process of reading the local directory textFile and loading to it
     */ 
#if !UNITY_EDITOR

    public async void loadIPAddresses()
    {
          Windows.Storage.StorageFile textFile = await localFolder.GetFileAsync("test.txt");
          ipText = await  Windows.Storage.FileIO.ReadTextAsync(textFile);


        
        string[] ipList = ipText.Split(new char[] { '\n' });
        ipAddressList = new List<string>(); 
        for (int i = 0; i < ipList.Length; i++)
        {
            ipAddressList.Add(ipList[i]);
        }
    }

    public async void storeIPAddresses()
    {
        string loadText = ""; 
        foreach (string ip in ipAddressList)
        {
            loadText += ip + "\n";
        }

        Windows.Storage.StorageFile textFile = await localFolder.GetFileAsync("test.txt");
        await Windows.Storage.FileIO.WriteTextAsync(textFile, loadText);
    
    }
#endif
    /* When the hololens recognizes a doubleTap, a waypoint will be placed 1 meter in front of where
     * the hololens is starting at.
     */
    void OnDoubleTap()
    {
        GameObject camera = GameObject.Find("Main Camera");

        if (inCalibrationMode && !hasPlacedFrame)
        {
            hasPlacedFrame = true;
            calibrationFrame = Instantiate(Resources.Load("CalibrationFrame") as GameObject, camera.transform.position 
               + camera.transform.forward, new Quaternion(0, 0, 0, 0)) as GameObject;
            calibrationFrame.name = "CalibrationFrame";
            MovementScript movementScript = calibrationFrame.GetComponent<MovementScript>();
           movementScript.debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
            movementScript.ignoreSurfaceMesh = true; 
           

        } else
        {
            GameObject cursor = GameObject.Find("Cursor");
            float distanceToCursor = (cursor.transform.position - camera.transform.forward * .2f - camera.transform.position).magnitude;
            float minDistance = Mathf.Min(distanceToCursor, 1); 
            instantiateMarker(camera.transform.position + minDistance * camera.transform.forward, new Quaternion(0, 0, 0, 0));
      
        }
        
    }

    /* Creates a new waypoint
     */
      
    public void instantiateMarker(Vector3 position, Quaternion quaternion)
    { 
        GameObject marker = Instantiate(Resources.Load("Marker") as GameObject,
        position, quaternion) as GameObject;
        marker.name = "Marker" +  id;
        id++;
        markerList.Add(marker);
  
            
        MovementScript movementScript = marker.GetComponent<MovementScript>();
        movementScript.debugText = GameObject.Find("Debug").GetComponent<TextMesh>();
        debugText.text = "Added Marker!";
        renumberFlags(); 
        debugText.text = " count: " + markerList.Count;
        for (int i = 0; i < markerList.Count; i++)
        {
            debugText.text += " " + markerList[i].name;
        }
    }

    public void loadSpatialMapping()
    {
        mapping.SetMappingEnabled(true);
    }

    public void endSpatialMapping()
    {
        mapping.SetMappingEnabled(false);
    }

    /* Each waypoint flag has number on it. This determines the order in which the drone flies. Number one first etc. 
     * Renumberflags updates the sequence number on each flag when a waypoint is deleted, promoted or demoted. 
     */

    public void renumberFlags()
    {
        int i = 1;
        foreach (GameObject marker in markerList)
        {
            TextMesh flagText = GameObject.Find(marker.name + "/Flag").GetComponent<TextMesh>();
            flagText.text = "" + i;
            i++;
        }
    }

    public void deleteWaypointMarker(GameObject marker)
    {
        markerList.Remove(marker);
        Destroy(marker);
        renumberFlags();
        debugText.text = " count: " + markerList.Count;
        for (int i = 0; i < markerList.Count; i++)
        {
            debugText.text += " " + markerList[i].name;
        }
    }

    public void moveUpWayPoint(GameObject marker)
    {
        int index = markerList.IndexOf(marker);
        int indexNext = (index + 1) % markerList.Count;
        markerList[index] = markerList[indexNext];
        markerList[indexNext] = marker;
        renumberFlags();
        debugText.text = index + " " + indexNext + " list index: " + markerList[index].name +
        " list indexNext: " + markerList[indexNext].name;

    }

    public void moveDownWayPoint(GameObject marker)
    {
        int index = markerList.IndexOf(marker);
        int indexNext;
        if (index == 0)
        {
            indexNext = markerList.Count - 1;
        }
        else
        {
            indexNext = (index - 1) % markerList.Count;

        }
        markerList[index] = markerList[indexNext];
        markerList[indexNext] = marker;
        renumberFlags();
        debugText.text += " " + index + " " + indexNext + " list index: " + markerList[index].name +
        " list indexNext: " + markerList[indexNext].name;

    }

    /* Helper Methods below for coordinate synchronization
     */

    public static float[] SolveLinearEquations(float[][] rows)
    {

        int length = rows[0].Length;

        for (int i = 0; i < rows.Length - 1; i++)
        {
            if (rows[i][i] == 0 && !Swap(rows, i, i))
            {
                return null;
            }

            for (int j = i; j < rows.Length; j++)
            {
                float[] d = new float[length];
                for (int x = 0; x < length; x++)
                {
                    d[x] = rows[j][x];
                    if (rows[j][i] != 0)
                    {
                        d[x] = d[x] / rows[j][i];
                    }
                }
                rows[j] = d;
            }

            for (int y = i + 1; y < rows.Length; y++)
            {
                float[] f = new float[length];
                for (int g = 0; g < length; g++)
                {
                    f[g] = rows[y][g];
                    if (rows[y][i] != 0)
                    {
                        f[g] = f[g] - rows[i][g];
                    }

                }
                rows[y] = f;
            }
        }

        return CalculateResult(rows);
    }

    private static bool Swap(float[][] rows, int row, int column)
    {
        bool swapped = false;
        for (int z = rows.Length - 1; z > row; z--)
        {
            if (rows[z][row] != 0)
            {
                float[] temp = new float[rows[0].Length];
                temp = rows[z];
                rows[z] = rows[column];
                rows[column] = temp;
                swapped = true;
            }
        }

        return swapped;
    }
    private static float[] CalculateResult(float[][] rows)
    {
        float val = 0;
        int length = rows[0].Length;
        float[] result = new float[rows.Length];
        for (int i = rows.Length - 1; i >= 0; i--)
        {
            val = rows[i][length - 1];
            for (int x = length - 2; x > i - 1; x--)
            {
                val -= rows[i][x] * result[x];
            }
            result[i] = val / rows[i][i];

            if (!IsValidResult(result[i]))
            {
                return null;
            }
        }
        return result;
    }

    private static bool IsValidResult(float result)
    {
        return result.ToString() != "NaN" || !result.ToString().Contains("Infinity");
    }

    public static float[][] convertToJaggedArray(float[,] multiArray)
    {
        int firstElement = multiArray.GetLength(0);
        int secondElement = multiArray.GetLength(1);

        float[][] jaggedArray = new float[firstElement][];

        for (int c = 0; c < firstElement; c++)
        {
            jaggedArray[c] = new float[secondElement];
            for (int r = 0; r < secondElement; r++)
            {
                jaggedArray[c][r] = multiArray[c, r];
            }
        }
        return jaggedArray;
    }

    /* Changes coordinate from hololens space to VICON space, remember that vicon swaps the y and z axis
     */
      
    public static Vector3 changeToVicon(float[,] rotationMatrix, Vector3 transformation, Vector3 original)
    {

        Vector3 shifted = original - transformation; 
        float viconX = rotationMatrix[0, 0] * shifted.x + rotationMatrix[0, 1] * shifted.y + rotationMatrix[0, 2] * shifted.z;
        float viconY = rotationMatrix[1, 0] * shifted.x + rotationMatrix[1, 1] * shifted.y + rotationMatrix[1, 2] * shifted.z;
        float viconZ = rotationMatrix[2, 0] * shifted.x + rotationMatrix[2, 1] * shifted.y + rotationMatrix[2, 2] * shifted.z;
        return new Vector3(viconX, viconZ, viconY); 
    }


}
