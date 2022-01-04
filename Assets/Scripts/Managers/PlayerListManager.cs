using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class PlayerJoiningEvent : UnityEvent<PlayerId, bool> {}

[System.Serializable]
public class PlayerLeavingEvent : UnityEvent<PlayerId> {}

[System.Serializable]
public class PlayerChangingTeamEvent : UnityEvent<PlayerId, StaticData.AvailableTeams, StaticData.AvailableTeams, bool> {}

public class PlayerListManager : MonoBehaviour {
	public int maxNumPlayers;
	public int currentPlayerCount;
	public Vector3 playerCenter;
	public Bounds playerBounds;
	public float maxDistanceBetweenPlayers;
	public List<PlayerId> listOfPlayers {get; private set;}
    public List<PlayerId> listOfAvailablePlayers;
    public PlayerJoiningEvent playerJoining = new PlayerJoiningEvent();
    public PlayerLeavingEvent playerLeaving = new PlayerLeavingEvent();
	public PlayerChangingTeamEvent playerChangingTeam = new PlayerChangingTeamEvent();

    #region Singletonpattern
    public static PlayerListManager Instance { get; private set; }

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
		currentPlayerCount = 0;
		playerCenter = Vector3.zero;
		playerBounds = new Bounds();
        maxNumPlayers = Mathf.Min(maxNumPlayers, listOfAvailablePlayers.Count);
        listOfPlayers = new List<PlayerId> ();
    }

    private void Update() {
		if (GameStatesManager.gameState == StaticData.AvailableGameStates.Menu) {
			//[temp]
			if (Input.GetKeyDown(KeyCode.F1)) {
                DebugAddPlayers();
			}
			//[/temp]
			for (int i = listOfPlayers.Count - 1; i >= 0; i--) {
				if (listOfPlayers[i].controls.GetButtonBDown()) {
					RemovePlayer(listOfPlayers[i]);
				} else {
					if (Mathf.Abs(listOfPlayers[i].controls.GetLHorizontal()) >= 0.415f) {
						if (listOfPlayers[i].controls.GetLHorizontal() > 0.0f) {
							MoveToTeam(listOfPlayers[i], StaticData.AvailableTeams.Red);
						} else {
							MoveToTeam(listOfPlayers[i], StaticData.AvailableTeams.Blue);
						}
					}
					if (listOfPlayers[i].controls.GetButtonStartDown()) {
						if (IsGameReadyToStart()) {
							GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Starting);
						}
					}
				}
			}
			for (int i = listOfAvailablePlayers.Count - 1; i >= 0; i--) {
				if (listOfAvailablePlayers[i].controls.GetButtonADown()) {
					AddPlayer(listOfAvailablePlayers[i]);
				}
			}
		}
        else
        {
			if (GameStatesManager.gameState == StaticData.AvailableGameStates.Ending) {
				for (int i = listOfAvailablePlayers.Count - 1; i >= 0; i--) {
					if (listOfAvailablePlayers[i].controls.GetButtonStartDown()) {
						GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Menu);
						RemoveAllPlayer();
						return;
					}
				}
				for (int i = listOfPlayers.Count - 1; i >= 0; i--) {
					if (listOfPlayers[i].controls.GetButtonStartDown()) {
						GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Menu);
						RemoveAllPlayer();
						return;
					}
				}
			}

            
			if (currentPlayerCount != 0) {
				ComputePlayerBounds();
			}
		}
    }

    //This whole region need to be removed
    #region DebugAddPlayers 
    public PlayerId PlayerKeyboard1;
    public PlayerId PlayerKeyboard2;
    public bool isDebugging;
    private void DebugAddPlayers()
    {
        isDebugging = true;
        foreach (PlayerId playerId in listOfPlayers)
        {
            if (playerId.team == StaticData.AvailableTeams.Undecided)
            {
                MoveToTeam(playerId, StaticData.AvailableTeams.Blue);
            }
        }
        if (listOfAvailablePlayers.Contains(PlayerKeyboard1))
        {
            AddPlayer(PlayerKeyboard1);
        }
        if (listOfAvailablePlayers.Contains(PlayerKeyboard2))
        {
            AddPlayer(PlayerKeyboard2);
        }
        MoveToTeam(PlayerKeyboard1, StaticData.AvailableTeams.Blue);
        MoveToTeam(PlayerKeyboard2, StaticData.AvailableTeams.Red);
        GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Playing);
    }
    #endregion

    //Moves a player to the passed team
    private void MoveToTeam(PlayerId playerId, StaticData.AvailableTeams team) {
		if (playerId.team != team) {
			if (!IsTeamFull(team)) {
				AudioManager.Instance.PlayClip(AudioManager.Instance.menuClickClip);
				StaticData.AvailableTeams oldTeam = playerId.team;
				playerId.team = team;
				playerChangingTeam.Invoke(playerId, oldTeam, team, IsGameReadyToStart());
			} else {
				AudioManager.Instance.PlayClip(AudioManager.Instance.warningClip);
			}
		}
	}

	//Returns true if the passed team is full
	private bool IsTeamFull(StaticData.AvailableTeams team) {
		int numberOfPlayersInTeam = 0;
		foreach (PlayerId playerId in listOfPlayers) {
			if (playerId.team == team) {
				numberOfPlayersInTeam++;
			}
		}
		if (numberOfPlayersInTeam >= (maxNumPlayers - 1)) {
			return true;
		} else {
			return false;
		}
	}

	//Returns true if the game is ready to start
	public bool IsGameReadyToStart() {
		if (listOfPlayers.Count < 2) {
			return false;
		} else if (!IsEveryoneReady()) {
			return false;
		} else if (!AreTherePlayersInBothTeams()) {
			return false;
		}
		return true;
	}

	//Returns true if every player are ready
	private bool IsEveryoneReady() {
		foreach (PlayerId p in listOfPlayers) {
			if (p.team == StaticData.AvailableTeams.Undecided) {
				return false;
			}
		}
		return true;
	}

	//Returns true if both team has players
	private bool AreTherePlayersInBothTeams() {
		bool blueTeam = false;
		bool redTeam = false;
		foreach (PlayerId p in listOfPlayers) {
			if (p.team == StaticData.AvailableTeams.Blue) {
				blueTeam = true;
			} else if (p.team == StaticData.AvailableTeams.Red) {
				redTeam = true;
			}
		}
		if (blueTeam && redTeam) {
			return true;
		} else {
			return false;
		}
	}

	//Returns a list of teams that have player alive
	public List<StaticData.AvailableTeams> GetListOfTeamsWithPlayerAlive() {
		List<StaticData.AvailableTeams> listOfTeamsWithPlayerAlive = new List<StaticData.AvailableTeams>();
		foreach (PlayerId playerId in listOfPlayers) {
			if (playerId.team != StaticData.AvailableTeams.Undecided) {
				if (!listOfTeamsWithPlayerAlive.Contains(playerId.team)) {
					listOfTeamsWithPlayerAlive.Add(playerId.team);
				}
			}
		}
		return listOfTeamsWithPlayerAlive;
	}

	//Adds a player to the game
	private void AddPlayer(PlayerId playerId) {
		if (listOfPlayers.Count < maxNumPlayers) {
			AudioManager.Instance.PlayClip(AudioManager.Instance.playerJoiningClip);
            listOfPlayers.Add(playerId);
			playerId.team = StaticData.AvailableTeams.Undecided;
            listOfAvailablePlayers.Remove(playerId);
            bool gameFull = (listOfPlayers.Count < maxNumPlayers) ? false : true;
			playerJoining.Invoke(playerId, gameFull);
			currentPlayerCount = listOfPlayers.Count;

            ResetPlayerData(playerId);
        }
	}

    private void ResetPlayerData(PlayerId playerId) {
        playerId.currentHealth = playerId.maxHealth;
        playerId.ammo = playerId.maxAmmo;
        playerId.armor = playerId.startingArmor;
        playerId.weapon = playerId.startingWeapon;
        playerId.looting = false;
    }

    //Removes a player from the game
    private void RemovePlayer(PlayerId playerId) {
		Debug.Log("Removing player " + playerId.name);
		if (GameStatesManager.gameState == StaticData.AvailableGameStates.Menu) {
			AudioManager.Instance.PlayClip(AudioManager.Instance.playerLeavingClip);
		}
		listOfAvailablePlayers.Add(playerId);
        listOfPlayers.Remove(playerId);
		playerLeaving.Invoke(playerId);
		if (GameStatesManager.gameState == StaticData.AvailableGameStates.Playing) {
			if (!AreTherePlayersInBothTeams()) {
				GameStatesManager.Instance.ChangeGameStateTo(StaticData.AvailableGameStates.Ending);
			}
		}
		currentPlayerCount = listOfPlayers.Count;
	}

	//Removes all player from the game
	private void RemoveAllPlayer() {
		for (int i = listOfPlayers.Count - 1; i >= 0; i--) {
			RemovePlayer(listOfPlayers[i]);
		}
	}

    //This is call every frame. If it still in use it should probably belong to another script
    private void ComputePlayerBounds() {
		playerBounds = new Bounds(PlayerListManager.Instance.listOfPlayers[0].avatar.transform.position, Vector3.zero);
		for (int i = 1; i < PlayerListManager.Instance.currentPlayerCount; i++) {
			playerBounds.Encapsulate(PlayerListManager.Instance.listOfPlayers[i].avatar.transform.position);
		}
		playerCenter = playerBounds.center;
		maxDistanceBetweenPlayers = Mathf.Max(playerBounds.size.x, playerBounds.size.y);
	}

	public void PlayerDied(PlayerId playerId) {
		RemovePlayer(playerId);
	}
}
