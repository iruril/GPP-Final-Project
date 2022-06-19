using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : HPController
{
    public delegate void IsDestroyed();

    public event IsDestroyed myEvent; //¿ÉÀú¹ö

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        if (isDestroyed()) myEvent?.Invoke();
    }
    public bool isDestroyed()
    {
        return dead;
    }

    public void doReset()
    {
        this.Start();
    }
}
