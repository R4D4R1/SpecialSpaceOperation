using DG.Tweening;
using UnityEngine;

public class ShipPositionController : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private float offsetTime;


    private void Start()
    {
        transform.DOMoveY(transform.position.y + offset, offsetTime).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
    }
}
