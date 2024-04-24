using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmourPickup : MonoBehaviour, ICollectible
{
    public float rotationSpeed;

    public void Collect(PlayerResources player)
    {
        player.ReplenishArmour();
        Destroy(gameObject);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
}
