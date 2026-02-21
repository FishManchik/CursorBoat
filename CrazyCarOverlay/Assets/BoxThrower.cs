using DG.Tweening;
using UnityEngine;
using Zenject;

public class BoxThrower : MonoBehaviour
{
    [SerializeField] private GameObject gameObject; 

    private IInputHandler input;

    [Inject]
    public void Construct(IInputHandler input)
    {
        this.input = input;
    }

    void Update()
    {
        /*
        if (input.IsThrowPressed)
            InvokeBoxThrowing();*/
    }

    private void InvokeBoxThrowing()
    {
        gameObject.transform.DOJump(input.MouseWorldPosition, 6f, 1, 1f).SetEase(Ease.InBack)
            .OnComplete(() => gameObject.transform.SetParent(this.transform));         //Зачем мне оно вообще нужно???
    }

    /* Someone, fix that $hit pleeeese, it is really poorly written */ 
}