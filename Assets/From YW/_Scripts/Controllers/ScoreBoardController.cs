using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;
using System;

public class ScoreBoardController : MonoBehaviour
{
	public ScoreBoard sb;
	public string fileName = "Scores";
	public GameObject ScoreBoardPanel;
	public GameObject BoardPanel;
	public Button BackButton;

	private float timer;

	public static ScoreBoardController instance { get; private set; }

	protected void Awake ()
	{
		instance = this;
		ScoreBoardPanel.SetActive (false);
		if (File.Exists (Application.persistentDataPath + "/" + fileName + ".xml")) {
			sb = ScoreBoard.Load (Application.persistentDataPath + "/" + fileName + ".xml");
		} else {
			sb = new ScoreBoard ();
		}
		if (BackButton != null) {
			BackButton.onClick.AddListener (BackButtonPressed);
		}
	}

	protected void Update ()
	{
		if (ScoreBoardPanel.activeSelf) {
			timer += Time.deltaTime;
			BackButton.GetComponent<Image> ().color = Color.red;
			if (timer > 1) {
				if (Input.GetAxisRaw ("Cancel") > 0.7f || Input.GetAxisRaw ("Submit") > 0.7f) {
					BackButton.onClick.Invoke ();
					timer = 0;
				}
			}
		}
	}

	public void Activate ()
	{
		sb.SortList ();
		for (int i = 0; i < BoardPanel.transform.childCount; i++) {
			if (i >= sb.scores.Count) {
				BoardPanel.transform.GetChild (i).gameObject.SetActive (false);
				continue;
			}
			BoardPanel.transform.GetChild (i).gameObject.transform.GetChild (1).GetComponent<Text> ().text = sb.scores [i].name;
			BoardPanel.transform.GetChild (i).gameObject.transform.GetChild (2).GetComponent<Text> ().text = sb.scores [i].score.ToString ();
		}

		ScoreBoardPanel.SetActive (true);
	}

	private void BackButtonPressed ()
	{
		ScoreBoardPanel.SetActive (false);
		MainMenuController.instance.MainMenu.SetActive (true);	
	}
}

public class ScoreBoard
{
	[XmlArray ("Scores")]
	[XmlArrayItem ("Score")]
	public List<Score> scores = new List<Score> ();

	public void Save (string path)
	{
		SortList ();
		var serializer = new XmlSerializer (typeof(ScoreBoard));
		using (var stream = new FileStream (path, FileMode.Create)) {
			serializer.Serialize (stream, this);	
		}
	}

	public static ScoreBoard Load (string path)
	{
		var serializer = new XmlSerializer (typeof(ScoreBoard));
		using (var stream = new FileStream (path, FileMode.Open)) {
			return serializer.Deserialize (stream) as ScoreBoard;	
		}
	}

	public void SortList ()
	{
		scores.Sort ((x, y) => -x.score.CompareTo (y.score));
	}
}

public class Score
{
	public string name;
	public int score;

	public Score ()
	{
		name = "Admin";
		score = 9999;
	}

	public Score (int i)
	{
		name = "Admin2";
		score = 6666;
	}

	public Score (string s)
	{
		name = "Admin3";
		score = 8888;
	}
}
