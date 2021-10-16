using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GetRequest : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent whenWaiting;
    public UnityEvent onConfirmation;

    public static GetRequest instance;

    private void OnEnable()
    {
        instance = this;
    }
    public void Get(string url)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        
        StartCoroutine(WaitForRequest(www));
        
    }
    public IEnumerator WaitForRequest(UnityWebRequest www)
    {
        whenWaiting.Invoke();
        yield return www.SendWebRequest();

        if (www.error != null)
        {
            Debug.Log("erro " + www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
            onConfirmation.Invoke();
            ConfirmationRequest.instance.TreatResponse(www.downloadHandler.text);

        }
    }
}

