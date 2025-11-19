using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class StarCollection : MonoBehaviour
    {
          public AudioSource audioSource;
        public AudioClip collected;
        public GameObject coinModel;
        
        
     
        // Start is called before the first frame update
      private void OnCollisionEnter(Collision collision)
      {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            audioSource.PlayOneShot(collected, 1.0F);

            
            Destroy(gameObject);
            
        }
      }
    }
}
    

