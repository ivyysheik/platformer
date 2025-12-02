using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    
    public class Button : MonoBehaviour
    {
        public GameObject platforms;
        public AudioSource buttonSound;
        public AudioSource metronome;
        
        public void OnTriggerEnter(Collider other)
        {
          StartCoroutine(PlatformsCycle());
            buttonSound.Play();
            metronome.Play();


        }
       

        IEnumerator PlatformsCycle()
        {
            platforms.SetActive(true);
            yield return new WaitForSeconds(15);
            platforms.SetActive(false);
            metronome.Stop();
            platforms.SetActive(false);
        }

    }
}
