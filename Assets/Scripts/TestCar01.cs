using UnityEngine;
using System.Collections;

public class TestCar01 : MonoBehaviour
{
	float hoverHeight = 10;
	void Start () 
	{
		
	}

	void FixedUpdate () 
	{
		RaycastHit hit = new RaycastHit();
		Vector3 idealPosition = new Vector3();
    	if (Physics.Raycast (transform.position, -transform.up, out hit, 100.0f))
		{
    		idealPosition = transform.position + ((hoverHeight - hit.distance) * transform.up);
		} 
		
    	Vector3 pull = (idealPosition - transform.position);
    	rigidbody.AddForce( pull );
	}
}
