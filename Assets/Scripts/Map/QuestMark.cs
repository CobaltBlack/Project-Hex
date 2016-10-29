using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class QuestMark : MonoBehaviour
{
    SpriteRenderer sprite;
    Instance instance;
    bool isActive = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void ActivateQuestMark(Instance assignedInstance)
    {
        instance = assignedInstance;
        isActive = true;

        // make visible
        sprite.color = new Color(1f, 1f, 1f, 1f);
    }

    public bool IsActive()
    {
        return isActive;
    }

    void OnMouseEnter()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            StartPulsing();
        }
    }

    void OnMouseExit()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            StopPulsing();
        }
    }

    void OnMouseUp()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (isActive)
            {
                MapGameManager.instance.PlayInstance(instance);
                isActive = false;

                // make invisible
                sprite.color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    void StartPulsing()
    {
        StartCoroutine("Flicker", this.transform);
    }

    void StopPulsing()
    {
        _currentScale = InitScale;
        this.transform.localScale = Vector3.one * InitScale;
        StopCoroutine("Flicker");
    }

    // Flicker settings
    private float _currentScale = InitScale;
    private const float TargetScale = 1.2f; // must manually update according to gameobject size
    private const float InitScale = 1; // must manually update according to gameobject size
    private const int FramesCount = 30;
    private const float AnimationTimeSeconds = 0.25f;
    private float _deltaTime = AnimationTimeSeconds / FramesCount;
    private float _dx = (TargetScale - InitScale) / FramesCount;
    private bool _upScale = true;

    private IEnumerator Flicker(Transform target)
    {
        while (isActive)
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
