using TMPro;
using UnityEngine;

public class HUDManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI AmmoText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI HPText;

    [SerializeField] private ShipHealth shipHealth;
    [SerializeField] private ShipShooting shipShooting;
    [SerializeField] private GameManager gameManager;

    //public TextMeshProUGUI EnemyText;

    private void OnEnable()
    {
        shipHealth.OnHealthChanged += UpdateHealth;
        shipShooting.OnAmmoCapacityChanged += UpdateAmmo;
        gameManager.OnAsteroidDestroyedAmountChanged += UpdateScore;
    }
    private void OnDisable()
    {
        shipHealth.OnHealthChanged -= UpdateHealth;
        shipShooting.OnAmmoCapacityChanged -= UpdateAmmo;
        gameManager.OnAsteroidDestroyedAmountChanged -= UpdateScore;

    }


    public void UpdateScore(int score)
    {
        ScoreText.text = score.ToString();
    }

    public void UpdateAmmo(int ammo)
    {
        AmmoText.text = ammo.ToString();
    }

    public void UpdateHealth(int HP)
    {
        HPText.text = HP.ToString();
    }
}
