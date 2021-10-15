using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseRequest : MonoBehaviour
{
    public static void TreatResponse(string json)
    {
        Response response = new Response();
        response = JsonUtility.FromJson<Response>(json);
        Debug.Log(response.success);
        Debug.Log(response.status);
        Debug.Log(response.data.message);
    }
}
[System.Serializable]
public class Response
{
    public bool success;
    public string status;
    public Data data;

    [System.Serializable]
    public class Data
    {
        public string message;
    }
}
