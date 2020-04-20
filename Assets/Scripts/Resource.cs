using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Transform> spawnLocations;
    public  GameObject objectsToSpawned;
    public  double spawnProb;
    public  float timeSpan;
    private float prevTime;
    private  List<GameObject> spawnedObjects = new List<GameObject>();
    private  Charge chargeStation;

    void Start(){
        prevTime = Time.time;
        foreach(Transform trans in spawnLocations){
            if(Random.Range(0.0f,1.0f)>spawnProb){
                continue;
            }
            GameObject go = Instantiate(objectsToSpawned) as GameObject;
            go.transform.position = trans.position; 
            spawnedObjects.Add(go);   
        }
    }

    void Update() {
        
        if( Time.time-prevTime > timeSpan && spawnedObjects!=null){
            prevTime = Time.time;
            chargeStation = spawnedObjects[0].GetComponent<Charge>();
            chargeStation.Broken();
            spawnedObjects.RemoveAt(0);    
        }
        
    }

}
 
