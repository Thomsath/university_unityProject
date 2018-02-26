using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System;
using System.Linq;
using System.Diagnostics;
using UnityEngine.Video;

public class LineGraphManagerTension : MonoBehaviour {

	public GameObject linerenderer;
	public GameObject pointer;

	public GameObject pointerRed;
	public GameObject pointerBlue;

	public GameObject HolderPrefb;

	public GameObject holder;
	public GameObject xLineNumber;

	public Material bluemat;
	public Material greenmat;

	public Text topValue;

	public List<GraphData> graphDataPlayer1 = new List<GraphData>();

	private GraphData gd;
    private float highestValue = 150;

	public Transform origin;

	public TextMesh player1name;
	public VideoPlayer Movie;

	private float lrWidth = 0.1f;
	private int dataGap = 0;


    private string JSONString;
    private JsonData itemData;
    private string data;
    private string JSONdata;

    private List<string> Templist = new List<string> { };
	//private List<string> Templistupdate = new List<string> { };
	private List<string> ListTime = new List<string> { };
	private List<string> passtonext;

    IEnumerator Start(){

        // Ajouter les données ici 
        string url = "http://api.health.nokia.com/measure?action=getmeas&oauth_consumer_key=6901bc0863d8b2b110757d6edf48ff64676b4137f4e05574f431f7431175&oauth_nonce=8db1e10c1a06fb4cff09e75c5fb83089&oauth_signature=IHWedoJk5pSuCN1QoX0%2BQbAbbnk%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1517838908&oauth_token=8ef03624c0916d27d6fd6fbd1395cb6fbf86718c0e73645198cd9f31de3e22&oauth_version=1.0&userid=15399388";
        WWW www = new WWW(url);
        StartCoroutine(getJSONData(www));
        // On ecrit dans le fichier les nouvelles données
        //On récupère le JSON par le fichier
        JSONString = File.ReadAllText(Application.dataPath + "/data.json");
        //Debug.Log(JSONString);
        //Convertir JSON par un objet
        itemData = JsonMapper.ToObject(JSONString);

		StartCoroutine (GetItemsTemperature ());

        int index = Templist.Count;
        for (int i = 0; i < index; i++)
        {
            //Debug.Log(Templist[i]);
            GraphData gd = new GraphData();
			gd.marbles = float.Parse(Templist[i]);
            graphDataPlayer1.Add(gd);

        }
        // Montrer le graphique
        //TradTime(1518430929);
		if (!Movie.isPlaying)
		{
			ShowGraph();
			GameObject[] b = GameObject.FindGameObjectsWithTag("Graphel");
			List<Renderer> Listrenderer = new List<Renderer>() { };
			foreach (GameObject point in b)
			{
				Renderer[] listtoadd = point.GetComponentsInChildren<MeshRenderer>();
				foreach (Renderer r in listtoadd)
				{
					Listrenderer.Add(r);

				}
			}

			foreach (Renderer g in Listrenderer)
			{
				g.enabled = true;
			}

		}
		else
		{
			GameObject[] b = GameObject.FindGameObjectsWithTag("Graphel");
			List<Renderer> Listrenderer = new List<Renderer>() { };
			foreach (GameObject point in b)
			{
				Renderer[] listtoadd = point.GetComponentsInChildren<MeshRenderer>();
				foreach (Renderer r in listtoadd)
				{
					Listrenderer.Add(r);

				}
			}

			foreach (Renderer g in Listrenderer)
			{
				g.enabled = false;
			}
		}

        yield return StartCoroutine(UpdateScreen());
    }

    DateTime TradTime(double timestamp)
    {
        // First make a System.DateTime equivalent to the UNIX Epoch.
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        // Add the number of seconds in UNIX timestamp to be converted.
        dateTime = dateTime.AddSeconds(timestamp);


        return dateTime;
    }
	
	IEnumerator UpdateScreen(){

        string url = "http://api.health.nokia.com/measure?action=getmeas&oauth_consumer_key=6901bc0863d8b2b110757d6edf48ff64676b4137f4e05574f431f7431175&oauth_nonce=8db1e10c1a06fb4cff09e75c5fb83089&oauth_signature=IHWedoJk5pSuCN1QoX0%2BQbAbbnk%3D&oauth_signature_method=HMAC-SHA1&oauth_timestamp=1517838908&oauth_token=8ef03624c0916d27d6fd6fbd1395cb6fbf86718c0e73645198cd9f31de3e22&oauth_version=1.0&userid=15399388";
        WWW www = new WWW(url);
        StartCoroutine(getJSONData(www));


        // On ecrit dans le fichier les nouvelles données
        //On récupère le JSON par le fichier
        JSONString = File.ReadAllText(Application.dataPath + "/data.json");
        //Debug.Log(JSONString);
        //Convertir JSON par un objet
        itemData = JsonMapper.ToObject(JSONString);

        //GetItemsTemperature();
		StartCoroutine(GetItemsTemperature());

		graphDataPlayer1.Clear();

        int index = Templist.Count;
        for (int i = 0; i < index; i++)
        {
            //Debug.Log(Templist[i]);
            GraphData gd = new GraphData();
			gd.marbles = float.Parse(Templist[i]);
            graphDataPlayer1.Add(gd);

        }
        // Montrer le graphique
		if (!Movie.isPlaying)
		{
			ShowGraph();
			GameObject[] b = GameObject.FindGameObjectsWithTag("Graphel");
			List<Renderer> Listrenderer = new List<Renderer>() { };
			foreach (GameObject point in b)
			{
				Renderer[] listtoadd = point.GetComponentsInChildren<MeshRenderer>();
				foreach (Renderer r in listtoadd)
				{
					Listrenderer.Add(r);

				}
			}

			foreach (Renderer g in Listrenderer)
			{
				g.enabled = true;
			}

		}
		else
		{
			GameObject[] b = GameObject.FindGameObjectsWithTag("Graphel");
			List<Renderer> Listrenderer = new List<Renderer>() { };
			foreach (GameObject point in b)
			{
				Renderer[] listtoadd = point.GetComponentsInChildren<MeshRenderer>();
				foreach (Renderer r in listtoadd)
				{
					Listrenderer.Add(r);

				}
			}

			foreach (Renderer g in Listrenderer)
			{
				g.enabled = false;
			}
		}
		yield return null;
        StartCoroutine(UpdateScreen());
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

        }
        else
        {
            //Debug.Log("WWW Error:");
        }
    }

	IEnumerator GetItemsTemperature()
	{
		List<string> passtonext = new List<string>(ListTime);

		StartCoroutine(Alltempadd());
		ListTime = passtonext;

		if (passtonext.Count != Templist.Count)
		{
			if(Math.Round(Time.time, 0)-25 < 0)
			{
				ListTime.Add(""  + 0);
			}
			else
			{
				ListTime.Add("" + (Math.Round(Time.time, 1) - 25));
			}

		}

		yield return null;
	}


	IEnumerator Alltempadd()
	{
		Templist.Clear();
		var listitem = new List<JsonData> { };
		var listmesure = new List<JsonData> { };

		foreach (JsonData item in itemData["body"]["measuregrps"])
		{
			listitem.Add(item);
		}
		for (int i = listitem.Count-1; i > -1; i--)
		{

			foreach (JsonData mesure in itemData["body"]["measuregrps"][i]["measures"])
			{
				listmesure.Add(mesure);
			}
			for (int j = listmesure.Count-1; j > -1 ; j--)
			{
				string val = "" + itemData["body"]["measuregrps"][i]["measures"][j]["value"].GetNatural();

				string type = "" + itemData["body"]["measuregrps"][i]["measures"][j]["type"].GetNatural();

				double time = itemData["body"]["measuregrps"][i]["date"].GetNatural();


				if (type == "11")
				{
					Templist.Add(val);
					//ListTime.Add(TradTime(time));
				}

			}
			listmesure = new List<JsonData> { };
		}
		yield return null;
	}

    /*void GetItemsTemperature()
    {
        Templist.Clear();
        var listitem = new List<JsonData> { };
        var listmesure = new List<JsonData> { };

        foreach (JsonData item in itemData["body"]["measuregrps"])
        {
            listitem.Add(item);
        }
        for (int i = listitem.Count-1; i > -1; i--)
        {

            foreach (JsonData mesure in itemData["body"]["measuregrps"][i]["measures"])
            {
                listmesure.Add(mesure);
            }
            for (int j = listmesure.Count-1; j > -1 ; j--)
            {
                string val = "" + itemData["body"]["measuregrps"][i]["measures"][j]["value"].GetNatural();

                string type = "" + itemData["body"]["measuregrps"][i]["measures"][j]["type"].GetNatural();

                double time = itemData["body"]["measuregrps"][i]["date"].GetNatural();


                if (type == "11")
                {
                    Templist.Add(val);
                    ListTime.Add(TradTime(time));
                }

                }
            listmesure = new List<JsonData> { };
        }
           
      }*/

	  
    public void ShowData(GraphData[] gdlist, int playerNum, float gap)
    {

        // Adjusting value to fit in graph
        for (int i = 0; i < gdlist.Length; i++)
        {
            // since Y axis is from 0 to 7 we are dividing the marbles with the highestValue
            // so that we get a value less than or equals to 1 and than we can multiply that
            // number with Y axis range to fit in graph. 
            // e.g. marbles = 90, highest = 90 so 90/90 = 1 and than 1*7 = 7 so for 90, Y = 7
            //Debug.Log(Convert.ToSingle(Math.Round(((gdlist[i].marbles / highestValue) * 11) - 2f, 2)));
			gdlist[i].marbles = Convert.ToSingle(Math.Round(((gdlist[i].marbles / highestValue) * 10) -3.35, 2));


        }
        
        StartCoroutine(BarGraphBlue(gdlist, gap));
    }


    public void ShowGraph()
    {

        //ClearGraph();

        if (graphDataPlayer1.Count >= 1 /*&& graphDataPlayer2.Count >= 1*/)
        {
            holder = Instantiate(HolderPrefb, Vector3.zero, Quaternion.identity) as GameObject;
            holder.name = "h2";
			holder.tag = "Graphel";

            GraphData[] gd1 = new GraphData[graphDataPlayer1.Count];
            for (int i = 0; i < graphDataPlayer1.Count; i++)
            {
                GraphData gd = new GraphData();
                gd.marbles = graphDataPlayer1[i].marbles;
                gd1[i] = gd;
            }

            dataGap = GetDataGap(graphDataPlayer1.Count);


            int dataCount = 0;
            int gapLength = 1;
            float gap = 1.0f;
            bool flag = false;

            ShowData(gd1, 1, gap);
        }
    }

    public void ClearGraph(){
		if(holder)
			Destroy(holder);
	}

	int GetDataGap(int dataCount){
		int value = 1;
		int num = 0;
		while((dataCount-(40+num)) >= 0){
			value+= 1;
			num+= 20;
		}
		
		return value;
	}
    

	IEnumerator BarGraphBlue(GraphData[] gd,float gap)
	{
		float xIncrement = gap;
		int dataCount = 0;
		bool flag = false;
		Vector3 startpoint = new Vector3((origin.position.x-xIncrement),(origin.position.y+gd[dataCount].marbles),(origin.position.z));


		while(dataCount < gd.Length)
		{
			
			Vector3 endpoint = new Vector3((origin.position.x-xIncrement),(origin.position.y+gd[dataCount].marbles),(origin.position.z));
			startpoint = new Vector3(startpoint.x,startpoint.y,origin.position.z);
			// pointer is an empty gameObject, i made a prefab of it and attach it in the inspector
			GameObject p = Instantiate(pointer, new Vector3(startpoint.x, startpoint.y, origin.position.z),Quaternion.identity) as GameObject;
			p.transform.parent = holder.transform;

			GameObject Temptext = Instantiate(xLineNumber, new Vector3(origin.position.x - xIncrement, origin.position.y + gd[dataCount].marbles + 0.5f, origin.position.z), Quaternion.identity) as GameObject;
			Temptext.transform.parent = holder.transform;
            Temptext.GetComponent<TextMesh>().text = Templist[dataCount].ToString() ;
            Temptext.GetComponent<TextMesh>().color = Color.black;
			Temptext.transform.Rotate (0, 180, 0);

            GameObject lineNumber = Instantiate(xLineNumber, new Vector3(origin.position.x - xIncrement, origin.position.y - 0.5f, origin.position.z), Quaternion.identity) as GameObject;
            lineNumber.transform.parent = holder.transform;
			lineNumber.GetComponent<TextMesh>().text = ListTime [dataCount];
            lineNumber.GetComponent<TextMesh>().color = Color.black;
			lineNumber.transform.Rotate (0, 180, 0);


            // linerenderer is an empty gameObject with Line Renderer Component Attach to it, 
            // i made a prefab of it and attach it in the inspector
            GameObject lineObj = Instantiate(linerenderer,startpoint,Quaternion.identity) as GameObject;
			lineObj.transform.parent = holder.transform;
			lineObj.name = dataCount.ToString();
			
			LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();
			
			lineRenderer.material = bluemat;
			lineRenderer.SetWidth(lrWidth, lrWidth);
			lineRenderer.SetVertexCount(2);

			if (Movie.isPlaying)
			{
				lineRenderer.enabled = false;
				/*Renderer[] a = holder.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in a)
                {
                    r.enabled = false;
                }*/


			}
			else
			{
				lineRenderer.enabled = true;
				/*Renderer[] a = holder.GetComponentsInChildren<Renderer>();
                foreach (Renderer r in a)
                {
                    r.enabled = true;
                }*/

			}


			while(Vector3.Distance(p.transform.position,endpoint) > 0.2f)
			{
				float step = 5 * Time.deltaTime;
				p.transform.position = Vector3.MoveTowards(p.transform.position, endpoint, step);
				lineRenderer.SetPosition(0, startpoint);
				lineRenderer.SetPosition(1, p.transform.position);
				
				yield return null;
			}
			
			lineRenderer.SetPosition(0, startpoint);
			lineRenderer.SetPosition(1, endpoint);
			

			p.transform.position = endpoint;
			GameObject pointered = Instantiate(pointerRed,endpoint,pointerRed.transform.rotation) as GameObject ;
			pointered.transform.parent = holder.transform;
			startpoint = endpoint;

			if(dataGap > 1){
				if((dataCount+dataGap) == gd.Length){
					dataCount+=dataGap-1;
					flag = true;
				}
				else if((dataCount+dataGap) > gd.Length && !flag){
					dataCount =	gd.Length-1;
					flag = true;
				}
				else{
					dataCount+=dataGap;
					if(dataCount == (gd.Length-1))
						flag = true;
				}
			}
			else
				dataCount+=dataGap;

			xIncrement+= gap;
			
			yield return null;
			
		}
	}

	public class GraphData
	{
		public float marbles;
	}
}
