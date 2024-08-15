using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public CrushCutscene cutscene;

    public void P1Ended()
    {
        cutscene.P1Ended();
    }

    public void EndScene()
    {
        cutscene.End();
    }
}
