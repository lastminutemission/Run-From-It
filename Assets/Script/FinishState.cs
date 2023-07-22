using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishState : MonoBehaviour
{
    public GameObject Player;
    public static bool isFinished = false;
    public bool GameIsOver = false;

    [SerializeField]
    string TagName;
    // Start is called before the first frame update
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == TagName)
        {            
            isFinished = true;
            Debug.Log("Finish!");
            Time.timeScale = 0f;
            GameIsOver = true;
        }
    }

}
