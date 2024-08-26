using DG.Tweening.Core.Easing;
using System.Collections;
using UnityEngine;

public class SpawnerHP : Spawner
{
    public static SpawnerHP Instance;

    [SerializeField] private Transform endTransform;

    private float _boxXEnd;
    private float _boxYEnd;
    private float _boxZEnd;
    private float _boxWidthEnd;
    private float _boxHeightEnd;
    private float _boxDepthEnd;

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

    public void StartSpecial()
    {
        _boxXEnd = endTransform.position.x;
        _boxYEnd = endTransform.position.y;
        _boxZEnd = endTransform.position.z;

        _boxWidthEnd = endTransform.localScale.x;
        _boxHeightEnd = endTransform.localScale.y;
        _boxDepthEnd = endTransform.localScale.z;

        StartCoroutine(LaunchObjectCourutine());
    }

    protected override void SpawnObject()
    {
        Vector3 startPoint = new Vector3(_boxX - _boxWidth / 2, _boxY - _boxHeight / 2, _boxZ - _boxDepth / 2);
        Vector3 randomStartSpawnTransform = new Vector3(startPoint.x + Random.Range(0, _boxWidth),
        startPoint.y + Random.Range(0, _boxHeight), startPoint.z + Random.Range(0, _boxDepth));

        Vector3 startPointEnd = new Vector3(_boxXEnd - _boxWidthEnd / 2, _boxYEnd - _boxHeightEnd / 2, _boxZEnd - _boxDepthEnd / 2);
        Vector3 randomEndSpawnTransform = new Vector3(startPointEnd.x + Random.Range(0, _boxWidthEnd),
        startPointEnd.y + Random.Range(0, _boxHeightEnd), startPointEnd.z + Random.Range(0, _boxDepthEnd));

        // Spawn HP
        GameObject special = PoolManager.Instance.GetPooledObject((PoolObjectType)4, randomStartSpawnTransform);

        special.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(1, 2), Random.Range(1, 2), Random.Range(1, 2));
        special.GetComponent<Rigidbody>().AddForce
        (Random.Range(startRandomSpeed, endRandomSpeed) * (randomEndSpawnTransform - randomStartSpawnTransform).normalized, ForceMode.Impulse);
    }

    protected override IEnumerator LaunchObjectCourutine()
    {
        while (gameManager.GameOver != true)
        {
            SpawnObject();
            yield return new WaitForSeconds(Random.Range(startRandomRateTime, endRandomRateTime));
        }
    }
}
