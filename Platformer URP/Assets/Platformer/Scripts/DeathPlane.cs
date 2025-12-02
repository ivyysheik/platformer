using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    public class DeathPlane : MonoBehaviour
    {
      public void OnTriggerEnter (Collider other)
      {
        SceneManager.LoadScene("Area 2");
      }
    }
}
