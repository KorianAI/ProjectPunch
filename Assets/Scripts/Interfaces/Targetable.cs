using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    public Transform targetPoint;
    public bool environment;

    public bool pullMe;
    public bool pushMe;
    public Material mat;
    public Color test;

    public MeshRenderer mRenderer;
    public SkinnedMeshRenderer sRenderer;
    public float outlineThicknessMultiplier = 2;

    private void Start()
    {

    }

    public void SetColor()
    {
        if (mat == null) return;

        if (mRenderer != null || sRenderer != null)
        {
            Material newMat = Instantiate(mat);

            if (mRenderer != null)
            {
                mRenderer.material = newMat;
            }

            if (sRenderer != null)
            {
                Material[] materials = sRenderer.materials;  

                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = newMat;  
                }

                sRenderer.materials = materials;
            }

            newMat.SetColor("_OutlineColor", test);
            newMat.SetFloat("_OutlineWidth", mat.GetFloat("_OutlineWidth") * outlineThicknessMultiplier); 
        }
    }

    public void ResetColor()
    {
        if (mRenderer != null)
        {
            mRenderer.material = mat;
        }

        if (sRenderer != null)
        {
            Material[] materials = sRenderer.materials;

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = mat;
            }

            sRenderer.materials = materials;
        }
    }
}
