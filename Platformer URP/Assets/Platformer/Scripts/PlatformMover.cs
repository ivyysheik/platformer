using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


namespace Platformer
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] Vector3 moveTo = Vector3.zero;
        [SerializeField] float moveTime = 1f;
        [SerializeField] Ease ease = Ease.InOutQuad;

        Vector3 startPosition;


        void Start()
        {
            startPosition = transform.position;
            Move();
        }

        void Move()
        {
            transform.DOMove(moveTo + startPosition, moveTime).SetEase(ease).SetLoops(-1, LoopType.Yoyo);
        }


    }
}
