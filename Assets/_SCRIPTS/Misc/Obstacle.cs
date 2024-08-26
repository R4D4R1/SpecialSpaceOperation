using System;
using System.Collections;
using UnityEngine;


public class Obstacle : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private ObjectType objectType;

    public Action OnAsteroidDestroyed;

    public enum ObjectType
    {
        Asteroid,
        HP
    }


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter(Collider collider)
    {
        if (objectType == ObjectType.Asteroid)
        {
            // Ship Attacked
            if (collider == ShipShooting.Instance.GetCollider())
            {
                ShipHealth.Instance.TakeDamage(15);

                PoolManager.Instance.GetPooledObject((PoolObjectType)2, transform.position);
                PoolManager.Instance.ReleaseObject(gameObject, (PoolObjectType)1);
            }

            if (collider.name.Contains("Bullet"))
            {
                OnAsteroidDestroyed.Invoke();

                PoolManager.Instance.GetPooledObject((PoolObjectType)3, transform.position);

                collider.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                collider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                PoolManager.Instance.ReleaseObject(gameObject, (PoolObjectType)1);
                PoolManager.Instance.ReleaseObject(collider.gameObject, 0);
            }

            GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        }

        if (objectType == ObjectType.HP)
        {
            if (collider.name.Contains("Bullet"))
            {
                ShipHealth.Instance.Heal(20);

                PoolManager.Instance.GetPooledObject((PoolObjectType)5, transform.position);;

                GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                collider.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                collider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                PoolManager.Instance.ReleaseObject(gameObject, (PoolObjectType)4);
                PoolManager.Instance.ReleaseObject(collider.gameObject, 0);
            }
        }
    }
}
