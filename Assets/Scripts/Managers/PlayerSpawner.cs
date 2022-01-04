using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {

	public GameObject avatarPrefab;
	public List<GameObject> availableBlueTeamSpawns;
	public List<GameObject> availableRedTeamSpawns;

    #region Singletonpattern
    public static PlayerSpawner Instance { get; private set; }

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

    // Use this for initialization
    private void Start () {
		PlayerListManager.Instance.playerChangingTeam.AddListener(OnPlayerChangingTeam);
		PlayerListManager.Instance.playerLeaving.AddListener(OnPlayerLeaving);
	}

	private void OnPlayerChangingTeam(PlayerId playerId, StaticData.AvailableTeams oldTeam, StaticData.AvailableTeams newTeam, bool readyToStart) {
		ReturnSpawnPositionToAvailableSpawnList(playerId, oldTeam);
		playerId.spawnPosition = GetAvailableSpawnPosition(newTeam);
		if (playerId.avatar != null) {
			playerId.avatar.transform.position = playerId.spawnPosition.transform.position;
		} else {
			playerId.avatar = Instantiate(avatarPrefab, playerId.spawnPosition.transform.position, Quaternion.identity);
			playerId.avatar.GetComponent<Player>().playerId = playerId;
		}
	}

	private void ReturnSpawnPositionToAvailableSpawnList(PlayerId playerId, StaticData.AvailableTeams oldTeam) {
		if (oldTeam == StaticData.AvailableTeams.Blue) {
			availableBlueTeamSpawns.Add(playerId.spawnPosition);
		} else if (oldTeam == StaticData.AvailableTeams.Red) {
			availableRedTeamSpawns.Add(playerId.spawnPosition);
		}
	}

	private GameObject GetAvailableSpawnPosition(StaticData.AvailableTeams team) {
		if (team == StaticData.AvailableTeams.Blue) {
			int rand = Random.Range(0, availableBlueTeamSpawns.Count);
			GameObject spawnGo = availableBlueTeamSpawns[rand];
			availableBlueTeamSpawns.RemoveAt(rand);
			return spawnGo;
		} else {
			int rand = Random.Range(0, availableRedTeamSpawns.Count);
			GameObject spawnGo = availableRedTeamSpawns[rand];
			availableRedTeamSpawns.RemoveAt(rand);
			return spawnGo;
		}
	}

	private void OnPlayerLeaving(PlayerId playerId) {
		if (playerId.avatar != null) {
			if (playerId.team == StaticData.AvailableTeams.Blue) {
				availableBlueTeamSpawns.Add(playerId.spawnPosition);
			} else if (playerId.team == StaticData.AvailableTeams.Red) {
				availableRedTeamSpawns.Add(playerId.spawnPosition);
			}
			Destroy(playerId.avatar);
			playerId.avatar = null;
		}
	}
}
