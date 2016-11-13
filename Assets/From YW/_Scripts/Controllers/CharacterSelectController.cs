using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectController : MonoBehaviour
{
	public static CharacterSelectController instance { get; private set; }

	public Button FirstChoice, SecondChoice, ThirdChoice, Start;
	private Button[] buttonArray = new Button[4];
	private int p1Selection, p2Selection;

	public bool ForceStart = false;

	public GameObject CharacterSelectPanel;
	private float timer;
	private int choicesMade = 0;
	private bool p1moveAgain, p2moveAgain;

	protected void Awake ()
	{
		PlayerPrefs.DeleteAll ();
		instance = this;
		p1Selection = 0;
		p2Selection = 0;

		buttonArray [0] = FirstChoice;
		buttonArray [1] = SecondChoice;
		buttonArray [2] = ThirdChoice;
		buttonArray [3] = Start;

		CharacterSelectPanel.SetActive (false);
	}

	protected void Update ()
	{
		if (CharacterSelectPanel.activeSelf) {
			timer += Time.deltaTime;
			if (timer > 1) {
				for (int i = 0; i < buttonArray.Length; i++) {
					if (i == p1Selection && i == p2Selection) {
						buttonArray [i].GetComponent<Image> ().color = new Color (1, 0, 1);
						continue;
					} else if (i == p1Selection) {
						buttonArray [i].GetComponent<Image> ().color = Color.red;
						continue;
					} else if (i == p2Selection) {
						buttonArray [i].GetComponent<Image> ().color = Color.blue;
						continue;
					} else {
						buttonArray [i].GetComponent<Image> ().color = Color.white;
					}
				}
				int v = 0;
				if (Input.GetAxisRaw ("ShipHorizontalPlayer1") > 0.7f) {
					v = 1;
				}		

				if (Input.GetAxisRaw ("ShipHorizontalPlayer1") < -0.7f) {
					v = -1;
				}

				if (p1moveAgain == true) {
					p1Selection += v;
				}
				p1moveAgain = false;	
				if (p1Selection < 0) {
					p1Selection = 0;
				} else if (p1Selection > 3) {
					p1Selection = 3;
				}
				if (v < 0.05f && v > -0.05f) {
					p1moveAgain = true;
				}

				v = 0;
				if (Input.GetAxisRaw ("ShipHorizontalPlayer2") > 0.7f) {
					v = 1;
				}		

				if (Input.GetAxisRaw ("ShipHorizontalPlayer2") < -0.7f) {
					v = -1;
				}

				if (p2moveAgain == true) {
					p2Selection += v;
				}
				p2moveAgain = false;	
				if (p2Selection < 0) {
					p2Selection = 0;
				} else if (p2Selection > 3) {
					p2Selection = 3;
				}
				if (v < 0.05f && v > -0.05f) {
					p2moveAgain = true;
				}

				if (Input.GetAxisRaw ("Submit") > 0.7f) {
					MakeDecision (PlayerEnum.Player1, p1Selection);
					timer = 0;
				}
				if (Input.GetAxisRaw ("SubmitP2") > 0.7f) {
					MakeDecision (PlayerEnum.Player2, p2Selection);
					timer = 0;
				}
			}
		}
	}

	private void MakeDecision (PlayerEnum type, int choice)
	{
		if (choicesMade == 2) {
			SceneManager.LoadScene ("0", 0);
			return;
		}
		if (PlayerPrefs.GetInt (type.ToString () + "Choice", -1) == -1) {
			PlayerPrefs.SetInt (type.ToString () + "Choice", choice);
			choicesMade++;
		}
	}

	public void Activate ()
	{
		if (ForceStart == true) {
			PlayerPrefs.SetInt ("Player1Choice", 0);
			PlayerPrefs.SetInt ("Player2Choice", 0);
			SceneManager.LoadScene ("0", 0);
		}
		CharacterSelectPanel.SetActive (true);
	}


}
