using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BirthdateMask : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputBox;

    [SerializeField] public UnityEvent onValidate;

    public const string MathDayPattern = "^([0]?[0-9]|[12][0-9]|[3][01])$";
    public const string MathMonthPattern = "^([0]?[1-9]|[1][0-2])$";
    public const string MathYearPattern = "^([0-9]{4}|[0-9]{2})$";

    private bool canAutocomplete;
    [SerializeField] private bool valided;

    public static BirthdateMask instance;

    private void OnEnable()
    {
        instance = this;
    }
    private void Start()
    {
        onValidate.AddListener(delegate { ValidateInput.instance.SetInputValid(TypeInput.Type.BIRTHDATE); });
        onValidate.AddListener(delegate { valided = true; });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnPressBackSpace();
        }
    }
    private void OnPressBackSpace()
    {
        canAutocomplete = false;
        VerifyLastChar();

        if (valided)
        {
            ValidateInput.instance.HideInputValid(TypeInput.Type.BIRTHDATE);
            valided = false;
        }
    }


    #region VerifyValues
    //----------------------VERIFY-REGION---------------------
    public void ValueChangeCheck()
    {
        string inputText = inputBox.text;

        if (inputText.Length != 0)
        {
            if ((inputText[inputText.Length - 1]) != '/')
            {

                int size = inputText.Split('/').Length;

                switch (size)
                {
                    case 1:
                        string day = inputText.Split('/')[0];
                        VerifyDay(day);
                        break;
                    case 2:
                        string month = inputText.Split('/')[1];
                        VerifyMonth(month);
                        break;
                    case 3:
                        string year = inputText.Split('/')[2];
                        VerifyYear(year);
                        break;
                }
            }
        }
    }
    private void VerifyLastChar()
    {
        try
        {
            if (inputBox.text[inputBox.text.Length - 1] == '/')
            {
                RemoveLastNumbers(2);
            }
        }
        catch (System.Exception)
        {

        }
    }
    private void VerifyDay(string day)
    {
        if (day.Length == 2)
        {
            if (Regex.IsMatch(day, MathDayPattern))
            {
                if (canAutocomplete)
                {
                    StartCoroutine(AutoCompleteBar(inputBox));
                    canAutocomplete = false;
                }
            }
            else
            {
                RemoveLastNumbers(2);
                ShowInvalidText("dia inválido");
            }
        }
        else
        {
            canAutocomplete = true;
        }
    }
    private void VerifyMonth(string month)
    {
        if (month.Length == 2)
        {
            if (Regex.IsMatch(month, MathMonthPattern))
            {
                if (canAutocomplete)
                {
                    StartCoroutine(AutoCompleteBar(inputBox));
                    canAutocomplete = false;
                }
            }
            else
            {
                RemoveLastNumbers(2);
                ShowInvalidText("mês inválido");
            }
        }
        else
        {
            canAutocomplete = true;
        }
    }
    private void VerifyYear(string year)
    {
        if (year.Length == 4)
        {
            if (Regex.IsMatch(year, MathYearPattern))
            {
                if ((int.Parse(year) < System.DateTime.Now.Year) && (int.Parse(year) > System.DateTime.Now.Year - 130))
                {
                    onValidate.Invoke();
                }
                else
                {
                    RemoveLastNumbers(4);
                    ShowInvalidText("ano inválido");
                }
            }
            else
            {
                RemoveLastNumbers(4);
                ShowInvalidText("ano inválido");
            }
        }
        if (year.Length > 4)
        {
            RemoveLastNumbers(1);
        }
    }


    #endregion

    private void RemoveLastNumbers(int number)
    {
        inputBox.text = inputBox.text.Remove(inputBox.text.Length - number);
    }
    private void ShowInvalidText(string text)
    {
        ValidateInput.instance.SetInputInvalid(TypeInput.Type.BIRTHDATE, text);
    }


    private IEnumerator AutoCompleteBar(TMP_InputField input)
    {
        input.text = input.text.Insert(inputBox.text.Length, "/");
        yield return new WaitForEndOfFrame();
        input.caretPosition = input.text.Length;
        input.ForceLabelUpdate();
    }
}
