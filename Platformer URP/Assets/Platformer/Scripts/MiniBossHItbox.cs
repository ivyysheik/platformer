using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MiniBossHItbox : MonoBehaviour
    {

      public float MiniBossHealth = 3f;
      public GameObject Star; 
      public GameObject miniBoss;
      public MeshRenderer renderer;
      Color originalColor;
      float flashDuration = 0.1f;
      public Material green;
      
      public GameObject Player;



      void Start()
      {
        renderer = GetComponentInParent<MeshRenderer>(); 
        originalColor = renderer.material.color;
        Material red = GetComponentInParent<Material>();


      }

      public void OnCollisionEnter(Collision collision)
      {
        if (collision.gameObject.CompareTag ("Player"))
        {
            
            MiniBossHealth -= 1f;
            StartCoroutine(FlashCoroutine());
           
            if (MiniBossHealth <= 0f)
            { 
                miniBoss.SetActive(false);
                GetComponent<Collider>().enabled = false;
                Destroy(miniBoss, 0.1f);
                Instantiate(Star, transform.position + new Vector3(0,6,0), Quaternion.Euler(0, 0, 90));
                Star.tag = "Star";
            }
        }
      }
      public void damageFlash()
      {
        
      }

        IEnumerator FlashCoroutine()
        {
           
            renderer.material.color = Color.black;
            yield return new WaitForSeconds(flashDuration);
            renderer.material.color = originalColor;
        }
    }
}
 