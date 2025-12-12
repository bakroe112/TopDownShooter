using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomMove : MonoBehaviour
{
    // Random movement settings
    public float maxIdleTime = 2f;
    public float randomMoveRadius = 4f;
    public float randomMoveSpeed = 5f;

    private Vector3 lastPosition;
    private float idleTimer = 0f;
    private Vector3 randomTargetPosition;
    private bool isMovingToRandomPos = false;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        CheckIdleMovement();
    }

    private void CheckIdleMovement()
    {
        float moveThreshold = 0.1f;
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved < moveThreshold)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= maxIdleTime && !isMovingToRandomPos)
            {
                GenerateRandomPosition();
                isMovingToRandomPos = true;
            }

            if (isMovingToRandomPos)
            {
                MoveToRandomPosition();
            }
        }
        else
        {
            idleTimer = 0f;
            isMovingToRandomPos = false;
        }

        lastPosition = transform.position;
    }

    private void GenerateRandomPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(0.5f, randomMoveRadius);
        randomTargetPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0) * randomDistance;
    }

    private void MoveToRandomPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, randomTargetPosition, randomMoveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, randomTargetPosition) < 0.1f)
        {
            isMovingToRandomPos = false;
            idleTimer = 0f;
        }
    }
}
