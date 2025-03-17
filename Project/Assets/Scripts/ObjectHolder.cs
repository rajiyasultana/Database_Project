using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    public static ObjectHolder Instance;
    public Web Web;
    void Start()
    {
        Instance = this;
        Web = GetComponent<Web>();
    }

    
}
