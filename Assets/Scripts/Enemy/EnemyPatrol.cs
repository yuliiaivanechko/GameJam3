using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol: MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Movement parameters")]
    [SerializeField]
    private float speed;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    private bool movingLeft;

    private void Start()
    {
        movingLeft = false;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x >= leftEdge.position.x)
            {
                MoveInDirection(-1, Time.deltaTime);
            }
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (transform.position.x <= rightEdge.position.x)
            {
                MoveInDirection(1, Time.deltaTime);
            }
            else
            {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            Vector3 oldScale = transform.localScale;
            oldScale.x *= -1;
            transform.localScale = oldScale;
            idleTimer = 0;
        }
    }

    private void MoveInDirection(int direction, float deltaTime)
    {
        Vector3 oldPosition = transform.position;
        Vector3 directionVector = direction * (rightEdge.position - leftEdge.position).normalized;

        oldPosition += deltaTime * speed * directionVector;
        transform.position = oldPosition;
    }
}
