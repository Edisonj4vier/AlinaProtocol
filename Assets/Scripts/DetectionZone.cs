using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;
    public List<Collider2D> detectedCollider2Ds = new List<Collider2D>();

    private Collider2D _collider2D;

    public void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        detectedCollider2Ds.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        detectedCollider2Ds.Remove(collision);
        if (detectedCollider2Ds.Count <= 0)
        {
            noCollidersRemain.Invoke();
        }
    }
}
