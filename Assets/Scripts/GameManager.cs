using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public FinishPointController finishPoint;
    public TimeManager timeManager;
    private bool gameAlreadyOver;

    void Update()
    {
        CheckPlayerStatus();
    }

    void CheckPlayerStatus()
    {
        if (finishPoint.PlayerAlreadyFinish == 1)
        {
            finishPoint.PlayerAlreadyFinish = 0;
            LevelCompleted();
        }
        if (timeManager.TimeIsUp == true)
        {
            timeManager.TimeIsUp = false;
            GameOver();
        }
    }

    void LevelCompleted ()
    {
        gameAlreadyOver = true;
        NextSceneLoader(0);
    }

    void GameOver()
    {
        gameAlreadyOver = true;
        NextSceneLoader(1);
    }

    void NextSceneLoader(int messageCode)
    {
        //Finish Scene = 0
        //Game Over/Dead Scene = 1
        if (messageCode == 0)
        {
            SceneManager.LoadScene("Finish");
        }
        if (messageCode == 1)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
    
    public bool GameAlreadyOver
    {
        get { return gameAlreadyOver; }
        set { gameAlreadyOver = value; }
    }
}
//reupload purpose 
