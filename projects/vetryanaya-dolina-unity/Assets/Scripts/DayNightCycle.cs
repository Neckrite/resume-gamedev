using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    public float timeSpeed = 0.02f;
    [Range(0f, 1f)]
    public float currentTime = 0.4f; 

    [Header("Celestial References")]
    public Light sunLight;
    public Light moonLight;

    [Header("Sun Settings")]
    public Color dayLightColor = Color.white;
    public Color nightLightColor = new Color(0.05f, 0.05f, 0.1f);
    private float maxSunIntensity = 1f;

    [Header("Moon Settings")]
    public Color moonLightColor = new Color(0.4f, 0.5f, 0.7f);
    public float maxMoonIntensity = 0.3f;
    
    [Header("Night Sky Colors")]
    public Color nightSkyTint = new Color(0.02f, 0.02f, 0.05f); 
    public Color nightGroundColor = new Color(0.01f, 0.01f, 0.02f);
    
    private Color defaultSkyTint;
    private Color defaultGroundColor;
    private Material skyboxMaterial;

    private void Start()
    {
        if (sunLight != null) maxSunIntensity = sunLight.intensity;
        
        skyboxMaterial = RenderSettings.skybox;
        if (skyboxMaterial != null)
        {
            if (skyboxMaterial.HasProperty("_SkyTint")) defaultSkyTint = skyboxMaterial.GetColor("_SkyTint");
            if (skyboxMaterial.HasProperty("_GroundColor")) defaultGroundColor = skyboxMaterial.GetColor("_GroundColor");
        }
    }

    private void Update()
    {
        if (sunLight == null || moonLight == null) return;

        currentTime += timeSpeed * Time.deltaTime;
        if (currentTime >= 1f) currentTime = 0f;

        float sunAngle = currentTime * 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunAngle - 90f, 170f, 0f);
        moonLight.transform.rotation = Quaternion.Euler((sunAngle + 180f) - 90f, 170f, 0f);

        float sunDot = Vector3.Dot(sunLight.transform.forward, Vector3.down);
        float sunMask = Mathf.Clamp01(sunDot);
        
        float moonDot = Vector3.Dot(moonLight.transform.forward, Vector3.down);
        float moonMask = Mathf.Clamp01(moonDot);

        sunLight.intensity = sunMask * maxSunIntensity;
        sunLight.color = Color.Lerp(nightLightColor, dayLightColor, sunMask);

        moonLight.intensity = moonMask * maxMoonIntensity;
        moonLight.color = moonLightColor;

        if (skyboxMaterial != null)
        {
            float dayNightBlend = Mathf.Clamp01((sunDot + 0.1f) / 0.2f); 
            if (sunDot > -0.05f) RenderSettings.sun = sunLight;
            else RenderSettings.sun = moonLight;

            Color targetSkyTint = Color.Lerp(nightSkyTint, defaultSkyTint, dayNightBlend);
            Color targetGroundColor = Color.Lerp(nightGroundColor, defaultGroundColor, dayNightBlend);
            float targetThickness = Mathf.Lerp(0.1f, 1.0f, dayNightBlend);

            skyboxMaterial.SetColor("_SkyTint", targetSkyTint);
            skyboxMaterial.SetColor("_GroundColor", targetGroundColor);
            skyboxMaterial.SetFloat("_AtmosphereThickness", targetThickness);
        }
    }

    private void OnDisable()
    {
        if (skyboxMaterial != null)
        {
            if (skyboxMaterial.HasProperty("_SkyTint")) skyboxMaterial.SetColor("_SkyTint", defaultSkyTint);
            if (skyboxMaterial.HasProperty("_GroundColor")) skyboxMaterial.SetColor("_GroundColor", defaultGroundColor);
            if (skyboxMaterial.HasProperty("_AtmosphereThickness")) skyboxMaterial.SetFloat("_AtmosphereThickness", 1.0f);
        }
    }
}
