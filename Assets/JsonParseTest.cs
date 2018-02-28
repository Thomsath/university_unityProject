using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEditor;
using UnityEngine.UI;
using System;

public class JsonParseTest : MonoBehaviour
{
    private string JSONString;
    private JsonData itemData;
    public Text m_MyText;
    private string data;
    private string JSONdata;

    // Use this for initialization
    void Start()
    {

        string url = "http://api.health.nokia.com/measure?action=getmeas&oauth_consumer_key=6901bc0863d8b2b110757d6edf48ff64676b4137f4e05574f431f7431175&oauth_nonce=8db1e10c1a06fb4cff09e75c5fb83089&oauth_signature=IHWedoJk5pSuCN1QoX0%2BQbAbbnk%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1517838908&oauth_token=8ef03624c0916d27d6fd6fbd1395cb6fbf86718c0e73645198cd9f31de3e22&oauth_version=1.0&userid=15399388";
        WWW www = new WWW(url);
        StartCoroutine(getJSONData(www));

        // On ecrit dans le fichier les nouvelles données


        //On récupère le JSON par le fichier
        JSONString = File.ReadAllText(Application.dataPath + "/data.json");
        Debug.Log(JSONString);
        //Convertir JSON par un objet
        itemData = JsonMapper.ToObject(JSONString);

        m_MyText.text = GetItemandWritewithType();
    }

    // Update is called once per frame
    void Update()
    {
        string url = "http://api.health.nokia.com/measure?action=getmeas&oauth_consumer_key=6901bc0863d8b2b110757d6edf48ff64676b4137f4e05574f431f7431175&oauth_nonce=8db1e10c1a06fb4cff09e75c5fb83089&oauth_signature=IHWedoJk5pSuCN1QoX0%2BQbAbbnk%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1517838908&oauth_token=8ef03624c0916d27d6fd6fbd1395cb6fbf86718c0e73645198cd9f31de3e22&oauth_version=1.0&userid=15399388";
        WWW www = new WWW(url);
        StartCoroutine(getJSONData(www));
        //On récupère le JSON par le fichier
        JSONString = File.ReadAllText(Application.dataPath + "/data.json");

        //Convertir JSON par un objet
        itemData = JsonMapper.ToObject(JSONString);

        m_MyText.text = GetItemandWritewithType();
    }


    JsonData GetItem(int item, string type)
    {

        return itemData["body"]["measuregrps"][item][type];

        /*for (int i = 0; i < itemData[type].Count; i++)
        {
            if((itemData["body"]["measuregrps"][i][name] == )
        }
        return null;*/

    }

    string GetItemandWritewithType()
    {
        var listitem = new List<JsonData> { };
        var listmesure = new List<JsonData> { };
        string text = "";

        foreach (JsonData item in itemData["body"]["measuregrps"])
        {
            listitem.Add(item);
        }
        for (int i = 0; i < listitem.Count; i++)
        {

            foreach (JsonData mesure in itemData["body"]["measuregrps"][i]["measures"])
            {
                listmesure.Add(mesure);
            }
            for (int j = 0; j < listmesure.Count; j++)
            {
                //Debug.Log(measures);
                string val = "" + itemData["body"]["measuregrps"][i]["measures"][j]["value"].GetNatural();

                string type = "" + itemData["body"]["measuregrps"][i]["measures"][j]["type"].GetNatural();
                //Debug.Log(type);

                switch (type)
                {
                    case "1":
                        //Poids
                        text += "Poids : " + val.Substring(0, 2) + "." + val.Substring(2, 2) + " kg\n";
                        break;
                    case "4":
                        //Taille
                        text += "Taille : " + val.Substring(0, 1) + "." + val.Substring(1, 2) + " m\n";
                        break;
                    case "9":
                        //Diastolic
                        text += "Diastolic : " + val + "\n";
                        break;
                    case "10":
                        //Systolic
                        text += "Sytolic : " + val + "\n";
                        break;
                    case "11":
                        //BPM (Battements par minute)
                        text += "BPM : " + val + "\n";
                        break;
                    case "71":
                        //Temperature (Battements par minute)
                        string dizaine = val.Substring(0, 2);
                        string unite = val.Substring(2, 1);
                        text += "Temperature : " + Math.Round(Convert.ToDouble(dizaine + "." + unite), 1) + "\n";
                        break;

                    default:
                        text += "Pas encore fait" + "\n";
                        break;
                }

            }
            listmesure = new List<JsonData> { };
        }

        return text;
    }

    IEnumerator getJSONData(WWW www)
    {
        yield return www;

        // Get du contnu de la page
        if (www.error == null)
        {
            JSONdata = www.text;

            string path = "Assets/data.json";
            //Write some text to the test.txt file

            //On écrit dans le fichier (String.empty)
            System.IO.File.WriteAllText("Assets/data.json", JSONdata);

            /* StreamWriter writer = new StreamWriter(path, true);

            writer.Write(JSONdata);*/
        }
        else
        {
            Debug.Log("WWW Error:");
        }
    }

    static void ReadString()
    {
        string path = "/data.json";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}