using UnityEngine;
using Zenject;
using DG.Tweening;

public class BoatAnimator : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private IInputHandler input;
    private bool IsInProgress = false; //хрень

    [Inject]
    public void Construct(IInputHandler input)
    {
        this.input = input;
    }

    void Update()
    {
        if (input.IsSpacePressed)
            PlaySqueezeEffect();
    }

    private void PlaySqueezeEffect()
    {
        if (IsInProgress)
            return;

        IsInProgress = true;
        float originalValue = player.transform.localScale.y;

        player.transform.DOScaleY(originalValue / 2, 0.2f).SetEase(Ease.InElastic).OnComplete(() =>
        {
            player.transform.DOScaleY(originalValue, 0.5f).SetEase(Ease.OutElastic).OnComplete(() =>
            {
                IsInProgress = false;
            });
        });
    }
}
