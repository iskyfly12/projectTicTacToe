using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerBehaviour : MonoBehaviour
{
    public static bool enable = true;

    private Animation _animation;

    void Awake()
    {
        _animation = GetComponent<Animation>();
        transform.position = Vector3.zero;
    }
    
    public void ShowPointer(Transform pointerPosition)
    {
        if (!enable)
            return;

        if (!_animation.isPlaying)
            _animation.Play();

        transform.position = pointerPosition.position;
    }

    public void HidePointer()
    {
        if (!enable)
            return;

        _animation.Stop();
        transform.position = Vector3.zero;
    }
}
