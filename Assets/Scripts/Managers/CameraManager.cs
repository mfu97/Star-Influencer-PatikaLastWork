using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject target;


    public float camSmoothness = 6;
    private float xPos = 0;


    private bool startTargetFollowing;
    private bool endTargetFollowing;
    private bool startMoveOn;
    private bool endMove;

    public bool StartCameraLocated;

    private float startMoveTimer;
    private float startMoveDuration = 2;

    private float endMoveTimer;
    private float endMoveDuration = 2f;

    private Vector3 endPos;
    private Vector3 endLastPos;
    private Vector3 startPos;
    private Vector3 startLastPos;

    private Quaternion startRot;
    private Quaternion startLastRot;
    private Quaternion endRot;
    private Quaternion endLastRot;

    private static CameraManager instance;
    public static CameraManager Instance => instance ??= FindObjectOfType<CameraManager>();
    private void LateUpdate()
    {
        if (startMoveOn)
        {
            StartMove();
        }
        if (startTargetFollowing)
        {
            xPos = Mathf.Lerp(transform.position.x, target.transform.position.x, Time.deltaTime * camSmoothness);
            transform.position = new Vector3(xPos, target.transform.position.y, target.transform.position.z);
        }
        if (PlayerCollisionController.Instance.End && endMove)
        {
            EndMove();
        }
    }

    public void SetStartMove()
    {
        startPos = mainCamera.transform.localPosition;
        startLastPos = new Vector3(0, 3.5f, -4.5f);
        startRot = mainCamera.transform.rotation;
        startLastRot = Quaternion.Euler(18.5f, 0, 0);
        startMoveOn = true;
    }
    public void SetEndMove()
    {
        endPos = new Vector3(0, 3.5f, -4.5f);
        endLastPos = new Vector3(2f, 2, -0.1f);
        endRot = Quaternion.Euler(-18.5f, 0, 0);
        endLastRot = Quaternion.Euler(18.5f, -90, 0);
        endMove = true;
    }
    private void StartMove()
    {
        if (startMoveTimer < startMoveDuration)
        {
            startMoveTimer += Time.deltaTime;
            mainCamera.transform.localPosition = Vector3.Lerp(startPos, startLastPos, startMoveTimer / startMoveDuration);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, startLastRot, startMoveTimer / startMoveDuration);
        }
        else
        {
            mainCamera.transform.localPosition = startLastPos;
            StartCameraLocated = true;
            startMoveTimer = 0;
            startMoveOn = false;
            startTargetFollowing = true;
        }
    }
    private void EndMove()
    {
        if (endMoveTimer < endMoveDuration)
        {
            endMoveTimer += Time.deltaTime;
            mainCamera.transform.localPosition = Vector3.Lerp(endPos, endLastPos, endMoveTimer / endMoveDuration);
            mainCamera.transform.rotation = Quaternion.Lerp(endRot, endLastRot, endMoveTimer / endMoveDuration);
        }
        else
        {
            mainCamera.transform.localPosition = endLastPos;
            //endTargetFollowing = true;
            endMoveTimer = 0;
            endMove = false;
        }
    }

    public void ResetCameraPosition()
    {
        StartCameraLocated = false;

        mainCamera.transform.localPosition = startPos;
        mainCamera.transform.localRotation = startRot;
        SetStartMove();
        SetEndMove();
    }
}
