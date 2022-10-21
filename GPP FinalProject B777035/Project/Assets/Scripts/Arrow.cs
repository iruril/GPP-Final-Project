using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float dealtDamage = 10.0f;
    private float speed = 20.0f;
    Transform castle = null;
    // Start is called before the first frame update
    void Start()
    {
        castle = GameObject.FindGameObjectWithTag("Castle").transform;
        this.transform.LookAt(castle, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Castle")
        {
            IDamgeable dealObject = other.gameObject.GetComponent<IDamgeable>();
            dealObject.TakeHit(dealtDamage);

            Destroy(this.gameObject);
        }
    }
}
