using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public static ShipShooting Instance;

    [Space(5)]
    [Header("Ammo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shotgunSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float loadingTime;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private int maxAmmo = 100;

    // 0 - bullet
    // 1 - shotgun
    [SerializeField] private int ammoType = 0;

    [Space(5)]
    [Header("Joystick")]
    [SerializeField] private Joystick joystick;

    [Space(5)]
    [Header("Extra")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Collider _shipCollider;

    public Action<int> OnAmmoCapacityChanged;


    private int _currentAmmo = 0;
    private bool _isFireable = true;
    private bool _gunIsLoading = true;

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

    private void Start()
    {
        // UI Setting
        _currentAmmo = maxAmmo;
        OnAmmoCapacityChanged?.Invoke(_currentAmmo);
    }


    void Update()
    {
        if (joystick.Direction.magnitude > 0.5f && joystick.Direction.y > 0 && _isFireable && !gameManager.GameOver) 
        {
            if (ammoType == 0 && _currentAmmo > 0)
            {
                StartCoroutine(ShootRegularBullet());
            }

            if (ammoType == 1 && _currentAmmo > 4)
            {
                StartCoroutine(ShootWaveBullet());
            }
        }

        //Loading Gun
        if (Input.touchCount == 0 && _currentAmmo < maxAmmo && !_gunIsLoading)
        {
            _gunIsLoading = true;
            StartCoroutine(StartLoadingGun());
        }
    }

    IEnumerator ShootRegularBullet()
    {
        //Start state of gun
        _gunIsLoading = false;
        _currentAmmo--;
        _isFireable = false;

        // Update UI
        OnAmmoCapacityChanged?.Invoke(_currentAmmo);

        //Create Bullet
        GameObject newBullet = PoolManager.Instance.GetPooledObject(0,barrelTransform.position);

        newBullet.GetComponent<TrailRenderer>().Clear();

        newBullet.GetComponent<Rigidbody>().
            AddForce(new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized
            * bulletSpeed, ForceMode.Impulse);

        StartCoroutine(FireRateOperation());

        //Sound
        audioManager.PlayShipShotClip(ammoType);

        yield return null;
    }

    IEnumerator ShootWaveBullet()
    {
        //Start state of gun
        _gunIsLoading = false;
        _currentAmmo -= 5;
        _isFireable = false;

        // Update UI
        OnAmmoCapacityChanged?.Invoke(_currentAmmo);

        //Create Bullet
        GameObject Bullet1 = PoolManager.Instance.GetPooledObject(0, barrelTransform.position);
        GameObject Bullet2 = PoolManager.Instance.GetPooledObject(0, barrelTransform.position);
        GameObject Bullet3 = PoolManager.Instance.GetPooledObject(0, barrelTransform.position);

        Bullet1.GetComponent<TrailRenderer>().Clear();
        Bullet2.GetComponent<TrailRenderer>().Clear();
        Bullet3.GetComponent<TrailRenderer>().Clear();

        Bullet1.GetComponent<Rigidbody>().
           AddForce(shotgunSpeed * new Vector3(joystick.Direction.x - .2f, 0, joystick.Direction.y).normalized, ForceMode.Impulse);

        Bullet2.GetComponent<Rigidbody>().
           AddForce(shotgunSpeed * new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized, ForceMode.Impulse);

        Bullet3.GetComponent<Rigidbody>().
           AddForce(shotgunSpeed * new Vector3(joystick.Direction.x + .2f, 0, joystick.Direction.y).normalized, ForceMode.Impulse);

        StartCoroutine(FireRateOperation());

        //Sound
        audioManager.PlayShipShotClip(ammoType);

        yield return null;
    }

    IEnumerator FireRateOperation()
    {
        yield return new WaitForSeconds(fireRate);
        _isFireable = true;
    }

    IEnumerator StartLoadingGun()
    {
        yield return new WaitForSeconds(loadingTime);
        _currentAmmo++;
        OnAmmoCapacityChanged?.Invoke(_currentAmmo);
        _gunIsLoading = false;
    }

   
    public void DisableShooting()
    {
        joystick.gameObject.SetActive(false);
        _shipCollider.enabled = false;
        audioManager.TurnOffEngine();
    }


    // EXTRA

    public void ChangeAmmoType(int ammoType)
    {
        this.ammoType = ammoType;
    } 

    public void StartFly(float startTimeFlight)
    {
        gameObject.transform.DOMoveZ(1.5f, startTimeFlight);
        audioManager.TurnOnEngine(startTimeFlight);
    }

    public Collider GetCollider()
    {
        return _shipCollider;
    }
}
