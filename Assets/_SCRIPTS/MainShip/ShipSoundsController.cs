using UnityEngine;

public class ShipSoundsController : MonoBehaviour
{
    public static ShipSoundsController Instance;
    [SerializeField] private AudioManager audioManager;

    // 0 - Regular shot
    // 1 - Shotgun shot
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


  
}
