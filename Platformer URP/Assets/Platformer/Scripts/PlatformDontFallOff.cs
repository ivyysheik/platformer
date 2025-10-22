using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlatformDontFallOff : MonoBehaviour
    {
      [SerializeField] string playerTag = "Player";
      [SerializeField] Transform platform;

      private void OnTriggerEnter(Collider other)
      {
          if (other.CompareTag(playerTag))
          {
              // Make the player a child of the platform to prevent falling off
              other.transform.SetParent(platform);
          }
      }
      private void OnTriggerExit(Collider other)
      {
          if (other.CompareTag(playerTag))
          {
              // Detach the player from the platform when they exit the trigger
              other.transform.SetParent(null);
          }
      }
    }
}
