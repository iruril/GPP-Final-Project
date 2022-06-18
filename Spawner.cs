using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public GameObject skeleton;
    public GameObject creeper;

    public Queue<GameObject> q_enemy = new Queue<GameObject>(); // 오브젝트 풀링

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        for (int i = 0; i < 4; i++)
        {
            GameObject enemyTemp;
            if ((i % 2) == 0)
            {
                enemyTemp = Instantiate(skeleton, this.gameObject.transform);
            }
            else
            {
                enemyTemp = Instantiate(creeper, this.gameObject.transform);
            }
            q_enemy.Enqueue(enemyTemp);
            enemyTemp.SetActive(false);
        }
        StartCoroutine(EnemySpawn());
    }
    public void InsertEnemy(GameObject enemy)
    {
        q_enemy.Enqueue(enemy);
        enemy.SetActive(false);
    }

    public GameObject PopEnemy()
    {
        GameObject enemyTemp = q_enemy.Dequeue();
        enemyTemp.SetActive(true);

        return enemyTemp;
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {
            if (q_enemy.Count != 0)
            {
                GameObject enemyTemp = PopEnemy();
                enemyTemp.transform.position = gameObject.transform.position;
            }
            yield return new WaitForSeconds(2.0f);
        }
    }
}
