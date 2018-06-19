namespace DraggablePanelMod
{
    using DraggablePanelMod.Core;

    using UnityEngine;

    /// <summary>
    /// The draggable panel.
    /// </summary>
    public class DraggablePanel : MonoBehaviour
    {
        public Vector2 Offset;

        // Use GetComponent<KScreen>() instead?
        public KScreen Screen;

        private bool _isDragging;

        // TODO: enable debug from config file
        private const bool Debugging = false;

        public static void Attach(KScreen screen)
        {
            DraggablePanel panel = screen.FindOrAddUnityComponent<DraggablePanel>();

            if (panel == null)
            {
                return;
            }

            panel.Screen = screen;

            if (Debugging)
            {
                Debug.Log("DraggablePanel: Attached to KScreen" + screen.displayName);
            }
        }

        // TODO: call when position is set by game
        public static void SetPositionFromFile(KScreen screen)
        {
            if (Debugging)
            {
                Debug.Log("DraggablePanel: SetPositionFromFile enter");
            }

            DraggablePanel panel = screen.FindOrAddUnityComponent<DraggablePanel>();

            if (panel != null)
            {
                if (panel.LoadPosition(out Vector2 newPosition))
                {
                    panel.SetPosition(newPosition);
                    Debug.Log("DraggablePanel: Loaded position: " + newPosition);
                }
            }
            else if (Debugging)
            {
                Debug.Log("DraggablePanel: Can't FindOrAddUnityComponent");
            }
        }

        // Todo: fix the mashup with injection, move to Harmony
        public void Update()
        {
            if (this.Screen == null)
            {
                return;
            }

            Vector2 mousePos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
            {
                if (this.Screen.GetMouseOver)
                {
                    // TODO: cache RectTransform component
                    this.Offset = mousePos - this.Screen.GetComponentInParent<RectTransform>().anchoredPosition;

                    this._isDragging = true;
                }
            }

            if (this._isDragging)
            {
                Vector2 newPosition = mousePos - this.Offset;

                if (Input.GetMouseButtonUp(0))
                {
                    this._isDragging = false;

                    this.SavePosition(newPosition);

                    if (Debugging)
                    {
                        Debug.Log("DraggablePanel: Saved new panel position: " + newPosition);
                    }
                }

                this.SetPosition(newPosition);
            }
        }

        private bool LoadPosition(out Vector2 position)
        {
            return DraggableUI.UIState.LoadWindowPosition(this.gameObject, out position);
        }

        // TODO: queue save to file
        private void SavePosition(Vector2 position)
        {
            DraggableUI.UIState.SaveWindowPosition(this.gameObject, position);
        }

        // use offset?
        private void SetPosition(Vector3 newPosition)
        {
            if (this.Screen == null)
            {
                return;
            }

            this.Screen.GetComponentInParent<RectTransform>().anchoredPosition = newPosition;
        }
    }
}