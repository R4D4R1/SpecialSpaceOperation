using System.Collections;
using UnityEngine;

public class SpawnerAsteroid : Spawner
{
    public static SpawnerAsteroid Instance;

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

    public void StartAsteroids()
    {
        StartCoroutine(LaunchObjectCourutine());
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
