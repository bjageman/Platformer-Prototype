using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointContainer : MonoBehaviour
{
	[SerializeField] bool isLoop = false;

	public bool IsLoop{ get { return isLoop; }}

	private void OnDrawGizmos()
	{
		Vector3 firstPosition = transform.GetChild(0).position;
		Vector3 previousPosition = firstPosition;

			
		foreach (Transform waypoint in transform)
		{
			Gizmos.DrawSphere(waypoint.position, .2f);
			Gizmos.DrawLine(previousPosition, waypoint.position);
			previousPosition = waypoint.position;
		}
		if (isLoop){
			Gizmos.DrawLine(previousPosition, firstPosition);
		}
	}
}