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

    [SerializeField] CashmereSpotlight[] spotlights;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i  < scrapVolleyProjectiles.Length; i++)
        {
            originalVolleyPosition[i] = scrapVolleyProjectiles[i].transform.localPosition;
        }
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
            Attack2();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Attack3();
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
        bomb.gameObject.SetActive(true);
        bomb.SortJumpOrder(currentSpotlight);
        bomb.JumpToNextPoint();
    }

    public override void Attack3()
    {
        bomb.gameObject.SetActive(true);
        StartCoroutine(bomb.RepeatedSlam());
    }

    #endregion

}
