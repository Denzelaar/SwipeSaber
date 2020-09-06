using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjBlink : MonoBehaviour
{
    Text text;

    [Range(0.0f, 2.0f)]
    public float increase = .1f;

    [Range(0.0f, 20.0f)]
    public int amountOfCor = 5;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();

        PhaseManager.NewPhase += OnNewPhase;
    }

    void ActivateBlink(float timeIncrease, int amountOfCoroutines)
    {
        text.enabled = true;

        for (int i = 1; i < amountOfCoroutines; i++)
        {
            StartCoroutine(ToggleActive(i * timeIncrease));
        }

        StartCoroutine(ToggleActive(amountOfCoroutines * timeIncrease, true)); //make sure the last coroutine turns the text off

    }

    void OnNewPhase(PhaseDetails phaseDetails)
    {
        StopAllCoroutines();

        if (text.enabled)
        {
            text.enabled = false;
        }

        ActivateBlink(increase, amountOfCor);

        text.text = "New " + phaseDetails.PhaseName + " Phase!";
    }

    IEnumerator ToggleActive(float sec, bool final = false)
    {
        yield return new WaitForSeconds(sec);

        if (final)
        {
            if (text.enabled)
            {
                text.enabled = false;
            }
        }
        else
        {
            text.enabled = !text.enabled;
        }
    }
}
