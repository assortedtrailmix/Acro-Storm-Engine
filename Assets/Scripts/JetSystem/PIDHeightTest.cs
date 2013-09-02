using System;
using UnityEngine;

public class PIDHeightTest : MonoBehaviour
{
    private Anchor _heightAnchor;
    public float Height_P, Height_I, Height_D;
    public float TargetHeight;
    public FloatLimit HeightForceLimit = new FloatLimit(-1000000,1000000);
	// Use this for initialization
	void Start () {
        _heightAnchor = new Anchor(transform, transform, new PID(Height_P, Height_I, Height_D, HeightForceLimit));
        Graph.YMax = 10f;
        Graph.YMin = 0f;
        Graph.channel[0].isActive = true;
        Graph.channel[1].isActive = true;
	}
	void FixedUpdate () {
	    RaycastHit _lastHit;
	    if (Physics.Raycast(transform.position, -transform.up, out _lastHit))
	    {
	        float pidForce = _heightAnchor.PidController.Update(_lastHit.distance, TargetHeight,Time.deltaTime * TimeSpan.TicksPerMillisecond);
            rigidbody.AddForce(transform.up * pidForce * Time.deltaTime);
	    }
        Graph.channel[0].Feed(TargetHeight);
        Graph.channel[1].Feed(_lastHit.distance);
    }
}
