using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class CustomerDataProvider : MonoBehaviour
{
    public static CustomerData[] Customers;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(GetAllCustomers());
    }

    IEnumerator GetAllCustomers()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("https://documentanalysis.azurewebsites.net/api/GetAccountNames?code=GUiyIkbVVF2hqNDM4mgbvvvoFIMP9QUQO0BXNdwy40t8QMowLnTFZg=="))
        {
            yield return www.SendWebRequest();

            try
            {
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    // Or retrieve results as binary data
                    byte[] results = www.downloadHandler.data;
                    string JsonData = System.Text.Encoding.UTF8.GetString(results);
                    //JsonData = JsonHelper.fixJson(JsonData);
                    Customers = Newtonsoft.Json.JsonConvert.DeserializeObject<CustomerData[]>(JsonData);
                }
            }
            catch (System.Exception)
            {

                throw;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
