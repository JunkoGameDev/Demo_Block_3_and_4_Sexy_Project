using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonController : MonoBehaviour
{
    //input fields
    //private ThirdPersonActionsAsset playerActionsAsset;
    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction move;
    private InputAction interact;

    //movement fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    //Action fields
    [HideInInspector] public GameObject InteractableItem;

    [SerializeField]
    private Camera playerCamera;
    private Animator animator;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        animator = this.GetComponent<Animator>();

        //playerActionsAsset = new ThirdPersonActionsAsset();
        inputAsset = this.GetComponent<PlayerInput>().actions;
        player = inputAsset.FindActionMap("Player");
    }

    private void OnEnable()
    {
        //playerActionsAsset.Player.Jump.started += DoJump;
        //playerActionsAsset.Player.Attack.started += DoAttack;
        //move = playerActionsAsset.Player.Move;
        //playerActionsAsset.Player.Enable();
        player.FindAction("Jump").started += DoJump;
        move = player.FindAction("Move");
        interact = player.FindAction("Interact");
        player.Enable();
    }

    private void OnDisable()
    {
        //playerActionsAsset.Player.Jump.started -= DoJump;
        //playerActionsAsset.Player.Attack.started -= DoAttack;
        //playerActionsAsset.Player.Disable();
        player.FindAction("Jump").started -= DoJump;
        player.Disable();
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.linearVelocity.y < 0f)
            rb.linearVelocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;

        Vector3 horizontalVelocity = rb.linearVelocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.linearVelocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.linearVelocity.y;

        LookAt();

        if(interact.ReadValue<bool>() && InteractableItem != null)
        {
            //Add Interaction effect when compendium and items are done
        }
    }

    private void LookAt()
    {
        Vector3 direction = rb.linearVelocity;
        direction.y = 0f;

        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            this.rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.5f))
            return true;
        else
            return false;
    }

    public void canInteract(GameObject _ItemToInteractWith)
    {
        InteractableItem = _ItemToInteractWith;
    }

    public void cannotInteract()
    {
        InteractableItem = null;
    }
}