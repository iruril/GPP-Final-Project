using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float dealtDamage = 0.0f;
    private float speed = 40.0f;
    Transform enemy = null;

    public void setDirection(Transform enemy)
    {
        this.enemy = enemy;
        this.transform.LookAt(enemy, Vector3.forward);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Destroy(this.gameObject, 2.0f);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            IDamgeable dealObject = other.gameObject.GetComponent<IDamgeable>();
            dealObject.TakeHit(dealtDamage);

            Destroy(this.gameObject);
        }
    }
}
