using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiffBehaviour : MonoBehaviour
{
    public float alpha;
    public float beta;
    public float gamma;
    public float delta;
    public float amplitudeScaler;
    public float hueShifter;
    public float hueOffset;
    private DiffSpawner spawner;
    private Vector3 currentSize;
    private float currentHue;
    private float[] dParams;
    private float[] phaseShift;
    public float varienceSpeed;
    private float fPrime;
    private float[] timers;
    private SpriteRenderer spriteRenderer;
    public void SetSpawner(DiffSpawner spawner)
    {
        this.spawner = spawner;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        dParams = new float[4];
        phaseShift = new float[4];
        timers = new float[4];
        for (int i = 0; i < 4; i++)
        {
            dParams[i] = 0;
            timers[i] = 0;
            phaseShift[i] = (float)i * 3f - 4f;
        }

        GetDerivative();
        UpdateSizeAndHue();
    }

    void Update()
    {
        for (int i = 0; i < 4; i++)
        {
            timers[i] += Time.fixedDeltaTime;
        }
        UpdateParameters();
        GetDerivative();
        UpdateSizeAndHue();
    }

    private void UpdateParameters()
    {
        for (int i = 0; i < 4; i++)
        {
            dParams[i] = alpha * Mathf.Sin(beta * (varienceSpeed * (timers[i] + phaseShift[i]))) + gamma * Mathf.Cos(delta * (varienceSpeed * (timers[i] + phaseShift[i])));
            if (dParams[i] == 0f)
            {
                timers[i] = 0f;
            }
        }
    }
    private void GetDerivative() // Function that ultimately determines the shape of the field
    {
        fPrime = dParams[0] * Mathf.Sin(dParams[1] * transform.position.x)
        + dParams[2] * Mathf.Cos(dParams[3] * transform.position.y);
    }
    private void UpdateSizeAndHue()
    {
        currentSize.x = amplitudeScaler * Mathf.Abs(fPrime);
        currentSize.y = currentSize.x;
        currentSize.z = currentSize.x;
        transform.localScale = currentSize;

        float hue = ((hueShifter) * fPrime + hueOffset) % 1f;
        Color newColor = Color.HSVToRGB(hue, 1, 1);
        spriteRenderer.color = newColor;
    }
}
