using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossInfo : MonoBehaviour
{
    public BossHealth health;
    public EnemySO stats;
    

    #region Phase1
    public abstract void Attack1();
    public abstract void Attack2();
    public abstract void Attack3();
    #endregion

    public abstract void Stunned();
}
