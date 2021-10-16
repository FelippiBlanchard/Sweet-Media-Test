using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class FormStructureManager : MonoBehaviour
{
    [Header("Settings Animation Panel")]
    [Range(0.5f,2f)]
    [SerializeField] private float timeToBegin;
    [Range(0.2f, 1.5f)]
    [SerializeField] private float timeFade;
    [Range(0.2f, 1.5f)]
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
        StartCoroutine(Initialize());
    }
    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(timeToBegin);
        StartForm();
    }

    #region ControlPanel
    //-------------------CONTROL-PANEL-REGION---------------------
    public void StartForm()
    {
        ScaleUpPanel(formPanel, timeScale);
        onStartForm.Invoke();
    }
    public void CloseForm()
    {
        ScaleDownPanel(formPanel, timeScale / 5);
    }


    public void StartLoading()
    {
        FadeInPanel(loadingPanel, timeFade);
        onLoading.Invoke();
    }
    public void CloseLoading()
    {
        FadeOutPanel(loadingPanel, timeScale / 5);
    }

    public void StartConfirmation()
    {
        FadeInPanel(confirmationPanel, timeFade);
        onConfirmation.Invoke();
    }
    public void CloseConfirmation()
    {
        FadeOutPanel(confirmationPanel, timeScale / 5);
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
