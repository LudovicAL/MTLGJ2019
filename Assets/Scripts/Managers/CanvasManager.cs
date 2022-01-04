using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {
	public static CanvasManager Instance { get; private set; }

	public GameObject panelMenu;
	public GameObject panelJoin;
	public GameObject panelStarting;
	public GameObject panelPlaying;
	public GameObject panelPausing;
	public GameObject panelEnding;
	public Text textCountDown;
	public Text textEnding;

    #region Singletonpattern
    private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
    #endregion

    void Start() {
		if (GameStatesManager.Instance != null) {
			GameStatesManager.Instance.GameStateChanged.AddListener(OnGameStateChange);
			OnGameStateChange();
		}
	}

	private void OnGameStateChange() {
		switch (GameStatesManager.gameState) {
			case (StaticData.AvailableGameStates.Menu):
				ShowPanel(panelMenu);
				break;
			case (StaticData.AvailableGameStates.Starting):
				ShowPanel(panelStarting);
				panelPlaying.SetActive(true);
				StartCoroutine(CountDown());
				break;
			case (StaticData.AvailableGameStates.Playing):
				ShowPanel(panelPlaying);
				break;
			case (StaticData.AvailableGameStates.Pausing):
				ShowPanel(panelPausing);
				break;
			case (StaticData.AvailableGameStates.Ending):
				AudioManager.Instance.PlayClip(AudioManager.Instance.victoryClip);
				ShowPanel(panelEnding);
				SetEndingText();
				break;
		}
	}

    #region Ending
    private void SetEndingText() {
		List<StaticData.AvailableTeams> listOfTeamsWithPlayerAlive = PlayerListManager.Instance.GetListOfTeamsWithPlayerAlive();
		switch (listOfTeamsWithPlayerAlive.Count) {
			case 0: //Everybody is dead
				textEnding.text = "Yay! Everybody is dead!";
				SetEndingTextColor(StaticData.AvailableTeams.Undecided);
				break;
			case 1: //One team still has player alive
				textEnding.text = listOfTeamsWithPlayerAlive[0].ToString() + " team won!";
				SetEndingTextColor(listOfTeamsWithPlayerAlive[0]);
				break;
			default:    //Two team still have player alive on game ending. This should not happen.
				textEnding.text = "Two team still have player alive on game ending. This should not happen. Call out the nearest programmer on his bullshit.";
				SetEndingTextColor(StaticData.AvailableTeams.Undecided);
				Debug.LogWarning("Two team still have player alive on game ending. This should not happen.");
				break;
		}
	}

	private void SetEndingTextColor(StaticData.AvailableTeams team) {
		switch (team) {
			case StaticData.AvailableTeams.Blue:
				textEnding.color = Color.blue;
				break;
			case StaticData.AvailableTeams.Red:
				textEnding.color = Color.red;
				break;
			case StaticData.AvailableTeams.Undecided:
				textEnding.color = Color.black;
				break;
		}
	}
    #endregion

    private void ShowPanel(GameObject panel) {
		panelMenu.SetActive(false);
		panelStarting.SetActive(false);
		panelPlaying.SetActive(false);
		panelPausing.SetActive(false);
		panelEnding.SetActive(false);
		panel.SetActive(true);
		if (panel == panelPausing) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
	}

	private IEnumerator CountDown() {
		AudioManager.Instance.PlayClip(AudioManager.Instance.ticTacClip);
		for (int i = 3; i > 0; i--) {
			textCountDown.text = i.ToString();
			yield return new WaitForSeconds(0.7f);
		}
		GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Playing);
	}

	public void OnStartButton() {
		GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Starting);
	}

	public void OnQuitButton() {
		Application.Quit();
	}
}