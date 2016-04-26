using MoreFun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum AnchorEnum
{
    LeftBottom = 0, 
    RightBottom = 1,
    Center = 2,
}

public class VirtualJoyStick : MonoBehaviour ,IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Image bgImg;
    [SerializeField]
    private Image joystickImg;
    public AnchorEnum anchorType;
    private Vector2 inputVector;

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,
                                                                    eventData.position,
                                                                    eventData.pressEventCamera,
                                                                    out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            //MoreDebug.MoreLog(pos);

            // Make the center (0, 0) instead (-0.5 , 0.5)
            // Make the left bottom (-1, -1) instead (-1 , 0)
            switch (anchorType)
            {
                case AnchorEnum.LeftBottom:
                    inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
                    break;
                case AnchorEnum.RightBottom:
                    inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
                    break;
                case AnchorEnum.Center:
                    inputVector = new Vector3(pos.x * 2, 0, pos.y * 2);
                    break;
                default:
                    break;
            }
            //inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            //inputVector = new Vector2(pos.x * 2 - 1, pos.y * 2 - 1);
            // To normalized
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;

            //MoreDebug.MoreLog(inputVector);

            // Move Joystick Img
            joystickImg.rectTransform.anchoredPosition = new Vector2(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3),
                                                                     inputVector.y * (bgImg.rectTransform.sizeDelta.y / 3));

        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        // Reset pos
        inputVector = Vector2.zero;
        joystickImg.rectTransform.anchoredPosition = Vector2.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x;
        }
        else
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float Vertical()
    {
        if (inputVector.y != 0)
        {
            return inputVector.y;
        }
        else
        {
            return Input.GetAxis("Vertical");
        }
    }
}
