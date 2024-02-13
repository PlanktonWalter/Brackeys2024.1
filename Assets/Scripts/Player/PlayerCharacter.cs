using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController), typeof(PlayerInteraction))]
public sealed class PlayerCharacter : Pawn
{

    public event Action Died;
    public event Action Respawned;

    [SerializeField] private Transform _head;
    [SerializeField] private PlayerInteraction _interactor;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private CharacterController _controller;
    private Vector3 _velocityXZ;
    private float _velocityY;
    private TimeSince _timeSinceLastDeath = new TimeSince(float.NegativeInfinity);
    private Vector3 _spawnPosition;
    private Quaternion _spawnRotation;

    public PlayerInteraction Interactor => _interactor;
    public Inventory Inventory => _inventory;
    public bool IsDead { get; private set; }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        _spawnPosition = transform.position;
        _spawnRotation = transform.rotation;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void PossessedTick()
    {
        if (IsDead == true && _timeSinceLastDeath > 5f)
        {
            Respawn();
            return;
        }

        UpdateRotation();

        FlatVector inputDirection = new FlatVector()
        {
            x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
            z = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0
        };

        inputDirection.Normalize();

        bool wantsJump = Input.GetKeyDown(KeyCode.Space);

        UpdateMovement(inputDirection, wantsJump);

        if (CanAct() == true)
        {
            if (Input.GetKeyDown(KeyCode.F) == true)
                _interactor.TryPerform(0);

            if (Input.GetKeyDown(KeyCode.E) == true)
                _interactor.TryPerform(1);

            if (Input.GetKeyDown(KeyCode.B) == true)
                _interactor.TryPerform(2);

            if (Input.GetKeyDown(KeyCode.Q) == true)
            {
                var items = _inventory.Content;
                if (items.Length > 0)
                {
                    var item = items[0];
                    _inventory.RemoveItem(item);
                    item.transform.position = _head.position;
                    item.Push(_head.forward * 250f + _velocityXZ * 45f);
                }
            }
        }
    }

    public void Kill()
    {  
        IsDead = true;
        _timeSinceLastDeath = new TimeSince(Time.time);
        Died?.Invoke();
        GetComponent<Animator>()?.SetBool("dead", true);
    }

    public void Respawn()
    {
        IsDead = false;
        Respawned?.Invoke();
        GetComponent<Animator>()?.SetBool("dead", false);
        transform.position = _spawnPosition;
        transform.rotation = _spawnRotation;
        _head.localRotation = Quaternion.identity;
    }

    private void UpdateRotation()
    {
        if (CanRotateHead() == false)
            return;

        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * 100f;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * 100f;

        var yRotation = transform.eulerAngles.y + mouseX * Time.deltaTime;
        var xRotation = _head.localEulerAngles.x - mouseY * Time.deltaTime;
        xRotation = ClampAngle(xRotation, -70f, 70f);

        transform.eulerAngles = new Vector3(0f, yRotation, 0f);
        _head.localEulerAngles = new Vector3(xRotation, 0f, 0f);

        float ClampAngle(float angle, float min, float max)
        {
            float start = (min + max) * 0.5f - 180;
            float floor = Mathf.FloorToInt((angle - start) / 360) * 360;
            return Mathf.Clamp(angle, min + floor, max + floor);
        }
    }

    private void UpdateMovement(FlatVector inputDirection, bool wantsJump)
    {
        Vector3 desiredVelocity = CanWalk() ?
            transform.TransformDirection(inputDirection) * GetSpeed() :
            Vector3.zero;
        
        _velocityXZ = Vector3.MoveTowards(_velocityXZ, desiredVelocity, 25f * Time.deltaTime);

        if (_controller.isGrounded == true)
        {
            _velocityY = wantsJump ? _jumpForce : -9.8f;
        }
        else
        {
            _velocityY -= 9.8f * Time.deltaTime;
        }

        Vector3 finalMove = new Vector3()
        {
            x = _velocityXZ.x,
            y = _velocityY,
            z = _velocityXZ.z,
        };

        finalMove *= Time.deltaTime;

        _controller.Move(finalMove);
    }

    private bool CanRotateHead()
    {
        return IsDead == false;
    }

    private bool CanAct()
    {
        return IsDead == false;
    }

    private bool CanWalk()
    {
        return IsDead == false;
    }

    private float GetSpeed()
    {
        return _speed;
    }

    public override Vector3 GetCameraPosition()
    {
        return _head.position;
    }

    public override Quaternion GetCameraRotation()
    {
        return _head.rotation;
    }

}
