using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausePanelController : MonoBehaviour
{
	public GameObject pausePanel;
	private bool paused;
	private	float pauseTimer, pause;

	protected void Start ()
	{
		pausePanel.SetActive (false);
	}

	protected void Update ()
	{
		pauseTimer++;
		pause = Input.GetAxisRaw ("Pause");
		if ((pause > 0 || Input.GetKeyDown (KeyCode.JoystickButton7)) && paused == false && pauseTimer > 60) {
			pausePanel.SetActive (true);
			paused = true;
			pauseTimer = 0;
			Time.timeScale = 0;
		} else if ((pause > 0 || Input.GetKeyDown (KeyCode.JoystickButton7)) && paused == true && pauseTimer > 60) {
			pausePanel.SetActive (false);
			paused = false;
			Time.timeScale = 1;
			pauseTimer = 0;
		}
	}
}
