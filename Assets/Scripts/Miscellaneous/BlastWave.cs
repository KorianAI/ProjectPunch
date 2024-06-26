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
    public LineRenderer lineRenderer;
    public int pointsCount;

    public GameObject hitBox;
    public ScrapShockwave shockwave;
    Vector3 hitboxOriginalScale;
    public float scaleWise;
    public float scaleDur;

    bool prefab;

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
        hitBox.gameObject.SetActive(true);
        float currentRadius = 0f;
        hitBox.transform.localScale = hitboxOriginalScale;
        shockwave.canDealDamage = true;

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
            float angle = i * angleBetweenPoints * Mathf.Deg2Rad;
            Vector3 direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f);
            Vector3 position = direction * currentRadius;
            lineRenderer.SetPosition(i, position);
        }

        lineRenderer.widthMultiplier = Mathf.Lerp(0f, startWidth, 1f - currentRadius/maxRadius);
    }

    void ScaleObject()
    {
        hitBox.gameObject.SetActive(true);
        hitBox.transform.DOScale(new Vector3(scaleWise, transform.localScale.y, scaleWise), scaleDur).OnComplete(() => { hitBox.gameObject.SetActive(false);  });
    }
}
