using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class Customer : MonoBehaviour {
    public string TestDataPath = "";
    private bool isTestmode = false;
    private CustomerData[] Data;

    // Use this for initialization
    void Start () {
        if (TestDataPath.Length > 0)
            isTestmode = true;

        StartCoroutine(GetAllCustomers());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator GetAllCustomers()
    {
        if(isTestmode)
        {
            string TestData = File.ReadAllText(TestDataPath);
            TestData = JsonHelper.fixJson(TestData);
            Data = JsonHelper.FromJson<CustomerData>(TestData);
        }
        else
        {
            using (UnityWebRequest www = UnityWebRequest.Get("https://documentanalysis.azurewebsites.net/api/GetAccountNames?code=GUiyIkbVVF2hqNDM4mgbvvvoFIMP9QUQO0BXNdwy40t8QMowLnTFZg== "))
            {
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    // Show results as text
                    //Debug.Log(www.downloadHandler.text);

                    // Or retrieve results as binary data
                    byte[] results = www.downloadHandler.data;
                    string JsonData = System.Text.Encoding.UTF8.GetString(results);
                    JsonData = JsonHelper.fixJson(JsonData);
                    Data = JsonHelper.FromJson<CustomerData>(JsonData);
                }
            }
        }
    }

    [System.Serializable]
    public class CustomerData
    {
        public string name;
        public string Id;
    }
}
