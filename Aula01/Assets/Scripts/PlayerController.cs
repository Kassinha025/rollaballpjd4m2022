using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
   private Controls _controls;
   private PlayerInput _playerInput;
   private Camera _mainCamera;
   private Vector2 _moveInput;
   private Rigidbody _rigidbody;
   private bool _isGrounded;
   public float moveMultiplayer;
   public float maxVelocity;
   public float rayDistance;
   public LayerMask LayerMask;
   public float jumpForce;
   

   private void OnEnable()
   {
       _rigidbody = GetComponent<Rigidbody>();
       
       _controls = new Controls();

       _playerInput = GetComponent<PlayerInput>();
       
       _mainCamera = Camera.main;

       _playerInput.onActionTriggered += OnActionTriggered;
       
   }

   private void OnDisable()
   {
       _playerInput.onActionTriggered -= OnActionTriggered;
   }

   private void OnActionTriggered(InputAction.CallbackContext obj)
   {
       if (obj.action.name.CompareTo(_controls.Gameplay.Move.name) == 0) 
       {
           _moveInput = obj.ReadValue<Vector2>();
       }

       if (obj.action.name.CompareTo(_controls.Gameplay.Jump.name) == 0)
       {
           if (obj.performed) Jump();
       }
   }

   private void Move()
   {

       Vector3 camForward = _mainCamera.transform.forward;
       Vector3 camRight = _mainCamera.transform.right;
       camForward.y = 0;
       camRight.y = 0;
       
       _rigidbody.AddForce((camForward * _moveInput.y + camRight * _moveInput.x ) * moveMultiplayer * Time.deltaTime);
   }
   private void FixedUpdate()
   {
       Move();
   }

   private void LimitVelocity()
   {
       Vector3 velocity = _rigidbody.velocity;
       if (Math.Abs(velocity.x) > maxVelocity) velocity.x = Math.Sign(velocity.x) * maxVelocity;

       velocity.z = Mathf.Clamp(value: velocity.z, min: -maxVelocity, maxVelocity);

       _rigidbody.velocity = velocity;
   }

   private void CheckGround()
   {
       RaycastHit collision;

       if (Physics.Raycast(origin: transform.position, direction: Vector3.down, out collision, rayDistance,
           (int) LayerMask))
       {
           _isGrounded = true;
       }
       else
       {
           _isGrounded = false;
       }
   }

   private void Jump()
   {
       if (_isGrounded)
       {
           _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
       }
   }

   private void Update()
   {
       CheckGround();
   }

   private void OnDrawGizmos()
   {
       Debug.DrawRay(start: transform.position, dir: Vector3.down * rayDistance, Color.yellow);
   }
}
