using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackStats", menuName = "Combat/AttackStats")]
public class AttackStats : ScriptableObject
{
    // stats
    public string attackName;
    public float damage;

    // feel effects
    public float rumbleAmnt;
    public float shakeAmnt;
    public float zoomAmnt;
    public float shakeDur;
    public float hitstopAmnt;
}
