using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public FinishPointController finishPoint;
    //nanti time controller disini

    private void Update()
    {
        if (finishPoint.PlayerAlreadyFinish == 1)
        {
            LevelCompleted();
        }
    }
    void LevelCompleted ()
    {
        finishPoint.PlayerAlreadyFinish = 0;
        LevelCompletedScene();
    }
    void GameOver()
    {
        Debug.Log("Game Over");
        SceneManager.LoadScene("GameOver");
    }

    void LevelCompletedScene()
    {
        SceneManager.LoadScene("Finish");
    }
}
