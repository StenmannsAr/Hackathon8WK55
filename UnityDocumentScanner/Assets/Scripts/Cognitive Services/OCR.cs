using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OCR : MonoBehaviour {
    public GameObject Canvas = null;
    public Button ButtonPrefab = null;
    public string subscriptionKey = "";
    public string TestPfad = "";
    private bool IsTestMode = true;
    private const string uriBase = "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/ocr?language=unk&detectOrientation=true";
    RootObject Data;

    // Use this for initialization
    void Start ()
    {

    }

    byte[] GetImageAsByteArray(string imageFilePath)
    {
        using (FileStream fileStream = new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
        {
            BinaryReader binaryReader = new BinaryReader(fileStream);
            return binaryReader.ReadBytes((int)fileStream.Length);
        }
    }

    // Update is called once per frame
    void Update () {		
	}

    public void SendPicture(List<byte> theBytes)
    {
        StartCoroutine(MakeAnalysisRequest(theBytes.ToArray()));
    }

    IEnumerator MakeAnalysisRequest(byte[] theBytes)
    {
        var headers = new Dictionary<string, string>() {
            { "Ocp-Apim-Subscription-Key", subscriptionKey },
            { "Content-Type", "application/octet-stream" }
        };

        WWW www = new WWW(uriBase, theBytes, headers);

        yield return www;
        string responseData = www.text; // Save the response as JSON string
        
        Data = JsonConvert.DeserializeObject<RootObject>(responseData);
        List<List<String>> linewords =  Data.regions
            .Select(region => region.lines)
            .Select(regionline => regionline.SelectMany(line => line.words)
                .Select(word => word.text).ToList())
            .ToList();

        Debug.Log(String.Join(Environment.NewLine, linewords.Select(v => String.Join(" ", v)).ToList().Select(line => String.Join(Environment.NewLine, line))));

        //TODO: Search in words for customerdata:
        List<CustomerData> customerCandidates = CustomerDataProvider.Customers.Where(cust => linewords.SelectMany(v => v).Contains(cust.name)).ToList();

        Debug.Log($"Found customers: {(customerCandidates.Count > 0 ? String.Join(", ",customerCandidates.Select(cust => $"{cust.name} ({cust.Id.ToString()})")) : "None.")}");

        for (int i = 0; i < customerCandidates.Count - 1; i++)
        {
            CustomerData customercandidate = customerCandidates[i];
            var button = Instantiate(ButtonPrefab);
            button.GetComponentInChildren<ButtonClick>().ButtonName = customercandidate.name;
            button.tag = customercandidate.Id.ToString();

            //button.transform.localPosition = new Vector3 { x = 0, y = 0 - (i * 30), z = 0 };

            button.transform.SetParent(Canvas.transform, false);
        }
     
        Canvas.SetActive(true);
    }

    private void CustomerSelected(String buttonText)
    {
        Debug.Log($"Clicked Button:{buttonText}");
    }
}

[System.Serializable]
public class Word
{
    public string boundingBox { get; set; }
    public string text { get; set; }
}

[System.Serializable]
public class Line
{
    public string boundingBox { get; set; }
    public List<Word> words { get; set; }
}

[System.Serializable]
public class Region
{
    public string boundingBox { get; set; }
    public List<Line> lines { get; set; }
}

[System.Serializable]
public class RootObject
{
    public string language { get; set; }
    public string orientation { get; set; }
    public double textAngle { get; set; }
    public List<Region> regions { get; set; }
}
