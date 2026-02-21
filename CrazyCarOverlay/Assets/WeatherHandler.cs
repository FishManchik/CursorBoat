using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WeatherHandler : MonoBehaviour
{
    public enum WeatherEnum
    {
        None,
        Snow,
        Leaves
    }

    [System.Serializable]
    public class WeatherState
    {
        public WeatherEnum weather;
        public ParticleSystem particle;
    }

    [Header("Dictionary для состояний погоды: ")]
    [Space(15f)]
    [SerializeField] private List<WeatherState> weatherStatesList;

    private Dictionary<WeatherEnum, ParticleSystem> weatherStates;
    private IInputHandler input;

    [Inject]
    public void Construct(IInputHandler input)
    {
        this.input = input;
    }

    private void Awake()
    {
        weatherStates = new Dictionary<WeatherEnum, ParticleSystem>();

        foreach (var state in weatherStatesList)
            weatherStates[state.weather] = state.particle;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
