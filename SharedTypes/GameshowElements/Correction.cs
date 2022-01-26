using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//Created whenver we are undoing a correction
public class Correction
{
    public List<Ask> correctibles;

    //Create a new correction
    public Correction(List<Ask> _correctibles, bool _isRoundEnd)
    {
        correctibles = _correctibles;
        //Call a new routine event and pass our correction process
        Brain.ins.eventManager.e_newRoutine.Invoke(CorrectAsks(_isRoundEnd));


    }


    IEnumerator CorrectAsks(bool isRoundEnd)
    {

        Debug.Log("New correction even was started.");



        //Stage manager build correction



        int ind = 0;

        while (ind < correctibles.Count)
        {

            //Do a correction thing
            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => Input.GetButtonUp("Master Advance"));

            //Pull data from the correctible
            Ask ask = correctibles[ind];



            Debug.Log("Currently correcting ask: " + ind);

            //Get a new gameobject instance of the question
            GameObject questionInstance = correctibles[ind].question.GetNewInstance(3);
            questionInstance.GetComponentInChildren<Timer>().gameObject.SetActive(false);

            questionInstance.transform.localPosition = new Vector3(-4f, 0f, 0f);
            LeanTween.moveLocalX(questionInstance, 0f, 0.35f).setEase(LeanTweenType.easeOutExpo);

            yield return new WaitForEndOfFrame();
            yield return new WaitUntil(() => Input.GetButtonUp("Master Advance"));


            //Do correction events here

            //Go through each response in the correctible being corrected
            foreach (Response response in ask.responses.Keys.ToList())
            {

                bool correct = ask.question.CompareResponse(response.responseData);

                if (correct)
                {
                    Debug.Log(response.respondant.name + " answered correctly.");
                    yield return new WaitForEndOfFrame();
                    yield return new WaitUntil(() => Input.GetButtonUp("Master Advance"));
                    //If returns true - award points to the respondant
                    Brain.ins.eventManager.e_addPoints.Invoke(response.respondant, 5f);
                }

                else
                {
                    Debug.Log(response.respondant.name + " answered incorrectly.");
                    yield return new WaitForEndOfFrame();
                    yield return new WaitUntil(() => Input.GetButtonUp("Master Advance"));
                }
            }

            // //Set corrected
            // correctibles[ind].isCorrected = true;
            //Destroy the instance
            GameObject.Destroy(questionInstance);

            //Iterate to next correctible
            ind++;

            yield return null;
        }

        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => Input.GetButtonUp("Master Advance"));

        if (isRoundEnd) Brain.ins.eventManager.e_goHubSegment.Invoke();
        else Brain.ins.eventManager.e_goRoundSegment.Invoke();
    }






}
