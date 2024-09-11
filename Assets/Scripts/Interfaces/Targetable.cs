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

    private void Start()
    {
        //mRenderer = GetComponent<MeshRenderer>() ?? GetComponentInChildren<MeshRenderer>() ?? GetComponentInParent<MeshRenderer>();
        //if (mRenderer != null)
        //{
        //    Debug.Log("MeshRenderer found: " + mRenderer.gameObject.name);
        //}
        //else
        //{
        //    Debug.Log("MeshRenderer not found.");
        //}

        //if (mRenderer == null)
        //{
        //    sRenderer = GetComponent<SkinnedMeshRenderer>() ?? GetComponentInChildren<SkinnedMeshRenderer>() ?? GetComponentInParent<SkinnedMeshRenderer>();
        //    if (sRenderer != null)
        //    {
        //        Debug.Log("SkinnedMeshRenderer found: " + sRenderer.gameObject.name);
        //    }
        //    else
        //    {
        //        Debug.Log("SkinnedMeshRenderer not found.");
        //    }
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetColor();
        }
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
