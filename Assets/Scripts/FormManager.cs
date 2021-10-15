using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FormManager : MonoBehaviour
{
    [SerializeField] private string urlAPI;
    [SerializeField] private List<CustomInputField> inputs;

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    public const string MathBirthdatePattern = "^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$";

    private bool formIsValid;

    public void Confirm()
    {
        if (VerifyInputs())
        {
            GetRequest.instance.Get(CreateLinkAPI());
        }
    }

    private string CreateLinkAPI()
    {
        string s = "candidate=" + GetFullname().Split()[0].ToLower() + "&fullname=" + GetFullname() + "&email=" + GetEmail() + "&birthdate=" + GetBirthdate();

        string url = urlAPI + s;

        return url;
    }

    public string GetFullname()
    {
        foreach (var input in inputs)
        {
            if(input.type == CustomInputField.Type.FULLNAME)
            {
                return input.inputBox.text;
            }
        }
        return null;
    }
    public string GetEmail()
    {
        foreach (var input in inputs)
        {
            if (input.type == CustomInputField.Type.EMAIL)
            {
                return input.inputBox.text;
            }
        }
        return null;
    }
    public string GetBirthdate()
    {
        foreach (var input in inputs)
        {
            if (input.type == CustomInputField.Type.BIRTHDATE)
            {
                return input.inputBox.text;
            }
        }
        return null;
    }
    private bool VerifyInputs()
    {
        formIsValid = true;

        foreach (var input in inputs)
        {
            if (input.required)
            {
                switch (input.type)
                {
                    case CustomInputField.Type.FULLNAME:
                        TreatResult("nome", VerifyFullName(input.inputBox.text));
                        break;

                    case CustomInputField.Type.EMAIL:
                        TreatResult("email", VerifyEmail(input.inputBox.text));
                        break;

                    case CustomInputField.Type.BIRTHDATE:
                        TreatResult("birthdate", VerifyBirthdate(input.inputBox.text));
                        break;
                }
            }
        }
        if (formIsValid)
        {
            return true;
        }
        return false;
    }

    private void TreatResult(string type, Result result)
    {
        switch (result)
        {
            case Result.OK:
                Debug.Log(type + " está ok");
                break;
            case Result.INCOMPLETE:
                NotificationError(type, "incompleto");
                break;
            case Result.NULL:
                NotificationError(type, "vazio");
                break;
        }
    }

    private void NotificationError(string type, string error)
    {
        formIsValid = false;
        Debug.Log(type + " está: " + error);
        //chamar um popup
    }

    private Result VerifyFullName(string name)
    {
        if(name != null)
        {
            if(name.Split().Length > 1)
            {
                return Result.OK;
            }
            else
            {
                return Result.INCOMPLETE;
            }
        }
        return Result.NULL;
    }
    private Result VerifyEmail(string email)
    {
        if (email != null)
        {
            if (Regex.IsMatch(email, MatchEmailPattern))
            {
                return Result.OK;
            }
            return Result.INCOMPLETE;
        }
        return Result.NULL;
    }
    private Result VerifyBirthdate(string birthdate)
    {
        if(birthdate != null)
        {
            if (Regex.IsMatch(birthdate, MathBirthdatePattern))
            {
                return Result.OK;
            }
            return Result.INCOMPLETE;
        }
        return Result.NULL;
    }

    public enum Result { NULL, INCOMPLETE, OK}

}
[System.Serializable]
public class CustomInputField
{
    [Tooltip("Apenas para visualização do desenvolvedor")]
    public string name;
    public bool required;
    public Type type;
    public TMP_InputField inputBox;

    public enum Type { FULLNAME, EMAIL, BIRTHDATE}
}
