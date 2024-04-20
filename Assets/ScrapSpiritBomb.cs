using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrapSpiritBomb : MonoBehaviour
{
    public Transform impactPoint;
    public GameObject shockwaveVFX;
    public GameObject rumbleVFX;

    [Header("Jumping")]
    public float jumpDuration = 1f;
    public float jumpPower;
    private int currentIndex = 0;
    public CashmereSpotlight[] jumpPoints;
    public CashmereSpotlight[] sortedQueue;
    public GameObject bigScrapPile;
    Vector3 originalPos;

    [Header("Slamming")]
    public float punchPower;
    public float punchDuration;

    public Transform slamPosition;

    private void Start()
    {
        originalPos = transform.position;
        sortedQueue = new CashmereSpotlight[jumpPoints.Length];
    }

    public void Slam()
    {
        GameObject shockwaveEffect = Instantiate(shockwaveVFX, impactPoint.position, Quaternion.Euler(-90, 0, 0));
        GameObject rumbleEffect = Instantiate(rumbleVFX, impactPoint.position, Quaternion.identity);
    }

    public void JumpToNextPoint()
    {
        // If all points have been visited, stop jumping
        if (currentIndex >= sortedQueue.Length) { gameObject.SetActive(false); transform.position = originalPos; currentIndex = 0;  return; }
        
        // Get the next jump point
        Transform targetPoint = sortedQueue[currentIndex].bombPoint;
        transform.DOJump(targetPoint.position, jumpPower, 1, jumpDuration)
            .OnComplete(() =>
            {
                // Move to the next point
                sortedQueue[currentIndex].Electrocute();
                Slam();
                if (currentIndex == sortedQueue.Length - 1) { Instantiate(bigScrapPile, sortedQueue[currentIndex].bombPoint.position, Quaternion.identity); }
                currentIndex++;            
                // Recursively call the function to jump to the next point
                JumpToNextPoint();
            });
    }

    public void SortJumpOrder(float order)
    {
        if (order == 1)
        {
            sortedQueue[0] = jumpPoints[0];
            sortedQueue[1] = jumpPoints[1];
            sortedQueue[2] = jumpPoints[2];
            sortedQueue[3] = jumpPoints[3];
        }

        else if (order == 2)
        {
            sortedQueue[0] = jumpPoints[1];
            sortedQueue[1] = jumpPoints[2];
            sortedQueue[2] = jumpPoints[3];
            sortedQueue[3] = jumpPoints[0];
        }

        else if (order == 3)
        {
            sortedQueue[0] = jumpPoints[2];
            sortedQueue[1] = jumpPoints[3];
            sortedQueue[2] = jumpPoints[0];
            sortedQueue[3] = jumpPoints[1];
        }

        else if (order == 4)
        {
            sortedQueue[0] = jumpPoints[3];
            sortedQueue[1] = jumpPoints[0];
            sortedQueue[2] = jumpPoints[1];
            sortedQueue[3] = jumpPoints[2];
        }
    }

    public IEnumerator RepeatedSlam()
    {
        transform.DOMove(slamPosition.position, .5f).OnComplete(() => { Slam(); });
        yield return new WaitForSeconds(.5f);
        transform.DOMove(originalPos, 1f).OnComplete(() =>  { StartCoroutine(RepeatedSlam());  });

    }

}
