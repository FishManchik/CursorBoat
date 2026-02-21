using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoBox : MonoBehaviour
{
    [Header("Wobbling Effect Settings: ")]
    [SerializeField] private float amplitude = 9f;
    [SerializeField] private float frequency = 2f;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        StartFloatingEffect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartFloatingEffect()
    {
        DOTween.To(() => time, x => time = x, Mathf.PI * 2f, 1f / frequency).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart)
        .OnUpdate(() =>
        {
            float angleX = Mathf.Sin(time) * amplitude;
            float angleZ = Mathf.Sin(time) * angleX;

            Vector3 rot = transform.localEulerAngles;
            rot.x = angleX;
            rot.z = angleZ;
            transform.localEulerAngles = rot;
        });
    }
}
