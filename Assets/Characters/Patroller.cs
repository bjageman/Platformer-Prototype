using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour {

	[SerializeField] WaypointContainer patrolPath;
    [SerializeField] float waypointTolerance = 0.1f;
	[SerializeField] float waypointDwellTime = 1f;
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] int startingWaypointIndex = 0;

	int waypointDirection = 1;

	private void Start(){
		StartCoroutine(Patrol());
	}
	
	IEnumerator Patrol()
	{
		while (patrolPath != null && (startingWaypointIndex < patrolPath.transform.childCount))
		{
			Vector2 nextWaypointPos = patrolPath.transform.GetChild(startingWaypointIndex).position;
			float step = moveSpeed * Time.deltaTime;
        	transform.position = Vector2.MoveTowards(transform.position, nextWaypointPos, step);
			CycleWaypointWhenClose(nextWaypointPos);
			if (Vector2.Distance(transform.position, nextWaypointPos) <= waypointTolerance){
				yield return new WaitForSecondsRealtime(waypointDwellTime);
			}else{
				yield return new WaitForEndOfFrame();
			}
		}
	}

	private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
	{
		if (Vector2.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
		{
			if (patrolPath.IsLoop){
				startingWaypointIndex = (startingWaypointIndex + 1) % patrolPath.transform.childCount;
			}else{
				if (startingWaypointIndex == patrolPath.transform.childCount - 1){
					waypointDirection = -1;
				}else if (startingWaypointIndex == 0){
					waypointDirection = 1;
				}
				startingWaypointIndex = (startingWaypointIndex + waypointDirection);
			}
		}
	}
}
