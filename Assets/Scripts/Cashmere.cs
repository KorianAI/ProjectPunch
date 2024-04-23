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
    public Animator anim;

    [Header("Stunned")]
    public Transform stunnedPos;
    public float stunLength;
    public GameObject disengageVFX;
    public Transform dsVFXPoint;
    public LayerMask pLayer;
    public float kbForce;
    public float kbDur;
    public bool stunned;

    [Header("AttackManagement")]
    public float atkAmnt;
    public float prevAtk;
    public bool playerOnSpotlight;
    public float volleyCD;
    bool firstVolley;
    public bool needsToAtk2;
    public bool needsToAtk3;
    public float minDistance;

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
           StartFight();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack3();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            MoveToSpotlight();
        }

        if (!stunned)
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
            yield return new WaitForSeconds(.3f);
        }

        volleyAnimator.enabled = true;
        yield return new WaitForSeconds(.1f);
        volleyAnimator.Play("ScrapVolley");

        yield return new WaitForSeconds(1);

        volleyAnimator.Play("Idle");
        volleyAnimator.enabled = false;

        foreach (GameObject proj in scrapVolleyProjectiles)
        {
            cashmereObj.transform.LookAt(new Vector3(player.transform.position.x, cashmereObj.transform.position.y, player.transform.position.z));
            ScrapVolleyProjectile vp = proj.GetComponent<ScrapVolleyProjectile>();
            vp.cm = this;
            anim.SetTrigger("ScrapVolley");
            proj.transform.DOMove(PlayerStateManager.instance.gameObject.transform.position, 1f).OnComplete(() => vp.SpawnScrapPile());
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < scrapVolleyProjectiles.Length; i++)
        {
            scrapVolleyProjectiles[i].transform.localPosition =   originalVolleyPosition[i];
        }

        if (!firstVolley)
        {
            firstVolley = true;
           
        }

        SelectNextAttack();
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

    public override void Stunned()
    {
        // kill tweens
        CancelMovement();
        bomb.transform.DOKill();

        // stop coroutines
        StopAllCoroutines();
        bomb.StopAllCoroutines();

        ResetProjectiles();
        stunned= true;
        needsToAtk3 = true;
        transform.DOMoveY(stunnedPos.position.y, 1f).OnComplete(() => { StartCoroutine(StunLength()); });

    }

    IEnumerator StunLength()
    {
        yield return new WaitForSeconds(stunLength);
        Disengage();   
        transform.DOMove(new Vector3(arenaCenter.position.x, transform.position.y, arenaCenter.position.z), 1f).OnComplete(() =>
        { transform.DOMoveY(arenaCenter.position.y, 2f).OnComplete(() =>
        { health.RegainArmour(); stunned = false; SelectNextAttack(); });
        });
    }

    void ResetProjectiles()
    {
        for (int i = 0; i < scrapVolleyProjectiles.Length; i++)
        {
            scrapVolleyProjectiles[i].transform.localPosition = originalVolleyPosition[i];
            scrapVolleyProjectiles[i].gameObject.SetActive(false);
        }
        bomb.ResetBomb();
        volleyAnimator.Play("Idle");
        volleyAnimator.enabled = false;
    }

    void Disengage()
    {
        GameObject vfx = Instantiate(disengageVFX, dsVFXPoint.position, Quaternion.Euler(-90, 0, 0));
        Collider[] colliders = Physics.OverlapSphere(transform.position, 4f, pLayer);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<IKnockback>().Knockback(kbForce, transform, kbDur);
            }
        }       
    }
    #endregion

    #region EventManagement
    public override void StartFight()
    {
        // cinematic bs

        Attack3();
    }



    public void SelectNextAttack()
    {
        if (needsToAtk3)
        {
            Attack3();
            prevAtk = 3;
            needsToAtk3 = false;
            return;
        }

        if (needsToAtk2)
        {
            Attack2();
            prevAtk = 2;
            needsToAtk2 = false;
        }

        if (atkAmnt >= 3) { MoveToSpotlight(); atkAmnt = 0; Attack1(); prevAtk = 1;  } // checks how many atks its been since moved

        else
        {
            if (!firstVolley) { Attack1();  atkAmnt++;  return; }

            if (playerOnSpotlight)
            {
                if (prevAtk == 1 || prevAtk == 3)
                {
                    Attack2();
                    prevAtk = 2;
                }

                else if(prevAtk == 2)
                {
                    Attack1();
                    prevAtk = 1;
                }
                
            }

            else
            {
                if (prevAtk == 1 || prevAtk == 2)
                {
                    Attack3();
                    prevAtk = 3;
                }

                else if (prevAtk == 3)
                {
                    Attack1();
                    prevAtk = 1;
                }
            }

            atkAmnt++;
        }
    }

    public override void EndFight()
    {
        throw new System.NotImplementedException();
    }
    #endregion

}
