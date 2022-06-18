using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace EnemyState
{
    public class EnemyControl : HPController
    {
        // Start is called before the first frame update
        protected NavMeshAgent navAgent;
        protected Renderer enemyRender;

        protected Color baseColor = Color.black;

        public EnemyState state = EnemyState.NONE; // ���� ����.
        public EnemyState next_state = EnemyState.NONE; // ���� ����.
        public float state_timer = 0.0f; // Ÿ�̸�
        public float enemySpeed = 5.0f;

        protected Transform castle = null;

        [SerializeField]
        private float jumpForce = 10.0f;

        private bool isFireworked = false;
        public GameObject firework = null;
        Castle castleObserver;
        Rigidbody rigid;

        public enum EnemyState //������Ʈ
        {
            NONE,
            MOVE,
            HITTED,
            ATTACK
        };

        protected override void Start()
        {
            base.Start();

            navAgent = GetComponent<NavMeshAgent>();
            navAgent.speed = enemySpeed;
            castle = GameObject.FindGameObjectWithTag("Castle").transform;
            this.gameObject.GetComponent<Renderer>().material.color = baseColor;
            enemyRender = gameObject.GetComponent<Renderer>();

            this.state = EnemyState.MOVE;
            this.next_state = EnemyState.NONE;

            castleObserver = GameObject.Find("Castle").GetComponent<Castle>();
            castleObserver.myEvent += doFireworks;
        }

        public void doFireworks()
        {
            navAgent.SetDestination(this.transform.position);
            if (!isFireworked)
            {
                Instantiate(firework, this.transform);
                isFireworked = true;
            }
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            while (this.next_state != EnemyState.NONE)
            { // ���� ��ȯ �� �ʱ�ȭ.
                this.state = this.next_state;
                this.next_state = EnemyState.NONE;
                switch (this.state)
                {
                    case EnemyState.MOVE:
                        navAgent.SetDestination(new Vector3(castle.position.x, castle.position.y, castle.position.z));
                        enemyRender.material.color = baseColor;
                        break;
                    case EnemyState.HITTED:
                        enemyRender.material.color = Color.red;
                        break;
                    case EnemyState.ATTACK:
                        navAgent.SetDestination(this.transform.position);
                        enemyRender.material.color = baseColor;
                        break;
                }
                this.state_timer = 0.0f;
            }
            // �� ��Ȳ���� �ݺ��� ��.
            switch (this.state)
            {
                case EnemyState.MOVE:
                    navAgent.SetDestination(new Vector3(castle.position.x, castle.position.y, castle.position.z));
                    break;
                case EnemyState.HITTED:
                    this.state_timer += Time.deltaTime;
                    break;
                case EnemyState.ATTACK:
                    break;
            }
        }
    }
}