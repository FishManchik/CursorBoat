using System.Collections;
using UnityEngine;
using Zenject;
using System.Collections.Generic;
using DG.Tweening;

public class ConeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject conePrefab;
    [SerializeField] private float interval;

    private IInputHandler input;
    private bool IsCoroutineFinished = true;

    [SerializeField] private List<GameObject> cones = new List<GameObject>();

    [Inject]
    public void Construct(IInputHandler input)
    {
        this.input = input;
    }

    void Update()
    {
        if (input.IsClearSpacePressed)
        {
            ClearSpace();
        }

        if (!input.IsConeSpawnPressed) return;

        if (!IsCoroutineFinished) return;

        if (input.IsMovePressed)
        {
            StartCoroutine(SpawnCone(input.MouseWorldPosition));
        }
    }

    private IEnumerator SpawnCone(Vector3 coneSpawningPos)
    {
        IsCoroutineFinished = false;

        var clone = Instantiate(conePrefab, this.transform);
        clone.transform.position = coneSpawningPos;

        Vector3 origValue = clone.transform.localScale;
        clone.transform.localScale = Vector3.zero;
        clone.transform.DOScale(origValue, 0.2f).SetEase(Ease.InOutBack);

        cones.Add(clone);

        yield return new WaitForSeconds(interval);
        IsCoroutineFinished = true;
    }

    private void ClearSpace()
    {
        Sequence sequence = DOTween.Sequence();

        foreach (var cone in cones)
        {
            sequence.Join(cone.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutBack)
                    .OnComplete(() => Destroy(cone)));
        }

        sequence.OnComplete(() => cones.Clear());
    }
}
