using UnityEngine;
using UnityEngine.EventSystems;

namespace Ramesh.gyroscopeScripts_2
{
    public class TpuchPadController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public GyroscopeControl GyroscopeControl;

        public void OnDrag(PointerEventData eventData)
        {
            OnDragData(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            HandleTouchInput(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            GyroscopeControl.isTouching = false;

        }

        private void HandleTouchInput(PointerEventData eventData)
        {
            GyroscopeControl.isTouching = true;
            GyroscopeControl.lastTouchPosition = eventData.position;

        }

        private void OnDragData(PointerEventData eventData)
        {
            GyroscopeControl.MouseDelta = eventData.position - GyroscopeControl.lastTouchPosition;

            // Apply touch input for camera rotation
            float rotationX = GyroscopeControl.MouseDelta.x * GyroscopeControl.inputSensitivity;
            float rotationY = -GyroscopeControl.MouseDelta.y * GyroscopeControl.inputSensitivity;
            GyroscopeControl.inputRotation *= Quaternion.Euler(rotationY, rotationX, 0); // Update the input rotation quaternion

            GyroscopeControl.lastTouchPosition = eventData.position;
        }
    }
}
