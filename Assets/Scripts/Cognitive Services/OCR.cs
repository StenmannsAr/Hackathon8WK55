using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class OCR : MonoBehaviour {
    public string subscriptionKey = "";
    public string TestPfad = "";
    private bool IsTestMode = true;
    private const string uriBase =
            "https://westeurope.api.cognitive.microsoft.com/vision/v2.0/ocr?language=unk&detectOrientation=true";
    RootObject Data;

    // Use this for initialization
    void Start () {
       IsTestMode = TestPfad.Length > 0 ? false : true;
        if (IsTestMode)
        {
            string path = @"C:\unity\CS_API\testpic2.jpg";
            byte[] bytes = GetImageAsByteArray(path);
            StartCoroutine(MakeAnalysisRequest(bytes));
        }
    }

    byte[] GetImageAsByteArray(string imageFilePath)
    {
        using (FileStream fileStream =
            new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
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
        Debug.Log(responseData);
        Data = JsonConvert.DeserializeObject<RootObject>(responseData);    
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
