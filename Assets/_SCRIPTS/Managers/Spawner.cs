using UnityEngine;
using System.Collections;
using DG.Tweening.Core.Easing;


public abstract class Spawner : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] protected float startRandomRateTime;
    [SerializeField] protected float endRandomRateTime;

    [Header("Speed")]
    [SerializeField] protected float startRandomSpeed;
    [SerializeField] protected float endRandomSpeed;

    [SerializeField] protected GameManager gameManager;
    [SerializeField] protected Transform startSpawnerTransform;


    protected float _boxX;
    protected float _boxY;
    protected float _boxZ;
    protected float _boxWidth;
    protected float _boxHeight;
    protected float _boxDepth;

    private void Start()
    {
        _boxX = startSpawnerTransform.position.x;
        _boxY = startSpawnerTransform.position.y;
        _boxZ = startSpawnerTransform.position.z;

        _boxWidth = startSpawnerTransform.localScale.x;
        _boxHeight = startSpawnerTransform.localScale.y;
        _boxDepth = startSpawnerTransform.localScale.z;
    }

    protected virtual void SpawnObject()
    {
        Vector3 startPoint = new Vector3(_boxX - _boxWidth / 2, _boxY - _boxHeight / 2, _boxZ - _boxDepth / 2);
        Vector3 randomSpawnTransform = new Vector3(startPoint.x + Random.Range(0, _boxWidth),
        startPoint.y + Random.Range(0, _boxHeight), startPoint.z + Random.Range(0, _boxDepth));

        // Spawn Asteroid
        GameObject obstacle = PoolManager.Instance.GetPooledObject((PoolObjectType)1, randomSpawnTransform);

        obstacle.GetComponent<Rigidbody>().AddForce
            (Random.Range(startRandomSpeed, endRandomSpeed) * (ShipShooting.Instance.transform.position - randomSpawnTransform).normalized, ForceMode.Impulse);

        obstacle.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(1, 2), Random.Range(1, 2), Random.Range(1, 2));

    }

    protected virtual IEnumerator LaunchObjectCourutine()
    {
        while (gameManager.GameOver != true)
        {
            SpawnObject();
            yield return new WaitForSeconds(Random.Range(startRandomRateTime, endRandomRateTime));
        }
    }
}
