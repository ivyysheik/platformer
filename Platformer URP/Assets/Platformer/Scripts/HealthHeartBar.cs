using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class HealthHeartBar : MonoBehaviour
    {
       public GameObject heartPrefab;
       public PlayerController PlayerHealthNumber;

       List<HeartHealth> hearts = new List <HeartHealth>();

       public void ClearHearts()
       {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        hearts = new List<HeartHealth>();
       }

       public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);
        HeartHealth heartComponent = newHeart.GetComponent<HeartHealth>();
        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);

    }

    private void OnEnable()
    {
        PlayerController.OnPlayerDamaged += DrawHearts;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDamaged -= DrawHearts;
    }

    private void Start()
    {
        DrawHearts();
    }

    public void DrawHearts()
    {
        ClearHearts();
        float maxHealthRemainder = PlayerHealthNumber.maxHealth % 2;
        int heartsToMake = (int)((PlayerHealthNumber.maxHealth / 2) + maxHealthRemainder);
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }

         for (int i = 0; i < hearts.Count; i++)
        {
            int heartStatusRemainder =(int)Mathf.Clamp(PlayerHealthNumber.playerHealth - (i *2), 0, 2 );
            hearts[i].SetHeartImage((HeartStatus)heartStatusRemainder);
        }
    }

        



    }
}
