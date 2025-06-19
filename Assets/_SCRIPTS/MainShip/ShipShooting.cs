using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ShipShooting : MonoBehaviour
{
    public static ShipShooting Instance;

    public enum AmmoType
    {
        Bullet,
        Shotgun
    }

    [Header("Ammo")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shotgunSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private float loadingTime;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private int maxAmmo = 100;

    [Header("Joystick")]
    [SerializeField] private Joystick joystick;

    [Header("Extra")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Collider shipCollider;

    public Action<int> OnAmmoCapacityChanged;

    private int currentAmmo;
    private bool isFireable = true;
    private bool isShooting = false;
    private AmmoType currentAmmoType = AmmoType.Bullet;

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
        currentAmmo = maxAmmo;
        OnAmmoCapacityChanged?.Invoke(currentAmmo);
        StartAmmoReloadLoop().Forget();
    }

    private void Update()
    {
        if (joystick.Direction.magnitude > 0.5f && joystick.Direction.y > 0 && isFireable && !gameManager.GameOver)
        {
            switch (currentAmmoType)
            {
                case AmmoType.Bullet:
                    if (currentAmmo > 0)
                        ShootRegularBulletAsync().Forget();
                    break;

                case AmmoType.Shotgun:
                    if (currentAmmo >= 3)
                        ShootWaveBulletAsync().Forget();
                    break;
            }
        }
    }

    private async UniTask ShootRegularBulletAsync()
    {
        if (!isFireable || isShooting) return;

        isShooting = true;
        isFireable = false;

        currentAmmo--;
        OnAmmoCapacityChanged?.Invoke(currentAmmo);

        GameObject bullet = PoolManager.Instance.GetPooledObject(0, barrelTransform.position);
        bullet.GetComponent<TrailRenderer>().Clear();

        Vector3 direction = new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized;
        bullet.GetComponent<Rigidbody>().AddForce(direction * bulletSpeed, ForceMode.Impulse);

        audioManager.PlayShipShotClip((int)currentAmmoType);

        await UniTask.Delay(TimeSpan.FromSeconds(fireRate));

        isFireable = true;
        isShooting = false;
    }

    private async UniTask ShootWaveBulletAsync()
    {
        if (!isFireable || isShooting) return;

        isShooting = true;
        isFireable = false;

        currentAmmo -= 3;
        OnAmmoCapacityChanged?.Invoke(currentAmmo);

        Vector3 direction = new Vector3(joystick.Direction.x, 0, joystick.Direction.y).normalized;

        SpawnWaveBullet(direction + new Vector3(-0.2f, 0, 0));
        SpawnWaveBullet(direction);
        SpawnWaveBullet(direction + new Vector3(0.2f, 0, 0));

        audioManager.PlayShipShotClip((int)currentAmmoType);

        await UniTask.Delay(TimeSpan.FromSeconds(fireRate));

        isFireable = true;
        isShooting = false;
    }

    private void SpawnWaveBullet(Vector3 direction)
    {
        GameObject bullet = PoolManager.Instance.GetPooledObject(0, barrelTransform.position);
        bullet.GetComponent<TrailRenderer>().Clear();
        bullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shotgunSpeed, ForceMode.Impulse);
    }

    private async UniTaskVoid StartAmmoReloadLoop()
    {
        var token = this.GetCancellationTokenOnDestroy();

        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(loadingTime), cancellationToken: token);

            bool joystickIdle = joystick.Direction.magnitude < 0.5f || joystick.Direction.y <= 0.1f;

            if (joystickIdle && currentAmmo < maxAmmo)
            {
                currentAmmo++;
                OnAmmoCapacityChanged?.Invoke(currentAmmo);
            }
        }
    }

    public void DisableShooting()
    {
        joystick.gameObject.SetActive(false);
        shipCollider.enabled = false;
        audioManager.TurnOffEngine();
    }

    public void ChangeAmmoType(int ammoTypeIndex)
    {
        currentAmmoType = (AmmoType)ammoTypeIndex;
    }

    public void StartFly(float startTimeFlight)
    {
        transform.DOMoveZ(1.5f, startTimeFlight);
        audioManager.TurnOnEngine(startTimeFlight);
    }

    public Collider GetCollider()
    {
        return shipCollider;
    }
}
