using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndController : MonoBehaviour
{
	public GameObject GameEndPanel;
	public Text ScoreText, MultiplierText, TitleText;
	public Button ToMainMenu;
	public InputField nameInput;
	public string MainMenuSceneString = "MainMenuScene";
	public bool activate;

	public static GameEndController instance { get; private set; }

	protected void Awake ()
	{
		instance = this;
		GameEndPanel.SetActive (false);
	}

	protected void Update ()
	{
		if (activate) {
			Activate ("You Win");
			activate = false;
		}
	}

	public void Activate (string title)
	{
		GameEndPanel.SetActive (true);
		TitleText.text = title;
		ScoreText.text = "Score :" + GameManager.instance.Score;
		MultiplierText.text = "Multiplier :" + GameManager.instance.Multiplier;
		ToMainMenu.onClick.AddListener (MainMenuButtonPressed);
	}

	private void MainMenuButtonPressed ()
	{
		Score s = new Score ();
		s.name = nameInput.textComponent.text;
		s.score = GameManager.instance.Score;

		ScoreBoardController.instance.sb.scores.Add (s);
		ScoreBoardController.instance.sb.Save (Application.persistentDataPath + "/Scores.xml");

		SceneManager.LoadScene (MainMenuSceneString);
	}
}
