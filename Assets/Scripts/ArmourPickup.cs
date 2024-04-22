using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourPickup : MonoBehaviour, ICollectible
{
    public void Collect(PlayerResources player)
    {
        player.ReplenishArmour();
        Destroy(gameObject);
    }
}
