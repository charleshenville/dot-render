using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffBehaviour : MonoBehaviour
{
    public float simulationSpeed;
    public float alpha;
    public float beta;
    public float gamma;
    public float delta;
    public float epsilon;
    public float zeta;
    public float amplitudeScaler;
    public float velocityScaler;
    public float frequencyScaler;
    public float intricacy;
    public float intricacyShift;
    public float paramScaler;
    public float hueShifter;
    public float hueOffset;
    public float resetCutoff;
    public float averageSize;
    private DiffSpawner spawner;
    private Vector3 currentSize;
    private float currentHue;
    private float[] dParams;
    private float[] phaseShift;
    public float varienceSpeed;
    public float minSize;
    private float fPrime;
    private float[] timers;
    private float[] globalCounters;
    private Vector3 originalPosition;
    private SpriteRenderer spriteRenderer;
    private float[] dNaughts;
    public void SetSpawner(DiffSpawner spawner)
    {
        this.spawner = spawner;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dParams = new float[4];
        dNaughts = new float[4];
        phaseShift = new float[4];
        timers = new float[4];
        globalCounters = new float[4];
        for (int i = 0; i < 4; i++)
        {
            timers[i] = 0f;
            globalCounters[i] = 0f;
            phaseShift[i] = (float)i * 90f - 45f;
        }

        originalPosition = transform.position;
        UpdateParameters();
        for (int i = 0; i < 4; i++)
        {
            dNaughts[i] = dParams[i];
        }
        GetDerivative();
        UpdateSizeAndHue();
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            timers[i] += simulationSpeed*Time.fixedDeltaTime;
        }
        UpdateParameters();
        GetDerivative();
        UpdateSizeAndHue();
        MoveAccordingly();
    }

    private void UpdateParameters()
    {
        for (int i = 0; i < 4; i++)
        {
            dParams[i] = alpha * Mathf.Sin(beta * (varienceSpeed * (timers[i] + phaseShift[i])))
            + gamma * Mathf.Cos(delta * (varienceSpeed * (timers[i] + phaseShift[i])))
            + epsilon * Mathf.Cos(zeta * (varienceSpeed * (timers[i] + phaseShift[i])));
            dParams[i] *= paramScaler;
            if (Mathf.Abs(dParams[i] - dNaughts[i]) <= resetCutoff && globalCounters[i] > 100f)
            {
                timers[i] = 0f;
                globalCounters[i] = 0f;
            }
            else if (Mathf.Abs(dParams[i] - dNaughts[i]) <= resetCutoff)
            {
                globalCounters[i]++;
            }
        }
    }
    private void MoveAccordingly()
    {
        Vector3 newPosition;
        newPosition.x = originalPosition.x + velocityScaler * dParams[1] * Mathf.Cos(frequencyScaler * dParams[0] * transform.position.y);
        newPosition.y = originalPosition.y + velocityScaler * dParams[3] * Mathf.Sin(frequencyScaler * dParams[2] * transform.position.x);
        newPosition.z = originalPosition.z;

        transform.position = newPosition;
    }
    private void GetDerivative() // Function that ultimately determines the shape of the field
    {
        fPrime = dParams[0] * Mathf.Sin(intricacy * (dParams[1] + intricacyShift) * transform.position.x)
        + dParams[2] * Mathf.Cos(intricacy * (dParams[3] + intricacyShift) * transform.position.y);
    }

    private void UpdateSizeAndHue()
    {
        currentSize.x = amplitudeScaler * Mathf.Abs(fPrime) + averageSize;
        if (currentSize.x < minSize) { currentSize.x = 0; }
        currentSize.y = currentSize.x;
        currentSize.z = currentSize.x;
        transform.localScale = currentSize;

        float hue = ((hueShifter) * fPrime + hueOffset) % 1f;
        Color newColor = Color.HSVToRGB(hue, 0.8f, 1f);
        spriteRenderer.color = newColor;
    }
}
