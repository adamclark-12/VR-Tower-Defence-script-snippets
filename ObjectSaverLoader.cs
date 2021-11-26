﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ObjectSaverLoader : MonoBehaviour {

    public static List<GameObject> existingObjects;
    public static WaveWrapper Waves;
    //public static UnityEngine.UI.Text dText;

    // Use this for initialization
    static void Start ()
    {
        existingObjects = new List<GameObject>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public static void SaveObjects(List<Wave> o)
    {
        List<Wave> output = new List<Wave>();
        foreach (Wave x in o)
        {
           output.Add(x);
        }
        FileStream f = new FileStream(Application.dataPath + "/StreamingAssets/XML/Waves.xml", FileMode.Create);
        XmlSerializer serial = new XmlSerializer(typeof(WaveWrapper));

        WaveWrapper wrapper = new WaveWrapper();
        wrapper.WaveList = output;
        serial.Serialize(f, wrapper);
        f.Close();
    } 

    public static List<Wave> LoadCards()
    {
        Debug.Log("LOADING Waves");
        List<Wave> ret = new List<Wave>();
        //dText = GameObject.Find("DebugText").GetComponent<UnityEngine.UI.Text>();
        try
        {
            FileStream f = new FileStream(Application.dataPath + "/StreamingAssets/XML/Waves.xml", FileMode.Open);
            //FileStream f = new FileStream(Resources.Load ( "/StreamingAssets/XML/Waves.xml")) as FileStream ;

            XmlSerializer serial = new XmlSerializer(typeof(WaveWrapper));


            Waves = serial.Deserialize(f) as WaveWrapper;

            f.Close();


            foreach (Wave s in Waves.WaveList)
            {
                ret.Add(s);
            }
        }
        catch(System.Exception ex)
        {
            Debug.Log(ex.ToString());
            //dText.text = ex.ToString();
        }
        finally
        {

        }

        return ret;
    }
}
