using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GameUI {
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectEnsureVisible : MonoBehaviour, IScrollHandler, IPointerEnterHandler {
        RectTransform scrollRectTransform;
        RectTransform contentPanel;
        RectTransform selectedRectTransform;
        GameObject lastSelected;
        public bool shutdownWithMouse = false;
        public bool runOnEnable = true;
        public bool isRunning = true;
        
        
        void OnEnable() {
            if (runOnEnable) {
                isRunning = true;
            }
        }
        public void OnScroll(PointerEventData data) {
            if (shutdownWithMouse && data.IsScrolling()) {
                isRunning = false;
            }
        }
        public void OnPointerEnter(PointerEventData eventData) {
            // if (eventData.pointerEnter) {}
            if (shutdownWithMouse) {
                isRunning = false;
            }
        }

        void Start() {
            scrollRectTransform = GetComponent<RectTransform>();
        }
        
        void Update() {
            if (!isRunning) {
                return;
            }
            //just incase content panel gets created in start.
            if (contentPanel == null) contentPanel = GetComponent<ScrollRect>().content;

            GameObject selected = EventSystem.current.currentSelectedGameObject;

            if (selected == null) {
                return;
            }
            if (selected.transform.parent != contentPanel.transform) {
                return;
            }
            if (selected == lastSelected) {
                return;
            }

            selectedRectTransform = selected.GetComponent<RectTransform>();
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, -(selectedRectTransform.localPosition.y) - (selectedRectTransform.rect.height / 2));

            lastSelected = selected;
        }
    }
}