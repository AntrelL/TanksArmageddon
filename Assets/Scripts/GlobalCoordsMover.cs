using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalCoordsMover : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _offsetX = 0;
    [SerializeField] private float _offsetY = 0;

    private void Update()
    {
        if (_target != null)
        {
            Vector3 targetPosition = _target.position;
            transform.position = new Vector3(targetPosition.x + _offsetX, targetPosition.y + _offsetY, targetPosition.z);
        }
    }
}
