using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class deadByKnives : MonoBehaviour
{
    [SerializeField]
    public UnityEvent onCollisionWith;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "projectile")
            onCollisionWith.Invoke();
    }
}
