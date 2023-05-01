using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotBehaviour : MonoBehaviour
{

    public float rotationSpeedMax = 4.0f;
    public float currentRotationZ;
    public Vector2 minBounds;
    public Vector2 maxBounds;
    public float acVariance = 0.01f;
    public float acInterval = 15f;
    public float sizeAmplitude = 0.001f;
    public float maxSpeed;
    public float lenSinTime = 0.01f;
    public float minSpeed;
    public float moveSpeed;
    public float currentAcceleration;
    public float currentAlpha;
    public float currentRotationSpeed;

    private float a;
    private float phaseShift;

    private Rigidbody2D rb;
    private float timer;
    private float rTimer;

    public Vector3 dst;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dst = transform.localScale;
        SetRandomPosition();
        SetTheta();
        ChangeAcceleration();
    }

    bool flag = false;
    void Update()
    {
        currentRotationZ = transform.eulerAngles.z;
        timer += Time.fixedDeltaTime;
        rTimer += Time.fixedDeltaTime;
        if (timer >= acInterval)
        {
            ChangeAcceleration();
            timer = 0.0f;
        }

        ApplyAcceleration();

        a += Time.fixedDeltaTime;
        if(Mathf.Sin(a)==0.0f && flag)
        {
            a=0;
        }
        else if(Mathf.Sin(a)==0.0f){
            flag = true;
        }
        dst.x = sizeAmplitude * (float)Mathf.Sin(lenSinTime*a + phaseShift) + 0.002f;
        dst.y = dst.x;
        dst.z = dst.x;
        transform.localScale = dst;

        MoveInDirection();
        CheckBounds();
    }

    void SetRandomPosition()
    {
        Vector3 currentPosition = transform.position;
        currentPosition.x = Random.Range(minBounds.x, maxBounds.x);
        currentPosition.y = Random.Range(minBounds.y, maxBounds.y);
        transform.position = currentPosition;
    }

    void SetTheta()
    {
        transform.Rotate(0, 0, Random.Range(0, 360));
        phaseShift = Random.Range(-10f, 10f);
    }

    void MoveInDirection()
    {
        rb.velocity = transform.up * moveSpeed;
        transform.Rotate(0, 0, currentRotationSpeed * Time.fixedDeltaTime);
        
    }

    void ChangeAcceleration()
    {
        currentAcceleration = Random.Range(-acVariance, acVariance);
        currentAlpha = Random.Range(-acVariance, acVariance);
    }

    void ApplyAcceleration()
    {   

        currentRotationSpeed += currentAlpha * Time.fixedDeltaTime;
        currentRotationSpeed = Mathf.Min(rotationSpeedMax, currentRotationSpeed);
        currentRotationSpeed = Mathf.Max(-rotationSpeedMax, currentRotationSpeed);

        moveSpeed += currentAcceleration * Time.fixedDeltaTime;
        moveSpeed = Mathf.Max(0.0f, moveSpeed);
        moveSpeed = Mathf.Min(maxSpeed, moveSpeed);
        moveSpeed = Mathf.Max(minSpeed, moveSpeed);
    }

    void CheckBounds()
    {
        Vector3 currentPosition = transform.position;
        bool isOutOfBounds = false;

        if (currentPosition.x < minBounds.x)
        {
            currentPosition.x = maxBounds.x;
            isOutOfBounds = true;
        }
        else if (currentPosition.x > maxBounds.x)
        {
            currentPosition.x = minBounds.x;
            isOutOfBounds = true;
        }

        if (currentPosition.y < minBounds.y)
        {
            currentPosition.y = maxBounds.y;
            isOutOfBounds = true;
        }
        else if (currentPosition.y > maxBounds.y)
        {
            currentPosition.y = minBounds.y;
            isOutOfBounds = true;
        }

        if (isOutOfBounds)
        {
            transform.position = currentPosition;

        }

    }
}
