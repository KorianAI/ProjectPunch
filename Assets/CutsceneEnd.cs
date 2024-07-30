using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneEnd : MonoBehaviour
{
    public CrushCutscene cutscene;

    public void EndScene()
    {
        cutscene.End();
    }
}
