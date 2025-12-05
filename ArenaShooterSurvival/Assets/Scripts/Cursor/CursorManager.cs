using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // === CURSOR ===
    [SerializeField] private Texture2D cursorTexture;

    private Vector2 cursorHotspot;

    private void Start()
    {
        // Make the center of the cursor the new hotspot
        cursorHotspot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        // Set crossair cursor
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    // Set default cursor
    public void SetDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}