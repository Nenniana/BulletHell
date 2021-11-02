using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllProjectiles : MonoBehaviour
{
    public event EventHandler OnDestoyAllProjectiles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            OnDestoyAllProjectiles?.Invoke(this, EventArgs.Empty);
    }
}
