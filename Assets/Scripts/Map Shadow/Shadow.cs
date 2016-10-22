using UnityEngine;
using System.Collections;

public class Shadow : MonoBehaviour
{
    // assign in inspector
    public GameObject shadow;
    public bool flicker = true;

    public void StartFlickerShadow()
    {
        StartCoroutine(Flicker(shadow.transform));
    }

    // Flicker settings
    private float _currentScale = InitScale;
    private const float TargetScale = 175; // must manually update according to gameobject size
    private const float InitScale = 150; // must manually update according to gameobject size
    private const int FramesCount = 100;
    private const float AnimationTimeSeconds = 3f;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx = (TargetScale - InitScale) / FramesCount;
    private bool _upScale = true;

    private IEnumerator Flicker(Transform target)
    {
        while (flicker)
        {
            while (_upScale)
            {
                _currentScale += _dx;
                if (_currentScale > TargetScale)
                {
                    _upScale = false;
                    _currentScale = TargetScale;
                }
                target.localScale = Vector3.one * _currentScale;
                yield return new WaitForSeconds(_deltaTime);
            }

            while (!_upScale)
            {
                _currentScale -= _dx;
                if (_currentScale < InitScale)
                {
                    _upScale = true;
                    _currentScale = InitScale;
                }
                target.localScale = Vector3.one * _currentScale;
                yield return new WaitForSeconds(_deltaTime);
            }
        }
    }
}
