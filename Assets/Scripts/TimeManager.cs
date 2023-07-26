using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameManager manager;
    public TimeController timeController;
    public float timeInSecond;
    private float timeLeft;
    private float timeLeftEditable;
    private float timeReducer;
    private bool timeIsUp = false;
    private int minutesFromTime;
    private int secondsFromTime;

    private int SECONDREDUCER = 1; //waktu berkurang per detik
    
    void Start()
    {
        timeLeft = timeInSecond;
        timeLeftEditable = timeInSecond;
        TimeInitialTranslate();
        //Debug.Log("Time left : " + minutesFromTime + " : " + secondsFromTime + ".");
    }
    void Update()
    {
        TimeReducing();
        TimeTranslateProcessing();
        IsTimeIsUp();
    }

    void TimeInitialTranslate()
    {
        minutesFromTime = (int)timeInSecond/60;
        secondsFromTime = (int)timeInSecond%60;
        SendNumbersToTimeController();
        timeController.ShowTheNumbers();
    }

    void TimeTranslateProcessing()
    {
        if (timeLeft <= timeLeftEditable - SECONDREDUCER)
        {
            secondsFromTime -= SECONDREDUCER;
            timeLeftEditable -= SECONDREDUCER;
            if (secondsFromTime < 0)
            {
                if (minutesFromTime > 0)
                {
                    minutesFromTime -= 1;
                    secondsFromTime = 59;
                }
                else if (minutesFromTime <= 0)
                {
                    secondsFromTime = 0;
                }
                
            }
            SendNumbersToTimeController();
            timeController.ShowTheNumbers();
            //Debug.Log("Time left : " + minutesFromTime + " : " + secondsFromTime + ".");
        } 
    }

    void SendNumbersToTimeController()
    {
        timeController.MinutesNumber = minutesFromTime;
        timeController.SecondsNumber = secondsFromTime;
    }

    void IsTimeIsUp()
    {
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            timeReducer = 0;
            timeIsUp = true;
        }
    }

    void TimeReducing()
    {
        if (timeLeft <= 0 || manager.GameAlreadyOver == true)
        {
            return;
        }
        else if (timeLeft > 0)
        {
            timeReducer = Time.deltaTime;
            timeLeft -= timeReducer;
        }
    }

    void ResetTimer()
    {
        timeLeft = timeInSecond;
        TimeInitialTranslate();
    }

    public bool TimeIsUp
    {
        get { return timeIsUp; }
        set { timeIsUp = value; }
    }
}
