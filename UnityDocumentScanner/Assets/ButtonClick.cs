using HoloToolkit.Unity.InputModule;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour, IInputHandler, IInputClickHandler, IFocusable, ISourceStateHandler, IHoldHandler, IManipulationHandler, INavigationHandler, IPointerClickHandler
{
    public string ButtonName = "";

    public void OnFocusEnter()
    {
        
    }

    public void OnFocusExit()
    {
        
    }

    public void OnHoldCanceled(HoldEventData eventData)
    {
        
    }

    public void OnHoldCompleted(HoldEventData eventData)
    {
        
    }

    public void OnHoldStarted(HoldEventData eventData)
    {
        
    }

    public void OnInputClicked(InputClickedEventData eventData)
    {
        //if (ButtonName == "plus")
        //{
        //    if (viewBodyObject)
        //        viewBodyObject.viewDistance += 10.0f;

        //}
        //else if (ButtonName == "minus")
        //{
        //    if (viewBodyObject)
        //        viewBodyObject.viewDistance -= 10.0f;
        //}
    }

    public void OnInputDown(InputEventData eventData)
    {
        Debug.LogFormat("OnInputUp\r\nSource: {0}  SourceId: {1}  InteractionPressKind: {2}", eventData.InputSource, eventData.SourceId, eventData.PressType);
        eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.
    }

    public void OnInputUp(InputEventData eventData)
    {
        Debug.LogFormat("OnInputUp\r\nSource: {0}  SourceId: {1}  InteractionPressKind: {2}", eventData.InputSource, eventData.SourceId, eventData.PressType);
        eventData.Use(); // Mark the event as used, so it doesn't fall through to other handlers.
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        
    }

    public void OnNavigationCanceled(NavigationEventData eventData)
    {
        
    }

    public void OnNavigationCompleted(NavigationEventData eventData)
    {
        
    }

    public void OnNavigationStarted(NavigationEventData eventData)
    {
        
    }

    public void OnNavigationUpdated(NavigationEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnSourceDetected(SourceStateEventData eventData)
    {
        
    }

    public void OnSourceLost(SourceStateEventData eventData)
    {
        
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
