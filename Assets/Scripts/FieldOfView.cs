using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class FieldOfView : MonoBehaviour
{
    [Range(0, 360)] public float viewAngle = 90f;
    public float viewDistance = 15f;
    public float detectionDistance = 3f;
    public Transform eye;
    public Transform target;

    private NavMeshAgent agent;
    private float rotationSpeed;
    private Transform agentTransform;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        rotationSpeed = agent.angularSpeed;
        agentTransform = agent.transform;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(target.transform.position, agent.transform.position);
        if (distanceToPlayer <= detectionDistance || IsInView())
        {
            RotateToTarget();
            MoveToTarget();
        }
        DrawViewState();
    }

    private bool IsInView()
    {
        float realAngle = Vector3.Angle(eye.forward, target.position - eye.position);
        RaycastHit hit;

        if(Physics.Raycast(eye.transform.position, target.position - eye.transform.position, out hit, viewDistance))
        {
            if (realAngle < viewAngle / 2f && Vector3.Distance(eye.position, target.position) <=viewDistance && hit.transform == target.transform )
            {
                return true;
            }
        }

        return false;
    }

    private void RotateToTarget()
    {
        Vector3 lookVector = target.position - agentTransform.position;
        lookVector.y = 0;
        
        if (lookVector == Vector3.zero)
            return;

        agentTransform.rotation = Quaternion.RotateTowards
            (
                agentTransform.rotation,
                Quaternion.LookRotation(lookVector, Vector3.up),
                rotationSpeed * Time.deltaTime
            );
    }

    private void MoveToTarget()
    {
        agent.SetDestination(target.position);
    }

    private void DrawViewState()
    {
        Vector3 left = eye.position + Quaternion.Euler(new Vector3(0, viewAngle / 2f, 0)) * (eye.forward * viewDistance);
        Vector3 right = eye.position + Quaternion.Euler(-new Vector3(0, viewAngle / 2f, 0)) * (eye.forward * viewDistance);

        Debug.DrawLine(eye.position, left, Color.yellow);
        Debug.DrawLine(eye.position, right, Color.yellow);

    }
}
