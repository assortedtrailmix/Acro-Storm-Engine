using System;
using UnityEngine;

public struct Anchor
{
    public Vector3 Position
    {
        get
        {
            return parentTransform.position + _positionOffset;
        }
    }

    public Transform parentTransform;

    public PID PidController;

    private Vector3 _positionOffset;

    public Anchor(Transform parent, Transform transform, PID pid)
    {
        this.parentTransform = parent;
        this._positionOffset = transform.position - parent.position;
        this.PidController = pid;
    }
}