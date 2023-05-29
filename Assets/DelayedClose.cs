using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedClose : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Close", 3f);
    }

    // Update is called once per frame
    void Close()
    {
        gameObject.SetActive(false);
    }
}
