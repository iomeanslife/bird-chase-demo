using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    FlightSettings flightSettings;
    Rigidbody ownRigidbody;
    private BirdControls controls;
    private float movingY;
    private float movingX;
    private float acceleration;
    private float deceleration;    

    public Text speedText;

    void Awake()
    {    
        controls = new BirdControls();

        controls.Gameplay.LeftStick.performed += ctx =>
        {
            Debug.Log("Left Stick performed");
            //ownRigidbody.AddTorque(transform.up * ctx.ReadValue<Vector2>().x * rollSpeed * Time.fixedDeltaTime);
            //ownRigidbody.AddTorque(transform.right * ctx.ReadValue<Vector2>().y * rollSpeed * Time.fixedDeltaTime);
        };
        controls.Gameplay.LeftStick.canceled += ctx =>
        {
            Debug.Log("Left Stick cancel");
            // ownRigidbody.AddTorque(Vector2.zero);
        };

        controls.Gameplay.RightStick.performed += ctx =>
        {
            Debug.Log("Right Stick performed");
            //ownRigidbody.AddTorque(transform.forward * ctx.ReadValue<Vector2>().x * rollSpeed * Time.fixedDeltaTime);
            //ownRigidbody.AddTorque(transform.right * ctx.ReadValue<Vector2>().y * rollSpeed * Time.fixedDeltaTime);
        };
        controls.Gameplay.RightStick.canceled += ctx =>
        {
            Debug.Log("Right Stick cancel");
            //ownRigidbody.AddTorque(Vector2.zero);
        };



        controls.Gameplay.Pad.performed += ctx =>
        {
            movingY = ctx.ReadValue<Vector2>().y;
            movingX = ctx.ReadValue<Vector2>().x;
            Debug.Log("DPad performed");
        };

        controls.Gameplay.Accelerate.performed += ctx =>
        {
            acceleration = ctx.ReadValue<float>();
            Debug.Log("Accelerate performed");
        };

        controls.Gameplay.Decelerate.performed += ctx =>
        {
            deceleration = ctx.ReadValue<float>();
            Debug.Log("Decelerate performed");
        };

        controls.Gameplay.Accelerate.canceled += ctx =>
        {
            acceleration = ctx.ReadValue<float>();
            Debug.Log("Accelerate cancelled");
        };

        controls.Gameplay.Decelerate.canceled += ctx =>
        {
            deceleration = ctx.ReadValue<float>();
            Debug.Log("Decelerate cancelled");
        };
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        ownRigidbody = GetComponent<Rigidbody>(); ;
        flightSettings = GetComponent<FlightSettings>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Chomper>() != null)
        {
            Debug.LogWarning($"GAME OVER!!! {Time.time}");
            Manager.Instance.gameOverUi.SetActive(true);
        }

        if (other.gameObject.GetComponent<ItemCollection>() != null)
        {
            other.gameObject.SetActive(false);

            Manager.Instance.collectibleCount = Manager.Instance.collectibleCount + 1;
        }

        if (other.gameObject.GetComponent<Goal>() != null)
        {
            Manager.Instance.winningUi.SetActive(true);            
        }

        Debug.LogWarning($"Ding with {other.gameObject.name}! {Time.time}");
    }

    void FixedUpdate()
    {
        if (Manager.Instance.gameWon)
        {
            return;
        }

        ownRigidbody.AddForce(transform.forward *
            (1 + acceleration - deceleration * 0.5f ) *
            flightSettings.BasicThrustSpeed * Time.fixedDeltaTime);

        ownRigidbody.AddForce(transform.up * movingY * flightSettings.MoveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);
        ownRigidbody.AddForce(transform.right * movingX * flightSettings.MoveSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        ownRigidbody.AddTorque(transform.up * controls.Gameplay.LeftStick.ReadValue<Vector2>().x * flightSettings.RollSpeed * Time.fixedDeltaTime);
        ownRigidbody.AddTorque(transform.right * controls.Gameplay.LeftStick.ReadValue<Vector2>().y * flightSettings.RollSpeed * Time.fixedDeltaTime);

        ownRigidbody.AddTorque(transform.forward * controls.Gameplay.RightStick.ReadValue<Vector2>().x * flightSettings.RollSpeed * Time.fixedDeltaTime);

        speedText.text = $"Speed: {ownRigidbody.velocity.magnitude}";
        //speedText.text = $"Speed: {acceleration}; {deceleration}";
    }

    
}
