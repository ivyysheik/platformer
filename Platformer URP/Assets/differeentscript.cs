using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class differeentscript : MonoBehaviour
    {
        public Renderer shadow;
        public float maxDistance;
        public RaycastHit hit;
        public float offset;

        private Vector3 _initialShadowScale;

        private void Awake()
        {
            _initialShadowScale = shadow.transform.localScale;
        }

        private void FixedUpdate()
        {
            Ray downRay = new Ray(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), -Vector3.up);
            bool rayHitSomething = Physics.Raycast(downRay, out hit, maxDistance);
            shadow.enabled = rayHitSomething;
            if (rayHitSomething)
            {
                float normalizedShadowDistance = 1 - Mathf.Clamp01(hit.distance / maxDistance);
                shadow.transform.position = hit.point;
                shadow.transform.localScale = _initialShadowScale * normalizedShadowDistance;
            }
        }
    }
}
