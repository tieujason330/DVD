using UnityEngine;
using System.Collections;

public class PlayManager : MonoBehaviour 
{
	public Commander[] playerGroup; 
	private string[] animClipNameGroup;
	private int currentNumber;

	// Use this for initialization
	void Start ()
    {
	}


	void OnGUI()
	{
		if(GUI.Button(new Rect(50,50,50,50),"<"))
		{
			currentNumber--;

			if(currentNumber < 0 )
			{
				currentNumber = animClipNameGroup.Length - 1;
			}

			for(int i = 0; i < playerGroup.Length; i++)
			{
                //playerGroup[i].speed = 1f;
                //playerGroup[i].Play(animClipNameGroup[currentNumber]);
                //playerGroup[i].PerformAction(animClipNameGroup[currentNumber]);

            }

		}

		if(GUI.Button(new Rect(160,50,50,50),">"))
		{
			currentNumber++;

			if(currentNumber == animClipNameGroup.Length)
			{
				currentNumber = 0;
			}

			for(int i = 0; i < playerGroup.Length; i++)
			{
                //playerGroup[i].speed = 1f;
                //playerGroup[i].Play(animClipNameGroup[currentNumber]);
                //playerGroup[i].PerformAction(animClipNameGroup[currentNumber]);
            }
		}

		GUI.Label (new Rect(240, 50, 200,100), animClipNameGroup[currentNumber].ToString() );

	}
}
