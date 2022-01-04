using UnityEngine;
using UnityEngine.UI;

public class HealthBarsManager : MonoBehaviour {

	public GameObject panelHealthBarPrefab;

    #region Singletonpattern
    public static HealthBarsManager Instance { get; private set; }

    private void Awake() {
		if (Instance == null) {
			Instance = this;
		} else if (Instance != this) {
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
	}
    #endregion

    private void Start()
    {
        PlayerListManager.Instance.playerChangingTeam.AddListener(OnPlayerChangingTeam);
        PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
    }

	private void OnPlayerChangingTeam(PlayerId playerId, StaticData.AvailableTeams oldTeam, StaticData.AvailableTeams newTeam, bool readyToStart) {
		if (playerId.panelHealthBar == null) {
			playerId.panelHealthBar = Instantiate(panelHealthBarPrefab, CanvasManager.Instance.panelPlaying.transform);
			playerId.greenHealthBar = playerId.panelHealthBar.transform.Find("Panel Green").gameObject.GetComponent<Image>();
		}
		Text textName = playerId.panelHealthBar.transform.Find("Text Name").gameObject.GetComponent<Text>();
		textName.text = playerId.name.ToUpper();
		switch (newTeam) {
			case StaticData.AvailableTeams.Blue:
				textName.color = Color.blue;
				break;
			case StaticData.AvailableTeams.Red:
				textName.color = Color.red;
				break;
			default:
				Debug.LogWarning("A healthbar is spawning without a team. This should not happen.");
				textName.color = Color.black;
				break;
		}
	}

	private void OnPlayerLeaving(PlayerId playerId) {
		if (playerId.panelHealthBar != null) {
			Destroy(playerId.panelHealthBar);
		}
	}
}
