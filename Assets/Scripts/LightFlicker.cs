using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;
    public float minIntensity = 0.3f;
    public float maxIntensity = 1.0f;
    public float flickerSpeed = 0.1f;
    public bool randomizeSpeed = true;

    private float nextFlickerTime;
    private float baseTime;

    void Awake()
    {
        if (flickerLight == null)
        {
            flickerLight = GetComponent<Light>();
        }

        baseTime = flickerSpeed;
        nextFlickerTime = Time.time + baseTime;
    }

    void Update()
    {
        if (Time.time >= nextFlickerTime)
        {
            // Use Perlin noise for smooth randomness (less jarring than Random.Range)
            float noise = Mathf.PerlinNoise(Time.time * 5f, 0f);
            flickerLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);

            // Schedule next flicker
            float interval = randomizeSpeed ? Random.Range(baseTime * 0.5f, baseTime * 1.5f) : baseTime;
            nextFlickerTime = Time.time + interval;
        }
    }
}
