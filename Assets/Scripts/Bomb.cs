using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    Cutter _cut;
    bool _dead;

    private void Start()
    {
        _cut = FindObjectOfType<Cutter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_dead) return;
        _cut.transform.position = transform.position;
        Invoke(nameof(DoCut), 0.001f);
        _dead = true;
    }

    void DoCut() {
        _cut.DoCut();
        //Destroy(gameObject);
    }

}
