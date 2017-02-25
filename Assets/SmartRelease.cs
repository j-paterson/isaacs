using UnityEngine;
using System.Collections;

public class SmartRelease : MonoBehaviour {


    int maxQueueSize = 25;
    float deviationThreshold = .015f;
    Queue positionHistory = new Queue();
    public TextMesh text;
    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


    public void AddToQueue(Vector3 pos)
    {
        if (positionHistory.Count >= maxQueueSize)
        {
            positionHistory.Dequeue();
        }
        positionHistory.Enqueue(pos);
        checkRelease(); 
    }
    
    public void checkRelease()
    {
        float deviation = CalculateAverageDeviation();
        text.text = "DEV = " + deviation;

        if (positionHistory.Count >= maxQueueSize && deviation < deviationThreshold)
        {
            
            clearQueue();
        }
    }
    public float CalculateAverageDeviation()
    {
        Vector3 sumTotal = new Vector3();
        foreach(Vector3 pos in positionHistory)
        {
            sumTotal += pos;
        }
        Vector3 averageVector = sumTotal / positionHistory.Count;
        float deviationTotal = 0; 
        foreach(Vector3 pos in positionHistory)
        {
            deviationTotal += (pos - averageVector).magnitude; 
        }
        return deviationTotal / positionHistory.Count;
    }
    public void clearQueue()
    {
        positionHistory.Clear();
    }
}
