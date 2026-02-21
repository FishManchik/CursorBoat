using DG.Tweening;
using System.Collections.Generic;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

public class WindowMinimizer : MonoBehaviour
{
    private IInputHandler input;
    private Dictionary<MeshRenderer, Vector3> objectsDic = new Dictionary<MeshRenderer, Vector3>(); //Mega softlock
    private bool IsFadeOutInProgress = false;

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(System.IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    const int SW_MINIMIZE = 6;

    [Inject]
    public void Construct(IInputHandler input)
    {
        this.input = input;
    }

    private void Update()
    {
        if (IsFadeOutInProgress)
        {
            return;
        }

        if (input.IsEscPressed)
        {
            InitializeMinimizer();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && IsFadeOutInProgress)
        {
            StartCoroutine(AnimateObjectsFadeIn());
        }
    }

    private void InitializeMinimizer()
    {
        var renderers = FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);

        foreach (var obj in renderers)
        {
            objectsDic[obj] = obj.transform.localScale;
        }

        AnimateObjectsFadeOut();
    }

    public void AnimateObjectsFadeOut()
    {
        Sequence sequence = DOTween.Sequence();
        IsFadeOutInProgress = true;

        foreach (var obj in objectsDic.Keys)
        {
            sequence.Join(obj.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutBack));
        }

        sequence.OnComplete(() => { Minimize(); 
            /*IsFadeOutInProgress = false; */});
    }

    public IEnumerator AnimateObjectsFadeIn()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (var pair in objectsDic)
        {
            pair.Key.transform.DOScale(pair.Value, 0.3f);
        }

        IsFadeOutInProgress = false;
        objectsDic.Clear();
    }

    public void Minimize()
    {
        ShowWindow(GetActiveWindow(), SW_MINIMIZE);
    }
}
