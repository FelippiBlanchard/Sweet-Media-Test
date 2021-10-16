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

    #region MatchPatternDeclaration
    public const string MatchEmailPattern =
    @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
    + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
    + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
    + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
    public const string MathBirthdatePattern = "^([0]?[0-9]|[12][0-9]|[3][01])[./-]([0]?[1-9]|[1][0-2])[./-]([0-9]{4}|[0-9]{2})$";
    #endregion

    private bool formIsValid;
    private void Start()
    {
        SetValueCheckForEachInputField();
    }
    private void SetValueCheckForEachInputField()
    {
        foreach (var input in inputs)
        {
            switch (input.type)
            {
                case TypeInput.Type.FULLNAME:
                    input.inputBox.onDeselect.AddListener(delegate {
                        TreatResult(TypeInput.Type.FULLNAME, CheckFullName(input.inputBox.text));
                    });
                    break;

                case TypeInput.Type.EMAIL:
                    input.inputBox.onDeselect.AddListener(delegate {
                        TreatResult(TypeInput.Type.EMAIL, CheckEmail(input.inputBox.text));
                    });
                    break;

                case TypeInput.Type.BIRTHDATE:
                    input.inputBox.onDeselect.AddListener(delegate {
                        TreatResult(TypeInput.Type.BIRTHDATE, CheckBirthdate(input.inputBox.text));
                    });
                    input.inputBox.onValueChanged.AddListener(delegate { BirthdateMask.instance.ValueChangeCheck(); });
                    break;
            }
        }
    }
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

    #region GetInputs
    //---------------------GET-INPUTS-REGION-----------------------
    public string GetFullname()
    {
        foreach (var input in inputs)
        {
            if (input.type == TypeInput.Type.FULLNAME)
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
            if (input.type == TypeInput.Type.EMAIL)
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
            if (input.type == TypeInput.Type.BIRTHDATE)
            {
                return input.inputBox.text;
            }
        }
        return null;
    }

    #endregion


    #region Notifications
    //---------------------NOTIFICATIONS-REGION-----------------------
    private void NotificationValid(TypeInput.Type type)
    {
        ValidateInput.instance.SetInputValid(type);
    }
    private void NotificationError(TypeInput.Type type, string error)
    {
        formIsValid = false;
        ValidateInput.instance.SetInputInvalid(type, error);
    }

    #endregion


    #region VerifyInputs
    //---------------------VERIFY-INPUTS-REGION-----------------------

    private bool VerifyInputs()
    {
        formIsValid = true;

        foreach (var input in inputs)
        {
            if (input.required)
            {
                switch (input.type)
                {
                    case TypeInput.Type.FULLNAME:
                        TreatResult(TypeInput.Type.FULLNAME, CheckFullName(input.inputBox.text));
                        break;

                    case TypeInput.Type.EMAIL:
                        TreatResult(TypeInput.Type.EMAIL, CheckEmail(input.inputBox.text));
                        break;

                    case TypeInput.Type.BIRTHDATE:
                        TreatResult(TypeInput.Type.BIRTHDATE, CheckBirthdate(input.inputBox.text));
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

    private void TreatResult(TypeInput.Type type, Result result)
    {
        switch (result)
        {
            case Result.OK:
                Debug.Log(type + " está ok");
                NotificationValid(type);
                break;
            case Result.INCOMPLETE:
                NotificationError(type, "incompleto");
                break;
            case Result.NULL:
                NotificationError(type, "vazio");
                break;
        }
    }

    private Result CheckFullName(string name)
    {
        if (name != null)
        {
            if (name.Split().Length > 1)
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
    private Result CheckEmail(string email)
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
    private Result CheckBirthdate(string birthdate)
    {
        if (birthdate != null)
        {
            if (Regex.IsMatch(birthdate, MathBirthdatePattern))
            {
                return Result.OK;
            }
            return Result.INCOMPLETE;
        }
        return Result.NULL;
    }


    #endregion


    public enum Result { NULL, INCOMPLETE, OK}

}
[System.Serializable]
public class CustomInputField
{
    [Tooltip("Apenas para visualização do desenvolvedor")]
    public string name;
    public bool required;
    public TypeInput.Type type;
    public TMP_InputField inputBox;
}
