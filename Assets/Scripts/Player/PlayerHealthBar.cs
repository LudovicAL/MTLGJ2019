using UnityEngine;

public class PlayerHealthBar : MonoBehaviour {

	public Vector3 offset = new Vector3(0.0f, 1.0f, 0.0f);
    public float scaleMultiplier = 0.03f;
    public Player player;

	void Start () {
		player.playerHealth.playerTakingDamage.AddListener(OnTakingDamage);
	}
	
	void Update () {
		player.playerId.panelHealthBar.transform.position = Camera.main.WorldToScreenPoint(player.transform.position) + offset;
        player.playerId.panelHealthBar.GetComponent<RectTransform>().localScale = Vector3.one * scaleMultiplier;
    }

	public void OnTakingDamage(PlayerId playerId, float healthRatio) {
		if (playerId.greenHealthBar == null)
			return;
		
		playerId.greenHealthBar.fillAmount = Mathf.Clamp(healthRatio, 0.0f, 1.0f);
	}
}
