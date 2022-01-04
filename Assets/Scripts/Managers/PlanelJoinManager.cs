using UnityEngine;
using UnityEngine.UI;

public class PlanelJoinManager : MonoBehaviour {

	public GameObject panelPlayerJoinedPrefab;
	public GameObject panelJoinInstruction;
	public Transform panelBlueTeamTransform;
	public Transform panelUndecidedTransform;
	public Transform panelRedTeamTransform;
	public Text textPressStart;

    #region Singletonpattern
    public static PlanelJoinManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    private void Start () {
		PlayerListManager.Instance.playerJoining.AddListener(OnPlayerJoining);
		PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
		PlayerListManager.Instance.playerChangingTeam.AddListener(OnPlayerChangingTeam);
	}

    private void OnPlayerJoining(PlayerId playerId, bool gameFull) {
        playerId.panelJoin = Instantiate(panelPlayerJoinedPrefab, panelUndecidedTransform);
		playerId.panelJoin.transform.Find("Image").GetComponent<Image>().sprite = playerId.controlUiImage;
        playerId.panelJoin.GetComponent<RectTransform>().localScale = Vector3.one;
        playerId.panelJoin.transform.Find("Text").GetComponent<Text>().text = playerId.playerName;
        if (gameFull) {
            panelJoinInstruction.SetActive(false);
        }
	}

    private void OnPlayerLeaving(PlayerId playerId) {
        if (playerId.panelJoin != null) {
            Destroy(playerId.panelJoin);
        }
        panelJoinInstruction.SetActive(true);
		textPressStart.gameObject.SetActive(PlayerListManager.Instance.IsGameReadyToStart());
	}

	private void OnPlayerChangingTeam(PlayerId playerId, StaticData.AvailableTeams oldTeam, StaticData.AvailableTeams newTeam, bool readyToStart) {
		if (playerId.panelJoin != null) {
			if (newTeam == StaticData.AvailableTeams.Blue) {
				playerId.panelJoin.transform.SetParent(panelBlueTeamTransform);
			} else {
				playerId.panelJoin.transform.SetParent(panelRedTeamTransform);
			}
			textPressStart.gameObject.SetActive(readyToStart);
		}
	}
}
