using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanelController : MonoBehaviour
{
	public Text MultiplierText, ScoreText;
	public Image LifesLeft;
	public GameObject HUDPanel;

	protected void Start ()
	{
		HUDPanel.SetActive (true);
	}

	protected void Update ()
	{
		MultiplierText.text = GameManager.instance.Multiplier.ToString ();
		ScoreText.text = GameManager.instance.Score.ToString ();
		LifesLeft.fillAmount = GameManager.instance.GetLifesLeftPercentage ();
	}
}
