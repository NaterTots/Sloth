using UnityEngine;
using System.Collections;

/// <summary>
/// For this game, the InputController will just be a wrapper of the Unity InputManager
/// </summary>
public class InputController : IController
{
    public enum Axis
    {
        Horizontal,
        Vertical
    }

    public float GetAxis(Axis a)
    {
        switch (a)
        { 
            case Axis.Horizontal:
                return Input.GetAxis("Horizontal");
            case Axis.Vertical:
                return Input.GetAxis("Vertical");
            default:
                return 0.0f;
        }
    }

    public enum ClickRegion
    {
        Left,
        Right,
        None
    };

    public ClickRegion GetRegionClicked()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
        if (Input.GetMouseButtonDown(0))
        {
            if (Input.mousePosition.x <= Screen.width / 2)
            {
                return ClickRegion.Left;
            }

            return ClickRegion.Right;
        }

        return ClickRegion.None;
#else
        if (Input.touchCount > 0)
        {
            Touch myTouch = Input.touches[0];
            
            if (myTouch.phase == TouchPhase.Began)
            {
                if (myTouch.position.x <= Screen.width / 2)
                {
                    return ClickRegion.Left;
                }
                return ClickRegion.Right;
            }
        }

        return ClickRegion.None;
#endif

    }

    public void Cleanup()
    {

    }
}
