using UnityEngine;

public class TimeScaleManager : MonoBehaviour {
    
    #region Singletonpattern
    public static TimeScaleManager Instance { get; private set; }

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

    private void Start() {
		if (GameStatesManager.Instance != null) {
			GameStatesManager.Instance.GameStateChanged.AddListener(OnGameStateChange);
			OnGameStateChange();
		}
	}

	private void OnGameStateChange() {
		if (GameStatesManager.gameState == StaticData.AvailableGameStates.Pausing) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
	}
}
