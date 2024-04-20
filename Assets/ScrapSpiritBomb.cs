using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapSpiritBomb : MonoBehaviour
{
    public Transform impactPoint;

    public GameObject shockwaveVFX;
    public GameObject rumbleVFX;

    public void Slam()
    {
        GameObject shockwaveEffect = Instantiate(shockwaveVFX, impactPoint.position, Quaternion.Euler(-90, 0, 0));
        GameObject rumbleEffect = Instantiate(rumbleVFX, impactPoint.position, Quaternion.identity);
    }

}
