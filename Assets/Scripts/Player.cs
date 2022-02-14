using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    [SerializeField] private List<Animator> modelsAnimators;

    [SerializeField] private Transform sideMovementRoot;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private Transform rightLimit;

    [SerializeField] private float sideMovementSensitivity;
    [SerializeField] private float sideMovementLerpSpeed;

    [SerializeField] private float forwardSpeed;


    private Vector2 inputDrag;
    private Vector2 previousMousePosition;


    private float leftLimitX => leftLimit.localPosition.x;
    private float rightLimitX => rightLimit.localPosition.x;

    private float sideMovementTarget = 0;
    private Vector2 mousePositionCM // Providing the same experience to everyone
    {
        get
        {
            Vector2 pixels = Input.mousePosition;
            var inches = pixels / Screen.dpi;
            var centimetres = inches * 2.54f; // 1 inch = 2.54 cm

            return centimetres;
        }
    }
    #endregion

    #region SINGLETON
    private static Player instance;
    public static Player Instance => instance ??= FindObjectOfType<Player>();
    #endregion
    private void Start()
    {
        CameraManager.Instance.SetStartMove();
        CameraManager.Instance.SetEndMove();
    }
    void Update()
    {
        HandleInput();
        if (GameManager.Instance.IsGameStarted() && CameraManager.Instance.StartCameraLocated)
        {
            HandleForwardMovement();
            HandleSideMovement();
        }
    }

    private void HandleForwardMovement()
    {
        if (!PlayerCollisionController.Instance.End)
        {
            transform.position += transform.forward * Time.deltaTime * forwardSpeed;
        }
    }

    private void HandleSideMovement()
    {
        if (!PlayerCollisionController.Instance.End)
        {
            sideMovementTarget += inputDrag.x * sideMovementSensitivity;
            sideMovementTarget = Mathf.Clamp(sideMovementTarget, leftLimitX, rightLimitX);

            var localPos = sideMovementRoot.localPosition;

            localPos.x = Mathf.Lerp(localPos.x, sideMovementTarget, Time.deltaTime * sideMovementLerpSpeed);

            sideMovementRoot.localPosition = localPos;
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            previousMousePosition = mousePositionCM;
        }
        if (Input.GetMouseButton(0))
        {
            var deltaMouse = (Vector2)mousePositionCM - previousMousePosition;
            inputDrag = deltaMouse;
            previousMousePosition = mousePositionCM;
        }
        else
        {
            inputDrag = Vector2.zero;
        }
    }
    public void AnimationChange()
    {
        foreach (Animator modelAnimator in modelsAnimators)
        {
            if (!PlayerCollisionController.Instance.End && GameManager.Instance.IsGameStarted())
            {
                modelAnimator.SetBool("Walk", true);
            }
            if (PlayerCollisionController.Instance.FullReward && PlayerCollisionController.Instance.End)
            {
                modelAnimator.SetBool("FullReward", true);
            }
            if (!PlayerCollisionController.Instance.FullReward && PlayerCollisionController.Instance.End)
            {
                modelAnimator.SetBool("NFullReward", true);
            }
        }
    }
    public void AnimationReset()
    {
        foreach (Animator modelAnimator in modelsAnimators)
        {
            modelAnimator.SetBool("Walk", false);
            modelAnimator.SetBool("FullReward", false);
            modelAnimator.SetBool("NFullReward", false);
        }
    }
}
