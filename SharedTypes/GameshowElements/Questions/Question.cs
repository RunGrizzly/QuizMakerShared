using System;
using System.Collections;
using System.Collections.Generic;
using Nakama.TinyJson;
using UnityEngine;
using UnityEngine.UI;

public enum QuestionType { simple, trueorfalse, image }


[Serializable]
public abstract class Question : ScriptableObject
{
    public QuestionType questionType;
    public Sprite questionIcon;
    public GameObject questionTemplate;
    public GameObject responseTemplate;
    public GameObject questionInstance;
    public ResponseData correctResponse;

    public virtual string AsJson()
    {
        //
        return "";
    }

    public virtual GameObject GetNewInstance(int _panelIndex)
    {
        GameObject newInstance = Instantiate(questionTemplate, Vector3.zero, Quaternion.Euler(Vector3.zero), Brain.ins.stageManager.layouts[_panelIndex].transform) as GameObject;
        newInstance.transform.localPosition = Vector3.zero;
        newInstance.transform.localEulerAngles = Vector3.zero;
        ShowInstance();
        return newInstance;
    }

    public virtual void SetupQuestion()
    {
        //
    }

    public Timer Timer()
    {
        if (questionInstance == null) return null;
        else return questionInstance.GetComponentInChildren<Timer>();
    }

    public void Refresh()
    {
        Canvas.ForceUpdateCanvases();

        foreach (LayoutGroup layoutGroup in questionInstance.GetComponentsInChildren<LayoutGroup>())
        {
            layoutGroup.enabled = false;
            layoutGroup.enabled = true;
        }
    }

    public virtual void ShowInstance()
    {
        if (questionInstance == null) return;
        CanvasGroup instanceCanvas = questionInstance.GetComponent<CanvasGroup>();
        instanceCanvas.alpha = 0;
        LeanTween.alphaCanvas(instanceCanvas, 1, 0.65f);
    }
    public virtual void HideInstance()
    {
        if (questionInstance == null) return;
        CanvasGroup instanceCanvas = questionInstance.GetComponent<CanvasGroup>();
        instanceCanvas.alpha = 1;
        LeanTween.alphaCanvas(instanceCanvas, 0, 0.45f);
    }

    public virtual void KillInstance()
    {
        if (questionInstance == null) return;

        CanvasGroup instanceCanvas = questionInstance.GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(instanceCanvas, 0, 0.45f).setOnComplete(() => { GameObject.DestroyImmediate(questionInstance); });
    }

    public virtual List<Response> CreateResponses(List<Contestant> _respondants, float _responseTime)
    {
        Debug.Log("This question requires " + _respondants.Count + " responses.");
        return new List<Response>();
    }

    public virtual bool CompareResponse(ResponseData contestantResponse)
    {

        if (contestantResponse.responseBool != correctResponse.responseBool) return false;
        if (contestantResponse.responseInt != correctResponse.responseInt) return false;

        //Do comparison in lower case
        if (contestantResponse.responseString.ToLower() != correctResponse.responseString.ToLower()) return false;
        return true;

    }

}



