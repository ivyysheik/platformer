using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Platformer
{
    public class Rotator : MonoBehaviour
    {
       [SerializeField] private Vector3 _rotation;
       [SerializeField] private float speed;



        void Update()
        {
            transform.Rotate(_rotation * speed * Time.deltaTime);
         }
    }
}

