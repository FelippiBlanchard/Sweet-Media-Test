using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class FormStructureManager : MonoBehaviour
{
    [Header("Settings Animation Panel")]
    [SerializeField] private float timeFade;
    [SerializeField] private float timeScale;

    [Header("Events")]
    [SerializeField] public UnityEvent onStartForm;
    [SerializeField] public UnityEvent onLoading;
    [SerializeField] public UnityEvent onConfirmation;

    [Header("Panels")]
    [SerializeField] private CanvasGroup formPanel;
    [SerializeField] private CanvasGroup loadingPanel;
    [SerializeField] private CanvasGroup confirmationPanel;

   
    private void OnEnable()
    {
        ScaleDownPanel(formPanel, 0f);
    }
    private void Start()
    {
        StartForm();
    }

    public void CloseForm()
    {
        ScaleDownPanel(formPanel, timeScale/5);
    }
    #region StartPanels
    //-------------------START-PANEL-REGION---------------------
    public void StartForm()
    {
        ScaleUpPanel(formPanel, timeScale);
        onStartForm.Invoke();
    }
    public void StartLoading()
    {
        FadeInPanel(loadingPanel, timeFade);
        onLoading.Invoke();
    }
    public void StartConfirmation()
    {
        FadeInPanel(confirmationPanel, timeFade);
        onConfirmation.Invoke();
    }

    #endregion

    #region FadeAnimation
    //----------------FADE-ANIMATION-REGION-----------------------
    private void FadeInPanel(CanvasGroup cg, float time)
    {
        cg.DOFade(1f, time);
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }
    private void FadeOutPanel(CanvasGroup cg, float time)
    {
        cg.DOFade(0f, time);
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    #endregion

    #region ScaleAnimation
    //----------------SCALE-ANIMATION-REGION-----------------------
    private void ScaleUpPanel(CanvasGroup cg, float time)
    {
        cg.GetComponent<RectTransform>().DOScale(1f, time);
        cg.blocksRaycasts = true;
        cg.interactable = true;
    }
    private void ScaleDownPanel(CanvasGroup cg, float time)
    {
        cg.GetComponent<RectTransform>().DOScale(0f, time);
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }

    #endregion
}
