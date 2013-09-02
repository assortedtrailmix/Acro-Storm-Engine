#if UNITY_EDITOR || UNITY_DEBUG
#define DEBUG
#endif

using System;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

#region Usage
/*
 * Height P: -450000
 * Height I: -50
 * Height D: -2e+09
 * 
 * Height Force Limit Min: -90000
 * Height Force Limit Max: Infinity
 * 
 * Gravity Check Distance = 20
 * Rotation Check Distance = 20
 * 
 * Rotation Speed = 10
 * 
 * Fall Speed Multiplier = 1 - 10
 */
#endregion
[RequireComponent(typeof(Rigidbody))]
public class JetOrientator : MonoBehaviour
{
    private const float GravityAcceleration = 9.80665f;
    
    #region Public Parameters

    public string POIRootName = "POIs";
    public string ModelRootName = "Model";

    public float HoverHeight = 5f;
    public float HeightPIDMaxDistance = 10f;


    public float Height_P, Height_I, Height_D;

    public FloatLimit HeightForceLimit = new FloatLimit(-90000f,Mathf.Infinity);

    public float GravityCheckDistance = 20f;

    public float RotationCheckDistance = 20f;
    public float RotationSpeed;

    public float FallSpeedMultiplier;
    #endregion

    #region Private Vars
    private Transform _poiTransform;
    private Transform _modelTransform;

    private Transform _transform;
    private Rigidbody _rigidbody;

    private PID _pidController;
    private Transform _heightPIDTransform;
    private Transform _centerOfMassTransform;
    private RaycastHit _lastHit;

    private float _gravityAccel
    {
        get
        {

            return GravityAcceleration*Mathf.Clamp(FallSpeedMultiplier, 1, Mathf.Infinity);
        }
    }
    #endregion

    #region Initalization Code

    void Awake()
    {
        InitCachedVars();
        InitHeightPID();
        InitGraph();
    }

    private void InitCachedVars()
    {
        _transform = GetComponent<Transform>();

        _poiTransform = string.IsNullOrEmpty(POIRootName) ? _transform.FindChild(POIRootName) : _transform;
        _modelTransform = string.IsNullOrEmpty(ModelRootName) ? _transform.FindChild(ModelRootName) : _transform;

        _rigidbody = GetComponent<Rigidbody>();
        _centerOfMassTransform = POI.GetPoiByName(_poiTransform, "com");

        _rigidbody.centerOfMass = _transform.TransformPoint(_centerOfMassTransform.position);
        _rigidbody.useGravity = false;
    }
    private void InitHeightPID()
    {
        _heightPIDTransform = POI.GetPoiByName(_poiTransform, "height_anchor");

        _pidController = new PID(Height_P, Height_I, Height_D, HeightForceLimit);
        
    }
    #endregion

    #region FixedUpdate Code

    void FixedUpdate()
    {
        UpdateHeightPID();
        UpdateGravity();
        UpdateRotation();
    }

    void UpdateRotation(){
        Ray rotationRay = new Ray(_centerOfMassTransform.position, -_centerOfMassTransform.up);
        if (!Physics.Raycast(rotationRay, out _lastHit, RotationCheckDistance)) return;
        var targetLook = _transform.forward - _lastHit.normal*Vector3.Dot(_lastHit.normal, _transform.forward);
        var targetOrientation = Quaternion.LookRotation(targetLook, _lastHit.normal);

        _transform.rotation = Quaternion.Lerp(_transform.rotation, targetOrientation, Time.deltaTime * RotationSpeed);
    }

    void UpdateGravity()
    {
        Ray gravityRay = new Ray(_centerOfMassTransform.position, -_centerOfMassTransform.up);

        if (Physics.Raycast(gravityRay, out _lastHit, GravityCheckDistance))
        {
            Vector3 gravityForce = -_lastHit.normal * GravityAcceleration;
            _rigidbody.AddForce(gravityForce, ForceMode.Acceleration);
            Debug.Log(gravityForce);
            Debug.DrawLine(_lastHit.point, _lastHit.point + gravityForce);
        }
        else
        {
            Vector3 gravityForce = Vector3.down * _gravityAccel;
            _rigidbody.AddForce(gravityForce, ForceMode.Acceleration);
        }
       
    }

    void UpdateHeightPID()
    {
            if (Physics.Raycast(_heightPIDTransform.position, -_transform.up, out _lastHit))
            {
                float height = _lastHit.distance;

                if (height < HeightPIDMaxDistance)
                {
                    float pidForce = _pidController.Update(height, HoverHeight, Time.deltaTime);
                    Vector3 heightForce = _transform.up;
                    heightForce *= pidForce;
                    _rigidbody.AddForce(heightForce*Time.deltaTime);
                }
            }
        UpdateGraph();
    }

    #endregion

    #region Debug Calls

    [Conditional("DEBUG")]
    private void InitGraph()
    {
        Graph.YMax = 0f;
        Graph.YMin = 10f;
        Graph.channel[0].isActive = true;
        Graph.channel[1].isActive = true;
    }


    [Conditional("DEBUG")]
    void UpdateGraph()
    {
        Graph.channel[0].Feed(HoverHeight);
        Graph.channel[1].Feed(_lastHit.distance);
    }

    [Conditional("DEBUG")]
    void OnGUI()
    {
        GUILayout.Label(_pidController.LastValue.ToString());
        GUILayout.Label(_lastHit.distance.ToString());
        GUILayout.Label(HoverHeight.ToString());
    }
    #endregion
}


