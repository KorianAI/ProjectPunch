using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class Cashmere : BossInfo
{
    [Header("Volley")]
    [SerializeField] GameObject[] scrapVolleyProjectiles;
    [SerializeField] Animator volleyAnimator;
    [SerializeField] Vector3[] originalVolleyPosition;

    [Header("Bomb")]
    [SerializeField] ScrapSpiritBomb bomb;
    [SerializeField] public int currentSpotlight;

    [Header("Movement")]
    [SerializeField] CashmereSpotlight[] spotlights;
    public GameObject cashmereObj;
    public CashmereSpotlight nextSpotlight;
    public Transform arenaCenter;
    public float moveDur;
    public float timeBetweenMove;
    public bool moving;
    Coroutine wait2move;

    PlayerStateManager player;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i  < scrapVolleyProjectiles.Length; i++)
        {
            originalVolleyPosition[i] = scrapVolleyProjectiles[i].transform.localPosition;
        }

        player = PlayerStateManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack1();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack3();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            MoveToSpotlight();
        }

        if (moving)
        {
            cashmereObj.transform.LookAt(new Vector3(player.transform.position.x, cashmereObj.transform.position.y, player.transform.position.z));
        }
    }

    #region Attacks

    public override void Attack1()
    {
        StartCoroutine(ScrapVolley());
    }

    IEnumerator ScrapVolley()
    {
           
        foreach (GameObject proj in scrapVolleyProjectiles)
        {
            proj.SetActive(true);
            yield return new WaitForSeconds(1f);
        }

        volleyAnimator.enabled = true;
        yield return new WaitForSeconds(.1f);
        volleyAnimator.Play("ScrapVolley");

        yield return new WaitForSeconds(2);

        volleyAnimator.Play("Idle");
        volleyAnimator.enabled = false;

        foreach (GameObject proj in scrapVolleyProjectiles)
        {
            ScrapVolleyProjectile vp = proj.GetComponent<ScrapVolleyProjectile>();  
            proj.transform.DOMove(PlayerStateManager.instance.gameObject.transform.position, .7f).OnComplete(() => vp.SpawnScrapPile());
            yield return new WaitForSeconds(.5f);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < scrapVolleyProjectiles.Length; i++)
        {
            scrapVolleyProjectiles[i].transform.localPosition =   originalVolleyPosition[i];
        }
    }

    public override void Attack2()
    {
        CancelMovement();
        transform.DOMove(arenaCenter.position, 2f).OnComplete(() => {
            bomb.gameObject.SetActive(true);
            bomb.SortJumpOrder(currentSpotlight);
            bomb.JumpToNextPoint(); ; });   
    }

    public override void Attack3()
    {
        CancelMovement();
        transform.DOMove(arenaCenter.position, 2f).OnComplete(() => {
            bomb.gameObject.SetActive(true);
            StartCoroutine(bomb.RepeatedSlam());
        });

    }

    #endregion

    #region Movement

    public void MoveToSpotlight()
    {
        // Remove the current target from the list of available points
        List<CashmereSpotlight> availablePoints = new List<CashmereSpotlight>(spotlights);
        if (nextSpotlight != null)
        {
            availablePoints.Remove(nextSpotlight);
        }

        // Select a random point from the available points
        int randomIndex = Random.Range(0, availablePoints.Count);
        CashmereSpotlight newTarget = availablePoints[randomIndex];
        moving = true;
        // Move the enemy to the selected point using DOTween
        transform.DOMove(newTarget.cmPoint.position, moveDur).SetEase(Ease.Linear).OnComplete(() =>
        {
            // After completing the movement, set the new target as the current target
            nextSpotlight = newTarget;
            // Move to the next random point
            moving = false;
            wait2move = StartCoroutine(WaitToMove());
        });
    }

    IEnumerator WaitToMove()
    {      
        yield return new WaitForSeconds(timeBetweenMove);
        MoveToSpotlight();       
    }

    public void CancelMovement()
    {
        transform.DOKill();
        if (wait2move != null) { StopCoroutine(wait2move); }
    }

    #endregion

}
