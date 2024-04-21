using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class BlastWave : MonoBehaviour
{
    public float maxRadius;
    public float speed;
    public float startWidth;
    private LineRenderer lineRenderer;
    public int pointsCount;

    public GameObject hitBox;
    Vector3 hitboxOriginalScale;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pointsCount + 1;
    }

    private void Start()
    {
        hitboxOriginalScale = hitBox.transform.localScale;
    }

    public IEnumerator Blast()
    {
        lineRenderer.enabled = true;
        hitBox.transform.DOKill();
        hitBox.gameObject.SetActive(false);
        float currentRadius = 0f;
        hitBox.transform.localScale = hitboxOriginalScale;

        ScaleObject();
        while (currentRadius < maxRadius)
        {
            currentRadius += Time.deltaTime * speed;
            Draw(currentRadius);
            yield return null;
        }
        lineRenderer.enabled = false;
    }

    private void Draw(float currentRadius)
    {
        float angleBetweenPoints = 360 / pointsCount;

        for (int i = 0; i <= pointsCount; i++)
        {
            float angle = i * angleBetweenPoints * Mathf.Rad2Deg;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;
            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius/maxRadius);
    }

    void ScaleObject()
    {
        hitBox.gameObject.SetActive(true);
        hitBox.transform.DOScale(new Vector3(17, transform.localScale.y, 17), 2.3f).OnComplete(() => { hitBox.gameObject.SetActive(false);  });
    }
}
