using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Player : MonoBehaviour
{
    public NavMeshAgent navAgent;

    public PlayerState state = PlayerState.NONE; // 현재 상태.
    public PlayerState next_state = PlayerState.NONE; // 다음 상태.

    float currentTime;
    public float speed = 5.0f;

    public GameObject tower;

    public bool isPPressed;

    Vector3 initPosition;
    private Vector3 pos;
    private Ray ray;
    private RaycastHit hitInfo;

    public Ray getPos()
    {
        return ray;
    }

    public float GetTime()
    {
        return currentTime;
    }

    public enum PlayerState
    {
        NONE,
        MOVE,
        PLACEMENT,
        REPLAY
    };

    [SerializeField]
    public static List<PlayerCommand> oldCommands;

    private PlayerCommand keyP, keyR, mouseRButton;

    int listIndex;
    public static bool isReplaying;

    void Start()
    {
        initPosition = this.transform.position;

        this.state = PlayerState.NONE; // 현 단계 상태를 초기화.
        this.next_state = PlayerState.MOVE; // 다음 단계 상태를 초기화.
        currentTime = 0;
        navAgent = GetComponent<NavMeshAgent>();
        this.navAgent.speed = this.speed;
        this.isPPressed = false;

        oldCommands = new List<PlayerCommand>();

        keyP = new PLACEMENT();
        keyR = new REPLAY();
        mouseRButton = new Move();
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        listIndex = 0;
        isReplaying = false;
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        pos = Input.mousePosition;
        ray = Camera.main.ScreenPointToRay(pos);

        if (!isReplaying)
        {
            if (this.next_state == PlayerState.NONE)
            {
                switch (this.state)
                {
                    case PlayerState.MOVE:
                        if (isPPressed) this.next_state = PlayerState.PLACEMENT;
                        break;
                    case PlayerState.PLACEMENT:
                        if (!isPPressed) this.next_state = PlayerState.MOVE;
                        break;
                    case PlayerState.REPLAY:
                        if (!isReplaying) this.next_state = PlayerState.MOVE;
                        break;
                }
            }

            while (this.next_state != PlayerState.NONE)
            {
                this.state = this.next_state;
                this.next_state = PlayerState.NONE;
                switch (this.state)
                {
                    case PlayerState.MOVE:
                        break;
                    case PlayerState.PLACEMENT:
                        break;
                    case PlayerState.REPLAY:
                        currentTime = 0;
                        keyR.Execute();
                        break;
                }
            }

            switch (this.state)
            {
                case PlayerState.MOVE:
                    if (Input.GetMouseButtonDown(1))
                        playerRPGMove();
                    break;
                case PlayerState.PLACEMENT:
                    placeTower();
                    break;
                case PlayerState.REPLAY:
                    break;
            }


            if (Input.GetKeyDown(KeyCode.P))
            {
                isPPressed = true;
            }
            else
            {
                isPPressed = false;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                currentTime = 0;
                keyR.Execute();
            }
        }
        else
        {
            Do_Replay();
        }
    }

    private void playerRPGMove()
    {
        mouseRButton = new Move();
        mouseRButton.Execute();
    }

    private void placeTower()
    {
        keyP = new PLACEMENT();
        keyP.Execute();
    }

    public void init_Replay()
    {
        isReplaying = true;
        currentTime = 0;
        listIndex = 0;
        this.transform.position = initPosition;

        GameObject[] towers;
        towers = GameObject.FindGameObjectsWithTag("Tower");
        for (int i = 0; i < towers.Length; i++)
        {
            Destroy(towers[i]);
        }
        GameObject[] effects;
        effects = GameObject.FindGameObjectsWithTag("Effect");
        for (int i = 0; i < effects.Length; i++)
        {
            Destroy(effects[i]);
        }
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyState.EnemyControl>().doReset();
        }
        GameObject castle = GameObject.FindGameObjectWithTag("Castle");
        castle.GetComponent<Castle>().doReset();
    }

    void Do_Replay()
    {
        if (currentTime >= oldCommands[listIndex].GetTimeTable())
        {
            oldCommands[listIndex].Do();

            listIndex++;
        }
        if (listIndex == oldCommands.Count) isReplaying = false;
    }
}
