using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public TimeManager timeManager;
    public Text minutesShow;
    public Text secondsShow;
    private int minutesNumber;
    private int secondsNumber;

    public void ShowTheNumbers()
    {
        minutesShow.text = minutesNumber.ToString();
        secondsShow.text = secondsNumber.ToString();
    }

    public int MinutesNumber
    {
        get {return minutesNumber;}
        set {minutesNumber = value;}
    }
    public int SecondsNumber
    {
        get { return secondsNumber; }
        set { secondsNumber = value; }
    }

}
