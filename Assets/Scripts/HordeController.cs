using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HordeController : MonoBehaviour
{
    private Vector3 mouseGroundPosition;
    private Vector3 initialMouseGroundPosition;

    [Header("Mouse Action Determination")]
    [SerializeField] private float maxClickTime;
    private float holdTimer;
    [SerializeField] private float minDragDistance;
    private bool dragging;

    [Header("Rat Flags")]
    [SerializeField] private float flagRadius;

    [Header("Rat Raking")]
    [SerializeField] private float rakeWidth;
    [SerializeField] private float rakeStartupTime;
    private Vector3 smoothDeltaMouseGroundPosition;
    [SerializeField] private int maxMouseSmoothSteps;
    private int mouseSmoothSteps;
    private EdgeCollider2D rakeCollider;

    private void Awake()
    {
        rakeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            OnClick();
        if (Input.GetMouseButton(0))
            OnHold();
        else if (Input.GetMouseButtonUp(0))
            OnRelease();
    }

    private void OnHold()
    {
        if (!Input.GetMouseButtonDown(0))
            CalculateMouseGroundPosition();

        // Calculate the smoothed position of the mouse by averaging over the last few frames.
        Vector3 deltaMouseGroundPosition = mouseGroundPosition - transform.position;
        if (mouseSmoothSteps >= maxMouseSmoothSteps) // Adjust the average by lerping it towards the new value.
            smoothDeltaMouseGroundPosition = (smoothDeltaMouseGroundPosition * (mouseSmoothSteps - 1) + deltaMouseGroundPosition) / mouseSmoothSteps;
        else // Add another frame into the average.
        {
            smoothDeltaMouseGroundPosition *= mouseSmoothSteps;

            smoothDeltaMouseGroundPosition += deltaMouseGroundPosition;
            mouseSmoothSteps += Mathf.Min(1, (int)(Time.deltaTime * Application.targetFrameRate));

            if (mouseSmoothSteps != 0)
                smoothDeltaMouseGroundPosition /= mouseSmoothSteps;
        }

        transform.SetPositionAndRotation(mouseGroundPosition, Quaternion.LookRotation(Vector3.back, smoothDeltaMouseGroundPosition));

        // Treat the mouse action as a drag if the mouse has been held long enough or it has strayed far enough from its initial position.
        holdTimer += Time.deltaTime;
        if (!dragging && (holdTimer > maxClickTime || Vector3.Distance(initialMouseGroundPosition, mouseGroundPosition) > minDragDistance))
        {
            dragging = true;
            holdTimer = maxClickTime;
        }
        if (dragging)
            OnDrag();
    }

    private void OnClick()
    {
        // Capture the initial ground position the mouse clicks.
        CalculateMouseGroundPosition();
        initialMouseGroundPosition = mouseGroundPosition;
    }

    private void OnClickConfirm()
    {
        // Set a flag at the clicked position.
        transform.position = mouseGroundPosition;
    }

    private void OnDrag()
    {
        SetRakeColliderWidth(holdTimer >= maxClickTime + rakeStartupTime ? rakeWidth : rakeWidth * (holdTimer - maxClickTime) / rakeStartupTime);
    }

    private void OnRelease()
    {
        // Treat the mouse action as a click if the mouse has been held short enough and has not strayed too far from its initial position.
        if (holdTimer < maxClickTime && Vector3.Distance(initialMouseGroundPosition, mouseGroundPosition) <= minDragDistance)
            OnClickConfirm();

        // Reset data about the mouse action.
        holdTimer = 0;
        dragging = false;

        mouseSmoothSteps = 0;
        SetRakeColliderWidth(0);

    }

    private void CalculateMouseGroundPosition()
    {
        // Get the ground position the mouse is over.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        mouseGroundPosition = mouseRay.origin - mouseRay.origin.z * mouseRay.direction / mouseRay.direction.z;
    }

    private void SetRakeColliderWidth(float width)
    {
        rakeCollider.SetPoints(new List<Vector2>()
        {
            width / 2 * Vector2.right,
            width / 2 * Vector2.left
        });

        // TODO: Remove - just here for debug info
        GetComponent<SpriteRenderer>().color = width == 0 ? Color.red : Color.green;
        GetComponent<LineRenderer>().SetPositions(new Vector3[2]
        {
            width / 2 * Vector2.right,
            width / 2 * Vector2.left
        });
    }
}
