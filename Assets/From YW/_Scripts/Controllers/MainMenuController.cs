using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
	public Button StartGameButton, ScoreBoardButton, EndGameButton;
	public GameObject MainMenu, ScoreBoard, CharacterSelect;

	public static MainMenuController instance { get; private set; }

	private bool moveAgain;
	private int selection = 0;
	private float timer;
	private Button[] buttonArray = new Button[3];

	protected void Awake ()
	{
		instance = this;
	}

	protected void Start ()
	{
		MainMenu.SetActive (true);

		buttonArray [0] = StartGameButton;
		buttonArray [1] = ScoreBoardButton;
		buttonArray [2] = EndGameButton;

		StartGameButton.onClick.AddListener (StartButtonPressed);
		ScoreBoardButton.onClick.AddListener (ScoreBoardButtonPressed);
		EndGameButton.onClick.AddListener (EndGameButtonPressed);
	}

	protected void Update ()
	{
		if (MainMenu.activeSelf) {
			timer += Time.deltaTime;
			if (timer > 1) {
				int v = 0;
				if (Input.GetAxisRaw ("ShipVerticalPlayer1") > 0.7f) {
					v = 1;
				}		

				if (Input.GetAxisRaw ("ShipVerticalPlayer1") < -0.7f) {
					v = -1;
				}
	
				if (moveAgain == true) {
					selection -= v;
				}
				moveAgain = false;	
				if (selection < 0) {
					selection = 0;
				} else if (selection > 2) {
					selection = 2;
				}
				if (v < 0.05f && v > -0.05f) {
					moveAgain = true;
				}
				for (int i = 0; i < buttonArray.Length; i++) {
					if (i == selection) {
						buttonArray [i].GetComponent<Image> ().color = Color.red;
					} else {
						buttonArray [i].GetComponent<Image> ().color = Color.white;
					}
				}

				if (Input.GetAxisRaw ("Submit") > 0.7f) {
					buttonArray [selection].onClick.Invoke ();
					timer = 0;
				}
			}
		}
	}

	private void StartButtonPressed ()
	{
		CharacterSelectController.instance.Activate ();
		MainMenu.SetActive (false);
	}

	private void ScoreBoardButtonPressed ()
	{
		ScoreBoardController.instance.Activate ();
		MainMenu.SetActive (false);
	}

	private void EndGameButtonPressed ()
	{
		Application.Quit ();
	}
}
