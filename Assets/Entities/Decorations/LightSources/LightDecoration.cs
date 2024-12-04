using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDecoration : Decoration
{

    [SerializeField] private Light2D _light;
    
    [SerializeField] private float _flickerIntensity = 1f;
    [SerializeField] private float _flickerFrequency = 1f;

    private void Update (){
        _light.intensity = Mathf.Max(Mathf.Abs(Mathf.Sin(_flickerFrequency * Time.time)) * _flickerIntensity, Random.Range(1f, 3f));

    }
}
