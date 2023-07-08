using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HordeController : MonoBehaviour
{
    private static List<Rat> s_rats;
    public static List<Rat> Rats
    {
        get
        {
            if (s_rats == null)
                s_rats = new List<Rat>();
            return s_rats;
        }
        set => s_rats = value;
    }

    private static List<Vector3> s_flags;
    public static List<Vector3> Flags
    {
        get
        {
            if (s_flags == null)
                s_flags = new List<Vector3>();
            return s_flags;
        }
        set => s_flags = value;
    }

    private Vector3 m_mouseGroundPosition;
    private Vector3 m_initialMouseGroundPosition;

    [Header("Mouse Action Determination")]
    [SerializeField] private float maxClickTime;
    private float m_holdTimer;
    [SerializeField] private float minDragDistance;
    private bool m_dragging;

    [Header("Click Flags")]
    [SerializeField] private float clickFlagRadius;

    [Header("Rat Raking")]
    [SerializeField] private float rakeWidth;
    [SerializeField] private float rakeStartupTime;
    private Vector3 m_smoothDeltaMouseGroundPosition;
    [SerializeField] private int maxMouseSmoothSteps;
    private int m_mouseSmoothSteps;
    private EdgeCollider2D m_rakeCollider;

    private void Awake()
    {
        Flags = null;

        // Cache components.
        m_rakeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        // Handle input events.
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
        Vector3 deltaMouseGroundPosition = m_mouseGroundPosition - transform.position;
        if (m_mouseSmoothSteps >= maxMouseSmoothSteps) // Adjust the average by lerping it towards the new value.
            m_smoothDeltaMouseGroundPosition = (m_smoothDeltaMouseGroundPosition * (m_mouseSmoothSteps - 1) + deltaMouseGroundPosition) / m_mouseSmoothSteps;
        else // Add another frame into the average.
        {
            m_smoothDeltaMouseGroundPosition *= m_mouseSmoothSteps;

            m_smoothDeltaMouseGroundPosition += deltaMouseGroundPosition;
            m_mouseSmoothSteps += Mathf.Min(1, (int)(Time.deltaTime * Application.targetFrameRate));

            if (m_mouseSmoothSteps != 0)
                m_smoothDeltaMouseGroundPosition /= m_mouseSmoothSteps;
        }

        transform.SetPositionAndRotation(m_mouseGroundPosition, Quaternion.LookRotation(Vector3.back, m_smoothDeltaMouseGroundPosition));

        // Treat the mouse action as a drag if the mouse has been held long enough or it has strayed far enough from its initial position.
        m_holdTimer += Time.deltaTime;
        if (!m_dragging && (m_holdTimer > maxClickTime || Vector3.Distance(m_initialMouseGroundPosition, m_mouseGroundPosition) > minDragDistance))
        {
            m_dragging = true;
            m_holdTimer = maxClickTime;
        }
        if (m_dragging)
            OnDrag();
    }

    private void OnClick()
    {
        // Capture the initial ground position the mouse clicks.
        CalculateMouseGroundPosition();
        m_initialMouseGroundPosition = m_mouseGroundPosition;
    }

    private void OnClickConfirm()
    {
        transform.position = m_mouseGroundPosition;

        // Set a flag at the clicked position and focus every rat within the click flag radius on it.
        Flags.Add(m_mouseGroundPosition);
        foreach (Rat rat in Rats)
            if ((rat.transform.position - m_mouseGroundPosition).sqrMagnitude <= clickFlagRadius * clickFlagRadius)
                rat.flagIndex = Flags.Count - 1;
    }

    private void OnDrag()
    {
        SetRakeColliderWidth(m_holdTimer >= maxClickTime + rakeStartupTime ? rakeWidth : rakeWidth * (m_holdTimer - maxClickTime) / rakeStartupTime);
    }

    private void OnRelease()
    {
        // Treat the mouse action as a click if the mouse has been held short enough and has not strayed too far from its initial position.
        if (m_holdTimer < maxClickTime && Vector3.Distance(m_initialMouseGroundPosition, m_mouseGroundPosition) <= minDragDistance)
            OnClickConfirm();

        // Reset data about the mouse action.
        m_holdTimer = 0;
        m_dragging = false;

        m_mouseSmoothSteps = 0;
        SetRakeColliderWidth(0);

    }

    private void CalculateMouseGroundPosition()
    {
        // Get the ground position the mouse is over.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        m_mouseGroundPosition = mouseRay.origin - mouseRay.origin.z * mouseRay.direction / mouseRay.direction.z;
    }

    private void SetRakeColliderWidth(float width)
    {
        m_rakeCollider.SetPoints(new List<Vector2>()
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
