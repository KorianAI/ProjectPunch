using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMagnetisable
{
    void Pull(PlayerStateManager player);

    void Push(PlayerStateManager player);
}
