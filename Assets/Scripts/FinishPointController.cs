using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPointController : MonoBehaviour
{
	public Collider thePlayer;
	private int playerAlreadyFinish;
	void OnTriggerEnter(Collider whosComing)
	{
		if (whosComing == thePlayer)
		{
			Debug.Log("Sampe finish");
			playerAlreadyFinish = 1;
		}
	}

	public int PlayerAlreadyFinish 
    {
		get { return playerAlreadyFinish; } 
		set { playerAlreadyFinish = value; }
	}
}
