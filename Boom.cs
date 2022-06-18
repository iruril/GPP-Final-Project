using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public float dealtDamage = 15.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * 1.0f * Time.deltaTime);
        Destroy(this.gameObject, 0.2f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Castle")
        {
            IDamgeable dealObject = other.gameObject.GetComponent<IDamgeable>();
            dealObject.TakeHit(dealtDamage);
        }
    }
}