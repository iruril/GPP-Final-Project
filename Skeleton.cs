using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyState
{
    public class Skeleton : EnemyControl
    {
        public float attackRange = 5.0f;
        public float attackCoolDown = 3.0f; //attack 빈도 조정
        private WaitForSeconds attackCoolDownWaitForSeconds;
        private bool isAttackCoolDown = false;
        public Coroutine attackCoolCoroutine;

        public GameObject arrow = null;

        // Start is called before the first frame update
        protected override void Start()
        {
            baseColor = Color.grey;
            attackCoolDownWaitForSeconds = new WaitForSeconds(attackCoolDown);
            base.Start();
        }

        // Update is called once per frame
        protected override void Update()
        {
            float distance = Vector3.Distance(this.transform.position, castle.position);

            if (this.next_state == EnemyState.NONE)
            { // 상태 변환 조건
                switch (this.state)
                {
                    case EnemyState.MOVE:
                        if (distance < attackRange)
                        {
                            this.next_state = EnemyState.ATTACK;
                        }
                        if (isHitted)
                        {
                            this.next_state = EnemyState.HITTED;
                        }
                        break;
                    case EnemyState.HITTED:
                        if (this.state_timer > 0.2f)
                        {
                            isHitted = false;
                            this.next_state = EnemyState.MOVE; 
                        }
                        break;
                    case EnemyState.ATTACK:
                        if (distance > attackRange)
                        {
                            this.next_state = EnemyState.MOVE;
                        }
                        if (isHitted)
                        {
                            this.next_state = EnemyState.HITTED;
                        }
                        break;
                }
            }

            switch (this.state)
            {
                case EnemyState.HITTED:
                    navAgent.SetDestination(this.transform.position);
                    break;
                case EnemyState.ATTACK:
                    if (!isAttackCoolDown)
                    {
                        enemyAttack();
                    }
                    break;
            }

            base.Update();
        }
        public IEnumerator attackCalc()
        {
            isAttackCoolDown = true;
            yield return attackCoolDownWaitForSeconds;
            isAttackCoolDown = false;
        }

        private void enemyAttack()
        {
            attackCoolCoroutine = StartCoroutine(attackCalc());
            Instantiate(arrow, this.transform);
        }

        protected override void Die()
        {
            base.Die();
            gameObject.SetActive(false);
            Spawner.instance.InsertEnemy(gameObject);
        }

        private void OnEnable()
        {
            this.Start();
        }
    }
}