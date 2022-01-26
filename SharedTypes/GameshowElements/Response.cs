using System;
using UnityEngine;
using System.Collections;

[Serializable]
public struct ResponseData
{
    public string responseString;
    public bool responseBool;
    public int responseInt;

    public ResponseData(string _rstring, bool _rbool, int _rint)
    {
        responseString = _rstring;
        responseBool = _rbool;
        responseInt = _rint;
    }

}

[Serializable]
public class Response
{

    [HideInInspector]
    public Question question;

    [HideInInspector]
    public InputRecorder inputRecorder;

    [HideInInspector]
    public float timeAllowance;

    public Contestant respondant;
    public ResponseData responseData;

    public Response(Contestant _respondant, Question _question, float _timeAllowance)
    {
        respondant = _respondant;
        question = _question;
        timeAllowance = _timeAllowance;

        inputRecorder = GetInputsFromDevice(_respondant.device);
    }

    public virtual InputRecorder GetInputsFromDevice(Device device)
    {
        //Instantiate the response template into the device
        inputRecorder = GameObject.Instantiate(question.responseTemplate, Vector3.zero, Quaternion.Euler(Vector3.zero), device.responseArea).GetComponent<InputRecorder>();
        inputRecorder.transform.localPosition = Vector3.zero;
        inputRecorder.transform.localRotation = Quaternion.Euler(Vector3.zero);
        inputRecorder.parentResponse = this;

        return inputRecorder;
    }

    public void SendResponse()
    {
        responseData = new ResponseData(inputRecorder.recordedString, inputRecorder.recordedBool, inputRecorder.recordedInt);
        Brain.ins.eventManager.e_responseConfirmed.Invoke(this);
    }
}

[Serializable]
public class StringResponse : Response
{
    public StringResponse(Contestant _respondant, Question _question, float _timeAllowance) : base(_respondant, _question, _timeAllowance)
    {

    }
}

[Serializable]
public class BoolResponse : Response
{
    public BoolResponse(Contestant _respondant, Question _question, float _timeAllowance) : base(_respondant, _question, _timeAllowance)
    {

    }
}

[Serializable]
public class IntResponse : Response
{
    public IntResponse(Contestant _respondant, Question _question, float _timeAllowance) : base(_respondant, _question, _timeAllowance)
    {

    }
}