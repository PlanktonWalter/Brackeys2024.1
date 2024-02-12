using UnityEngine;
using DG.Tweening;

public sealed class LockButton : MonoBehaviour
{

    [SerializeField] private Transform _child;

    private Tween _currentTween;

    public void OnSelected()
    {
        //_child.localPosition = Vector3.forward * -0.4f;
        _currentTween?.Kill();
        _currentTween = _child.DOLocalMoveZ(-0.2f, 0.05f);
    }

    public void OnDeselected()
    {
        //_child.localPosition = Vector3.zero;
        _currentTween?.Kill();
        _currentTween = _child.DOLocalMoveZ(0f, 0.05f);
    }

    public void OnPressed()
    {
        _currentTween?.Kill();
        _currentTween = _child.DOLocalMoveZ(0.1f, 0.2f);
    }

}
