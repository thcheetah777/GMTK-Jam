using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HordeController : PausableMonoBehaviour
{
    private static HordeController s_instance;
    public static HordeController Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = FindObjectOfType<HordeController>();
            return s_instance;
        }
    }

    private static List<Rat> s_rats;
    public static List<Rat> Rats
    {
        get
        {
            if (s_rats == null)
                s_rats = new();
            return s_rats;
        }
        set => s_rats = value;
    }

    /// <summary>
    /// Adds a rat to the global horde.
    /// </summary>
    public static void AddRat(Rat rat) => Rats.Add(rat);

    /// <summary>
    /// Removes a rat from the global horde.
    /// </summary>
    public static void RemoveRat(Rat rat)
    {
        Rats.Remove(rat);
        CleanupUnusedFlags();
    }

    private static List<Vector3> s_flags;
    private static List<int> s_flagRefCounts;

    /// <summary>
    /// Creates a new flag at the given position.
    /// </summary>
    public static void CreateFlag(Vector3 flag)
    {
        if (s_flags == null)
        {
            s_flags = new();
            s_flagRefCounts = new();
        }
        s_flags.Add(flag);
        s_flagRefCounts.Add(0);
    }

    /// <summary>
    /// Changes the flag the rat at the given index in the horde is focusing.
    /// </summary>
    public static void FocusRat(int ratIndex, int flagIndex)
    {
        Rat rat = Rats[ratIndex];

        if (rat.flagIndex != -1)
            s_flagRefCounts[rat.flagIndex]--;
        Rats[ratIndex].flagIndex = flagIndex;
        if (flagIndex != -1)
            s_flagRefCounts[flagIndex]++;

        CleanupUnusedFlags();
    }
    /// <summary>
    /// Focuses the rat at the given index in the horde on the last flag in the list.
    /// </summary>
    public static void FocusRat(int ratIndex) => FocusRat(ratIndex, s_flags.Count - 1);

    /// <summary>
    /// Deletes all flags that are not focused by any rats.
    /// </summary>
    private static void CleanupUnusedFlags()
    {
        List<int> indecesToDelete = new();

        // Iterate through the list of flag reference counts and mark the ones equal to 0 for deletion.
        for (int i = 0; i < s_flagRefCounts.Count; i++)
            if (s_flagRefCounts[i] == 0)
                indecesToDelete.Add(i);

        // Return from the function early if every flag is focused by at least one rat.
        if (indecesToDelete.Count == 0)
            return;
        // Reset the list of flags and return from the function early if all of them are to be deleted.
        if (indecesToDelete.Count == s_flags.Count)
        {
            s_flags.Clear();
            s_flagRefCounts.Clear();

            foreach (Rat rat in Rats)
                if (rat.flagIndex != -1)
                    rat.flagIndex = 0;

            return;
        }

        // Iterate through the list of flag indeces marked for deletion to overwrite them.
        int indecesDeleted = 0;

        for (int i = 0; i + indecesDeleted < s_flags.Count; i++)
        {
            while (indecesToDelete[indecesDeleted] == i)
                i++;

            // Exit the loop early if all indeces marked for deletion have been overwritten.
            if (i + indecesDeleted >= s_flags.Count)
                break;

            if (indecesDeleted != 0)
            {
                // Overwrite flags earlier in the list with flags later in the list.
                s_flags[i] = s_flags[i + indecesDeleted];
                s_flagRefCounts[i] = s_flagRefCounts[i + indecesDeleted];

                // Adjust the rats' flag indeces as data is being moved up in the list.
                foreach (Rat rat in Rats)
                    if (rat.flagIndex == i + indecesDeleted)
                        rat.flagIndex = i;
            }
        }

        // Remove the indeces at the end of the list after all of their data has been either copied to earlier entries or removed.
        s_flags.RemoveRange(s_flags.Count - indecesDeleted, indecesDeleted);
        s_flagRefCounts.RemoveRange(s_flagRefCounts.Count - indecesDeleted, indecesDeleted);
    }

    /// <summary>
    /// Returns the closest rat position to the given point.
    /// </summary>
    public static Vector3 ClosestRatPosition(Vector3 point)
    {
        if (s_rats.Count == 0)
            return point;

        Vector3 _out = s_rats[0].transform.position;
        float sqrDist = (_out - point).sqrMagnitude;

        for (int i = 1; i < s_rats.Count; i++)
        {
            Vector3 i_ratPosition = s_rats[i].transform.position;
            float i_sqrDist = (i_ratPosition - point).sqrMagnitude;
            if (i_sqrDist < sqrDist)
            {
                _out = i_ratPosition;
                sqrDist = i_sqrDist;
            }
        }

        return _out;
    }

    public static Vector3 MeanRatPosition() => new Vector3(Rats.Sum(rat => rat.transform.position.x), Rats.Sum(rat => rat.transform.position.y)) / Rats.Count;

    private Vector3 m_mouseGroundPosition;
    private Vector3 m_initialMouseGroundPosition;

    [Header("Mouse Action Determination")]
    [SerializeField] private float maxClickTime;
    private float m_holdTimer;
    [SerializeField] private float minDragDistance;
    private bool m_dragging;

    [Header("Cheese Reference")]
    public Transform cheeseTransform;

    [Header("Rat Movement")]
    public float minRatMovementSpeed;
    public float maxRatMovementSpeed;
    public float ratMovementSpeedWaveTime;
    public float unfocusedSlowFactor;

    [Header("Rat Distancing")]
    [SerializeField] private float ratDistancingSpeed;
    [SerializeField] private AnimationCurve ratDistancingWeight;
    public float minRatMass;
    public float maxRatMass;
    public float ratMassWaveTime;

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
        if (s_instance == null)
            s_instance = this;
        else if (s_instance != this)
            Destroy(gameObject);

        // Cache components.
        m_rakeCollider = GetComponent<EdgeCollider2D>();

        // Initialize rats.
        Rats.ForEach(InitRat);
    }

    public override void PausableUpdate()
    {
        // Handle input events.
        if (Input.GetMouseButtonDown(0))
            OnPress();
        if (Input.GetMouseButton(0))
            OnHold();
        else if (Input.GetMouseButtonUp(0))
            OnRelease();

        // Handle rat movement.
        Rats.ForEach(MoveRat);
    }

    private void InitRat(Rat rat)
    {
        rat.movementSpeedTimeOffset = Random.Range(0, ratMovementSpeedWaveTime);
        rat.massTimeOffset = Random.Range(0, ratMassWaveTime);
    }

    private void MoveRat(Rat rat)
    {
        // Follow focused flag.
        bool focused = rat.flagIndex != -1;
        Vector3 flag = focused ? s_flags[rat.flagIndex] : cheeseTransform.position;
        float movementSpeed = Wave(minRatMovementSpeed, maxRatMovementSpeed, Time.realtimeSinceStartup + rat.movementSpeedTimeOffset, ratMovementSpeedWaveTime);
        if (!focused)
            movementSpeed *= unfocusedSlowFactor;
        rat.transform.Translate((flag - rat.transform.position).normalized * movementSpeed * Time.deltaTime);

        // Push away from other rats.
        float mass = Wave(minRatMass, maxRatMass, Time.realtimeSinceStartup + rat.massTimeOffset, ratMassWaveTime);
        foreach (Rat other in Rats)
            if (rat != other)
            {
                Vector2 distVector = rat.transform.position - other.transform.position;
                float dist = distVector.magnitude;
                float weightedDist = ratDistancingWeight.Evaluate(dist);
                if (weightedDist == 0)
                    continue;

                float otherMass = Wave(minRatMass, maxRatMass, Time.realtimeSinceStartup + other.massTimeOffset, ratMassWaveTime);
                Vector3 scaledDistVector = distVector * ratDistancingSpeed * weightedDist / (dist * (mass + otherMass)) * Time.deltaTime;
                rat.transform.position += scaledDistVector * otherMass;
                other.transform.position -= scaledDistVector * mass;
            }
    }

    private void OnDestroy()
    {
        s_rats = null;

        s_flags = null;
        s_flagRefCounts = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Rat rat;
        if (collision.TryGetComponent(out rat))
            FocusRat(Rats.IndexOf(rat));
    }

    private void OnHold()
    {
        if (!Input.GetMouseButtonDown(0))
            m_mouseGroundPosition = MouseGroundPosition();

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
            OnDragConfirm();
        }
        if (m_dragging)
            OnDrag();
    }

    private void OnPress()
    {
        // Capture the initial ground position the mouse clicks.
        m_mouseGroundPosition = MouseGroundPosition();
        m_initialMouseGroundPosition = m_mouseGroundPosition;
    }

    private void OnClick()
    {
        transform.position = m_mouseGroundPosition;

        // Set a flag at the clicked position and focus every rat within the click flag radius on it.
        CreateFlag(m_mouseGroundPosition);
        for (int i = 0; i < Rats.Count; i++)
            if ((Rats[i].transform.position - m_mouseGroundPosition).sqrMagnitude <= clickFlagRadius * clickFlagRadius)
                FocusRat(i);
    }

    private void OnDragConfirm()
    {
        m_holdTimer = maxClickTime;
        CreateFlag(m_mouseGroundPosition);
    }

    private void OnDrag()
    {
        SetRakeColliderWidth(m_holdTimer >= maxClickTime + rakeStartupTime ? rakeWidth : rakeWidth * (m_holdTimer - maxClickTime) / rakeStartupTime);
        s_flags[^1] = m_mouseGroundPosition;
    }

    private void OnRelease()
    {
        // Treat the mouse action as a click if the mouse has been held short enough and has not strayed too far from its initial position.
        if (m_holdTimer < maxClickTime && Vector3.Distance(m_initialMouseGroundPosition, m_mouseGroundPosition) <= minDragDistance)
            OnClick();

        // Reset data about the mouse action.
        m_holdTimer = 0;
        m_dragging = false;

        m_mouseSmoothSteps = 0;
        SetRakeColliderWidth(0);

    }

    private Vector3 MouseGroundPosition()
    {
        // Get the ground position the mouse is over.
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        return mouseRay.origin - mouseRay.origin.z * mouseRay.direction / mouseRay.direction.z;
    }

    private void SetRakeColliderWidth(float width)
    {
        m_rakeCollider.SetPoints(new()
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

    private float Wave(float min, float max, float time, float timeScale) => ((min + max) + (max - min) * Mathf.Cos(Mathf.PI * time / (2 * timeScale))) / 2;
}
