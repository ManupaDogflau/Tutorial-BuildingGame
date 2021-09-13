using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    // Start is called before the first frame update
    private float orthographicSize;
    private float targetOrtographicSize;
    private bool edgeScrolling;
    public static CameraHandler Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        edgeScrolling = PlayerPrefs.GetInt("edgeScrolling", 1) == 1;
    }
    void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrtographicSize = orthographicSize;

    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (edgeScrolling)
        {
            if (Input.mousePosition.x > Screen.width - 30)
            {
                x = 1f;
            }
            if (Input.mousePosition.x < 30)
            {
                x = -1f;
            }
            if (Input.mousePosition.y > Screen.height - 30)
            {
                y = 1f;
            }
            if (Input.mousePosition.y < 30)
            {
                y = -1f;
            }
        }

        float moveSpeed = 30f;
        Vector3 moveDir = new Vector3(x, y).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomAmount = 2f;
        targetOrtographicSize -= Input.mouseScrollDelta.y * zoomAmount;

        float minOrthographicSize = 10f;
        float maxOrthographicSize = 30f;
        targetOrtographicSize = Mathf.Clamp(targetOrtographicSize, minOrthographicSize, maxOrthographicSize);

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrtographicSize, Time.deltaTime * zoomSpeed);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;
    }

    public void SetEdgeScrolling(bool edgeScrolling)
    {
        this.edgeScrolling = edgeScrolling;
        PlayerPrefs.SetInt("edgeScrolling", edgeScrolling ? 1 : 0);
    }

    public bool GetEdgeScrolling()
    {
        return edgeScrolling;
    }
}
