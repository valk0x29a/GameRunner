using UnityEngine;
using UnityEngine.InputSystem;

public class LookAtCursor : MonoBehaviour
{
    public GameObject cursorGameObject;

    public Camera playerCamera;

    //  [HideInInspector]
    //   public LayerMask raycastLayer;
    [DebugVariable]
    public LayerMask cursorRaycastLayers;
    public int raycastLenght;

    //   [HideInInspector]
    //  public GameObject cursorMarker;
    //  [HideInInspector]
    // public Vector3 cursorMarkerOffset;

    //    GameObject cursorMarkerGO;
    [DebugVariable("Cursor Position")]
    private Vector3 rayPosition;

    Vector2 cursorPosition;

    Vector2 lastCursorPos;

    // float recoilY;

    private void Update()
    {
        cursorPosition = InputSystem.actions.FindActionMap("Movement").FindAction("Mouse").ReadValue<Vector2>();
        if(cursorPosition == lastCursorPos) { return; }
        Ray ray = playerCamera.ScreenPointToRay(cursorPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastLenght, cursorRaycastLayers))
        {
            // Transform objectHit = hit.transform;
            rayPosition = hit.point;

            cursorGameObject.transform.position = rayPosition;

            float bX = gameObject.transform.localEulerAngles.x;
            float bZ = gameObject.transform.localEulerAngles.z;

            gameObject.transform.LookAt(cursorGameObject.transform);

            gameObject.transform.localEulerAngles = new Vector3(bX, gameObject.transform.localEulerAngles.y// + recoilY
                , bZ);

        }
        lastCursorPos = cursorPosition;
    }

    /*   public void SetYRecoil(float recoilY)
       {
           this.recoilY = recoilY;
       }
    */
    private void OnDrawGizmos()
    {
        //  Gizmos.color = Color.red;
        //     Gizmos.DrawCube(cursorGameObject.transform.position, new Vector3(0.5f, 0.5f, 0.5f));
        //     Gizmos.DrawLine(transform.position, cursorGameObject.transform.position);
        //    Gizmos.DrawLine(transform.position, transform.position+transform.forward);
    }
}
