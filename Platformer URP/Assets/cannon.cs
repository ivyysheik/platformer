using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class cannon : MonoBehaviour
    {

        public GameObject shot;
        public Transform cannonObj;
        public float force;
        // Start is called before the first frame update
        public void OnTriggerEnter(Collider other)
        {
            GameObject bullet = Instantiate(shot, cannonObj.position, cannonObj.rotation);
            shot.GetComponent<Rigidbody>().velocity = cannonObj.forward * force * Time.deltaTime;
        }
    }
}
