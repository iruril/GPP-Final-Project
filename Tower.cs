using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float attackRange = 10.0f;
    private Transform target;

    public GameObject bullet = null;

    public float attackCoolDown = 1.0f; //attack 빈도 조정
    private WaitForSeconds attackCoolDownWaitForSeconds;
    private bool isAttackCoolDown = false;
    public Coroutine attackCoolCoroutine;

    public TowerState state = TowerState.NONE; // 현재 상태.
    public TowerState next_state = TowerState.NONE; // 다음 상태.
    public float state_timer = 0.0f; // 타이머

    public enum TowerState //스테이트
    {
        NONE,
        IDLE,
        SHOOT
    };

    private GameObject lookOn = null;
    // Start is called before the first frame update
    void Start()
    {
        attackCoolDownWaitForSeconds = new WaitForSeconds(attackCoolDown);
        this.state = TowerState.NONE;
        this.next_state = TowerState.IDLE;
        InvokeRepeating("FindNearestEnemy", 0f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.next_state == TowerState.NONE)
        { // 상태 변환 조건
            switch (this.state)
            {
                case TowerState.IDLE:
                    if (target != null)
                    {
                        this.next_state = TowerState.SHOOT;
                    }
                    break;
                case TowerState.SHOOT:
                    if (target == null)
                    {
                        this.next_state = TowerState.IDLE;
                    }
                    break;
            }
        }

        while (this.next_state != TowerState.NONE)
        { // 상태 변환 후 초기화.
            this.state = this.next_state;
            this.next_state = TowerState.NONE;
            switch (this.state)
            {
                case TowerState.IDLE:
                    break;
                case TowerState.SHOOT:
                    break;
            }
            this.state_timer = 0.0f;
        }
        // 각 상황에서 반복할 것.
        switch (this.state)
        {
            case TowerState.IDLE:
                break;
            case TowerState.SHOOT:
                if (!isAttackCoolDown)
                {
                    shootBullet();
                }
                break;
                this.state_timer += Time.deltaTime;
        }
    }

    public IEnumerator attackCalc()
    {
        isAttackCoolDown = true;
        yield return attackCoolDownWaitForSeconds;
        isAttackCoolDown = false;
    }

    private void shootBullet()
    {
        attackCoolCoroutine = StartCoroutine(attackCalc());
        bullet.GetComponent<Bullet>().setDirection(target);
        Instantiate(bullet, this.transform.position,
            Quaternion.LookRotation(target.position - this.transform.position));
    }

    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDist = 100.0f;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distToEnemy < shortestDist)
            {
                shortestDist = distToEnemy;
                nearestEnemy = enemy;
            }

        }
        if (nearestEnemy != null && shortestDist <= attackRange) 
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
}
