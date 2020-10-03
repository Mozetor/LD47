using Assets.WaveSpawner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Spawnable {
    // Start is called before the first frame update
    public void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        
    }

    public abstract void Attack();
}
