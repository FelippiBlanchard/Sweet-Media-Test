using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ConfirmationRequest : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageBox;

    [Header("Events")]
    [SerializeField] private UnityEvent onSuccess;
    [SerializeField] private UnityEvent onFailure;

    public static ConfirmationRequest instance;
    private void Start()
    {
        instance = this;
    }
    public void TreatResponse(string json)
    {
        Response response = JsonUtility.FromJson<Response>(json);

        if (response.success)
        {
            onSuccess.Invoke();
            messageBox.text = response.data.message;
        }
        else
        {
            onFailure.Invoke();
            messageBox.text = "Erro: " + response.status;
        }
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
