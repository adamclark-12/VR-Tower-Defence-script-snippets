using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDictionary : MonoBehaviour {

    public Dictionary<string, GameObject> myDictionary;
    public GameObject Enemy1, Enemy2, Enemy3, Enemy4, Boss;

	// Use this for initialization
	void Start () {
        myDictionary = new Dictionary<string, GameObject>();
        myDictionary.Add("Enemy1", Enemy1);
        myDictionary.Add("Enemy2", Enemy2);
        myDictionary.Add("Enemy3", Enemy3);
        myDictionary.Add("Enemy4", Enemy4);
        myDictionary.Add("Boss", Boss);
    }
	
	// Update is called once per frame
	void Update () {

        Debug.Log(myDictionary["Enemy2"].name);
		
	}
}
