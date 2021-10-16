using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ValidateInput : MonoBehaviour
{
    [SerializeField] private float timeFade;

    [SerializeField] private List<FeedbackGroup> feedbackGroup;
    public static ValidateInput instance;

    private void OnEnable()
    {
        HideAll();
        instance = this;   
    }
    private void HideAll()
    {
        foreach (var feedback in feedbackGroup)
        {
            feedback.imageInvalid.DOFade(0f, 0f);
            feedback.imageValid.DOFade(0f, 0f);
            feedback.textBox.DOFade(0f, 0f);
        }
    }
    public void SetInputValid(TypeInput.Type type)
    {
        SetValid(feedbackGroup[(int)type]);
    }
    public void SetInputInvalid(TypeInput.Type type, string text)
    {
        SetInvalid(feedbackGroup[(int)type], text);
    }
    public void HideInputValid(TypeInput.Type type)
    {
        feedbackGroup[(int)type].imageValid.DOFade(0f, 0f);
    }
    private void SetValid(FeedbackGroup group)
    {
        group.imageValid.DOFade(1f, timeFade);
        group.imageInvalid.DOFade(0f, 0f);
        group.textBox.DOFade(0f, 0f);
    }
    private void SetInvalid(FeedbackGroup group, string text)
    {
        group.imageValid.DOFade(0f, timeFade);
        group.imageInvalid.DOFade(1f, timeFade);
        group.textBox.DOFade(1f, timeFade);
        group.textBox.text = text;
    }
}
[System.Serializable]
public class FeedbackGroup
{
    [Tooltip("Apenas para visualização do Desenvolvedor")]
    [SerializeField] private string name;
    [Header("Imagens")]
    [SerializeField] public Image imageValid;
    [SerializeField] public Image imageInvalid;
    [SerializeField] public TextMeshProUGUI textBox;
}
[System.Serializable]
public class TypeInput
{
    public enum Type { FULLNAME, EMAIL, BIRTHDATE }
}
