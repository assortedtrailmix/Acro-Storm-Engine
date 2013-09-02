#if OLD
using UnityEngine;
using System.Collections;

public class TestCar01 : MonoBehaviour
{

	
	public float Velocity_kP = 0.005f;
	public float Velocity_kI = 0.05f;
	public float Velocity_kD = 0.2f;
	
	public float Altitude_kP = 0.005f;
	public float Altitude_kI = 0.05f;
	public float Altitude_kD = 0.2f;
	
	PID frontHoverPid;
	PID backHoverPid;
	PID leftHoverPid;
	PID rightHoverPid;
	
	PID verticalVelocityPid;
	
	float hoverHeight = 5;
	float setVerticalVelocity = 0.001f;
	
	float deltaT = 1;
	void Start () 
	{
		frontHoverPid = new PID(Altitude_kP, Altitude_kI, Altitude_kD);
		backHoverPid = new PID(Altitude_kP, Altitude_kI, Altitude_kD);
		leftHoverPid = new PID(Altitude_kP, Altitude_kI, Altitude_kD);
		rightHoverPid = new PID(Altitude_kP, Altitude_kI, Altitude_kD);
		
		verticalVelocityPid = new PID(Velocity_kP, Velocity_kI, Velocity_kD);
	}

	void FixedUpdate () 
	{
		frontHoverPid.pFactor = Altitude_kP;
		frontHoverPid.iFactor = Altitude_kI;
		frontHoverPid.dFactor = Altitude_kD;

		backHoverPid.pFactor = Altitude_kP;
		backHoverPid.iFactor = Altitude_kI;
		backHoverPid.dFactor = Altitude_kD;

		leftHoverPid.pFactor = Altitude_kP;
		leftHoverPid.iFactor = Altitude_kI;
		leftHoverPid.dFactor = Altitude_kD;

		rightHoverPid.pFactor = Altitude_kP;
		rightHoverPid.iFactor = Altitude_kI;
		rightHoverPid.dFactor = Altitude_kD;
		
		verticalVelocityPid.pFactor = Velocity_kP;
		verticalVelocityPid.iFactor = Velocity_kI;
		verticalVelocityPid.dFactor = Velocity_kD;
		
		RaycastHit hit = new RaycastHit();
		
		Vector3 forcePointFront = transform.position + (0.5f * transform.forward);
		Vector3 forcePointBack = transform.position + (-0.5f * transform.forward);
		Vector3 forcePointLeft = transform.position + (-0.25f * transform.right);
		Vector3 forcePointRight = transform.position + (0.25f * transform.right);
		
		Vector3 forcePointCenter = transform.position;
		
		Vector3 forceStrengthFront = transform.up;
		Vector3 forceStrengthBack  = transform.up;
		Vector3 forceStrengthLeft  = transform.up;
		Vector3 forceStrengthRight = transform.up;
		
		Vector3 forceStrengthVelocity = transform.up;
				
		var roll = transform.rotation.z;
		var pitch = transform.rotation.x;
		var yaw = transform.rotation.y;
		
		if(Input.GetKeyDown(KeyCode.W))
		{
			rigidbody.AddForce(transform.forward * 15);
			//rigidbody.AddTorque(transform.right * 3);
			Debug.Log("Moving Forward");
		}
		
		if(Input.GetKeyDown(KeyCode.S))
		{
			rigidbody.AddForce(transform.forward * -15);
			Debug.Log("Moving Back");
		}
		
		//Vertical Velocity PID
		var pidForce = verticalVelocityPid.Update(setVerticalVelocity, rigidbody.velocity.y, deltaT);
		forceStrengthVelocity *= pidForce;
		rigidbody.AddForceAtPosition(forceStrengthVelocity, forcePointCenter);
		Debug.DrawLine(forcePointCenter, forcePointCenter + forceStrengthVelocity);
		
		//Front PID
    	if (Physics.Raycast (forcePointFront, transform.up * -1, out hit, 50.0f))
		{
			pidForce = frontHoverPid.Update(hoverHeight, hit.distance, deltaT);
			forceStrengthFront *= pidForce;
			rigidbody.AddForceAtPosition(forceStrengthFront, forcePointFront);	
			Debug.DrawLine(forcePointFront, forcePointFront + forceStrengthFront);
		}
		
		//Back PID
    	if (Physics.Raycast (forcePointBack, transform.up * -1, out hit, 50.0f))
		{
			pidForce = backHoverPid.Update(hoverHeight, hit.distance, deltaT);
			forceStrengthBack *= pidForce;
			rigidbody.AddForceAtPosition(forceStrengthBack, forcePointBack);
			Debug.DrawLine(forcePointBack, forcePointBack + forceStrengthBack);
		}
		
		//Left PID
    	if (Physics.Raycast (forcePointLeft, transform.up * -1, out hit, 50.0f))
		{
			pidForce = leftHoverPid.Update(hoverHeight, hit.distance, deltaT);
			forceStrengthLeft *= pidForce;
			rigidbody.AddForceAtPosition(forceStrengthLeft, forcePointLeft);
			Debug.DrawLine(forcePointLeft, forcePointLeft + forceStrengthLeft);
		}

		//Right PID
    	if (Physics.Raycast (forcePointLeft, transform.up * -1, out hit, 50.0f))
		{
			pidForce = rightHoverPid.Update(hoverHeight, hit.distance, deltaT);
			forceStrengthRight *= pidForce;
			rigidbody.AddForceAtPosition(forceStrengthRight, forcePointRight);
			Debug.DrawLine(forcePointRight, forcePointRight + forceStrengthRight);
		}		
	}
}
#endif