using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public float timeRemaining = 90;
    public bool timerIsRunning = false;
    string FinishedTime;
    bool Finish;
    public bool GameIsOver = false;

    private void start()
    {
        timerIsRunning = true;
    }
    // Update is called once per frame
    void Update()
    {
        Finish = FinishState.isFinished;
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);

                if (Finish == true)
                {
                    timerIsRunning = false;
                    Debug.Log("Timer stoped at " + FinishedTime);
                }
            }
            else{
                Debug.Log("Time has Run Out!");
                timeRemaining = 0;
                timerIsRunning= false;
                Time.timeScale = 0f;
                GameIsOver = true;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay +=1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        FinishedTime = string.Format("{0:00} m : {1:00} s", minutes, seconds);
        timerText.text = FinishedTime;

    }
}