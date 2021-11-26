using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class ObjectSaverLoaderNew : MonoBehaviour {

    public static List<GameObject> existingObjects;
    //public static SerializableCard Wave;
    public static WaveWrapper Waves;
    public static UnityEngine.UI.Text dText;

	// Use this for initialization
	void Start ()
    {
        existingObjects = new List<GameObject>();
        //dText = GameObject.Find("DebugText").GetComponent<UnityEngine.UI.Text>();
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
        FileStream f = new FileStream(Application.dataPath + "/StreamingAssets/XML/WavesNew.xml", FileMode.Create);
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

        try
        {
            FileStream f = new FileStream(Application.dataPath + "/StreamingAssets/XML/WavesNew.xml", FileMode.Open);

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
