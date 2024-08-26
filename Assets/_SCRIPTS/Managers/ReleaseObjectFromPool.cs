using System.Collections;
using UnityEngine;

public class ReleaseObjectFromPool : MonoBehaviour
{
    [SerializeField] private PoolObjectType poolObjectType;
    [SerializeField] private float releaseTime;
    [SerializeField] private bool useRigidbody;

    private Rigidbody _rb;

    public enum PoolObjectType
    {
        Bullet,
        Asteroid,
        VFXExplosion,
        PSRockHit,
        HP,
        HPHits
    }
    private void Start()
    {
        if(useRigidbody)
            _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(ReleaseObject());
    }
    IEnumerator ReleaseObject()
    {
        yield return new WaitForSeconds(releaseTime);
        PoolManager.Instance.ReleaseObject(gameObject, (global::PoolObjectType)(PoolObjectType)(int)poolObjectType);

        if(useRigidbody)
        {
            _rb.angularVelocity = Vector3.zero;
            _rb.linearVelocity = Vector3.zero;
        }
    }
}
