using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene("Play");
    }
}
