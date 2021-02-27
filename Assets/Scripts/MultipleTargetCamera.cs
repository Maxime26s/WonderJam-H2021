using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MultipleTargetCamera : MonoBehaviour
{
    public List<Transform> targets;

    private void LateUpdate()
    {
        Vector3 centerPoint = GetCenterPoint();

        transform.position = centerPoint;
    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }

        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        foreach(Transform target in targets)
        {
            bounds.Encapsulate(target.position);
        }
        return bounds.center;
    }

}
