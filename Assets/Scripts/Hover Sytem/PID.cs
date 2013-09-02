
#define SimplePID
using System;
using UnityEngine;

#if AdvancedPID
public class PID
{
    #region Public PID Prams
    public float KP;
    public float KI;
    public float KD;
    public float KGain = 1;
    public float Input = 0f;
    public float Output = 0f;
    public float Setpoint = 0f;

    private bool _bInverseReaction = false;
    public bool InverseReaction
    {
        set
        {
            if (value == true)
            {
                KP = (0 - KP);
                KI = (0 - KI);
                KD = (0 - KD);
            }
            _bInverseReaction = value;
        }
        get
        {
            return _bInverseReaction;
        }
    }

    private bool _bIsActive = true;
    public bool IsActive
    {
        set
        {
            if ((value == false) && (_bIsActive == true))
            {
                Reinitialize();
            }
            _bIsActive = value;
        }
        get
        {
            return _bIsActive;
        }
    }
    #endregion

    #region Private Vars
    private float _sampleTime;
    private float _lastUpdateTime = 0f;
    private float _intergrationTerm;
    private float _lastInput;
    #endregion

    #region Public Accessors
    public void SetTunings(float Kp, float iFactor, float dFactor)
    {
        if (Kp < 0 || iFactor < 0 || dFactor < 0) return;
        this.KP = Kp;
        this.KI = iFactor * _sampleTime;
        this.KD = dFactor / _sampleTime;
    }

    public void SetSampleTime(float newSampleTime)
    {
        if (_sampleTime == 0f)
            _sampleTime = newSampleTime;
        if (newSampleTime > 0)
        {
            float ratio = newSampleTime / _sampleTime;
            KI *= ratio;
            KD /= ratio;
        }
        _sampleTime = newSampleTime;
    }

    float _outMin;
    float _outMax;
    public void SetOutputLimits(float min, float max)
    {
        if (min > max) return;
        _outMin = min;
        _outMax = max;

        if (Output > _outMax) Output = _outMax;
        else if (Output < _outMin) Output = _outMin;

        if (_intergrationTerm > _outMax) _intergrationTerm = _outMax;
        else if (_intergrationTerm < _outMin) _intergrationTerm = _outMin;
    }
    #endregion

    #region Public Usage Functions
    public void Update(float input, float setpoint)
    {
        this.Input = input;
        this.Setpoint = setpoint;
        Update();
    }
    public void Update()
    {

        if (!IsActive)
            return;
        float now = Time.time;
        float timeChange = now - _lastUpdateTime;
        if (timeChange >= _sampleTime)
        {
            /*Compute all the working error variables*/
            float error = Setpoint - Input;

            _intergrationTerm += KI * error;
            if (_intergrationTerm > _outMax) _intergrationTerm = _outMax;
            else if (_intergrationTerm < _outMin) _intergrationTerm = _outMin;
            float dInput = (error - _lastInput);

            /*Compute PID Output*/
            Output = KP * error + _intergrationTerm - KD * dInput;
            if (Output > _outMax) Output = _outMax;
            else if (Output < _outMin) Output = _outMin;

            /*Remember some variables for next time*/
            _lastInput = Input;
            _lastUpdateTime = now;
        }
    }
    #endregion

    #region Internal Functions
    private void Reinitialize()
    {
        _lastInput = Input;
        _intergrationTerm = Output;
        if (_intergrationTerm > _outMax)
        {
            _intergrationTerm = _outMax;
        }
        else
        {
            if (_intergrationTerm < _outMin)
            {
                _intergrationTerm = _outMin;
            }
        }
    }
    #endregion
}
#else
[System.Serializable]
public class PID
{
    #region Public Parameters

    public float Kp, Ki, Kd;
    public FloatLimit ForceLimit;

    #endregion

    #region Public Vars
    public float LastValue = 0f;
    #endregion

    #region Private Vars
    private float _integral;
	private float _lastError;
    #endregion

    public PID(float kp, float iFactor, float dFactor, FloatLimit  forceLimit)
	{
        if (forceLimit == null) throw new ArgumentNullException("forceLimit");
        this.Kp = kp;
		this.Ki = iFactor;
		this.Kd = dFactor;
	    this.ForceLimit = forceLimit;
	}

	public float Update(float setpoint, float actual, float timeFrame)
	{
	    timeFrame *= TimeSpan.TicksPerMillisecond;
		float present = setpoint - actual;
		_integral += present * timeFrame;
		float deriv = (present - _lastError) / timeFrame;
		_lastError = present;
	    float output = present*Kp + _integral*Ki + deriv*Kd;
	    output = ForceLimit.Clamp(output);
	    LastValue = output;
		return output;
	}
}
#endif
