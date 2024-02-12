using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController), typeof(PlayerInteraction))]
public sealed class PlayerCharacter : Pawn
{

    [SerializeField] private Transform _head;
    [SerializeField] private PlayerInteraction _interactor;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private CharacterController _controller;
    private float xRotation;
    private float yRotation;
    private Vector3 _velocityXZ;
    private float _velocityY;

    public PlayerInteraction Interactor => _interactor;
    public Inventory Inventory => _inventory;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        //_interactor = GetComponent<PlayerInteraction>();
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = _head.localEulerAngles.x;
        yRotation = transform.eulerAngles.y;
    }

    public override void PossessedTick()
    {
        UpdateRotation();

        FlatVector inputDirection = new FlatVector()
        {
            x = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0,
            z = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0
        };

        inputDirection.Normalize();

        bool wantsJump = Input.GetKeyDown(KeyCode.Space);

        UpdateMovement(inputDirection, wantsJump);

        if (Input.GetKeyDown(KeyCode.F) == true)
            _interactor.TryPerform(0);

        if (Input.GetKeyDown(KeyCode.E) == true)
            _interactor.TryPerform(1);

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

    private void UpdateRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * 100f;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * 100f;

        yRotation += mouseX * Time.deltaTime;
        xRotation -= mouseY * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);

        transform.eulerAngles = new Vector3(0f, yRotation, 0f);
        _head.localEulerAngles = new Vector3(xRotation, 0f, 0f);
    }

    private void UpdateMovement(FlatVector inputDirection, bool wantsJump)
    {
        Vector3 desiredVelocity = transform.TransformDirection(inputDirection) * GetSpeed();
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

    private bool CanWalk()
    {
        return true;
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

public abstract class Pawn : MonoBehaviour
{
    public bool WantsUnpossess { get; private set; }
    public Player Player { get; private set; }

    public abstract Vector3 GetCameraPosition();
    public abstract Quaternion GetCameraRotation();
    public virtual void PossessedTick() { }

    public virtual void OnPossessed(Player player)
    {
        Player = player;
    }

    public virtual void OnUnpossessed()
    {
        Player = null;
    }

    protected void Unpossess()
    {
        WantsUnpossess = true;
    }

}
