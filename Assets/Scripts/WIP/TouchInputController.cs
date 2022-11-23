using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class TouchInputController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rect;
    private Vector2 currentTouchInput;
    [SerializeField] private InputHandlerSO _inputHandler;


    private void Awake()
    {
        TryGetComponent(out rect);
    }

    public void Update()
    {
        //rect.anchoredPosition = Vector2.ClampMagnitude(_inputHandler.DirectionalInput, 1) * 10;
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentTouchInput += eventData.delta.normalized;
        rect.anchoredPosition = Vector2.ClampMagnitude(currentTouchInput, 10);
        _inputHandler.SetDirectionalInput(currentTouchInput);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        currentTouchInput = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        _inputHandler.SetDirectionalInput(currentTouchInput);
    }
}
