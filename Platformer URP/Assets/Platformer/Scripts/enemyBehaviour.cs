using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class enemyBehaviour : MonoBehaviour
    {


      
        public NavMeshAgent enemy;
        public Transform player;
        // Start is called before the first frame update
        void Start()
        {
          enemy.speed = 0f;
        }
        
         private void OnTriggerEnter(Collider other)
      {
        if (other.gameObject.CompareTag("Player"))
        {
            
          enemy.SetDestination(player.position);
          enemy.speed = 3.5f; 
        }
      }

       void OnTriggerExit(Collider other)
    {
      if (other.gameObject.CompareTag("Player"))
      {
        enemy.speed = 0f;
      }
       
    }

     

       
        // Update is called once per frame
        void Update()
        {
          enemy.SetDestination(player.position);
          Debug.Log("the enemy speed is" + enemy.speed);
        }
    }
}
