using Assets.Scripts.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerMoveHandler, IPointerUpHandler
{
    [SerializeField] private float sensitivity = 1f;
    public float InputX { get; set; }
    public float InputY { get; set; }
    public bool IsTouching { get; set; }


    public bool IsSwipingVert()
    {
        if (IsTouching && Input.GetAxis("Mouse Y") != 0f)
            return true;

        return false;
    }

    public bool IsSwipingHor()
    {
        if (IsTouching && Input.GetAxis("Mouse X") != 0f)
            return true;

        return false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsTouching = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (IsTouching)
        {
            InputX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            InputY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsTouching = false;

        EventManager.Invoke("hunter-jump");
    }
}
