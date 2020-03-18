using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;
using Improbable.Gdk.QueryBasedInterest;
using Improbable;
using Tank;
using UnityEngine;

public class TankMovement : MonoBehaviour
{
    [Require] private TurretRotationWriter turretRotationWriter;
    //[Require] private InterestReader interestReader;

    [SerializeField] private float speed = 12f;
    [SerializeField] private float turnSpeed = 180f;
    [SerializeField] private float turretTurnSpeed = 180f;
    [SerializeField] private GameObject turret;

    private new Rigidbody rigidbody;
    private float movementInput;
    private float turnInput;
    private float turretInput;
    private float turretTurnInput;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody>();
        movementInput = 0;
        turnInput = 0;
        //interestReader.OnUpdate += OnInterestUpdate;
    }

    //private void OnInterestUpdate(Interest.Update update)
    //{
    //    Debug.Log("!!!UPDATE!!!");
    //}

    // Update is called once per frame
    void Update()
    {
        movementInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Q))
        {
            turretTurnInput = -1;
        }
        
        if (Input.GetKey(KeyCode.E))
        {
            turretTurnInput = 1;
        }

        if (Input.GetKeyUp(KeyCode.Q) || Input.GetKeyUp(KeyCode.E))
        {
            turretTurnInput = 0;
        }
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
        TurnTurret();
    }

    private void Move()
    {
        //Vector3 movement = transform.forward * movementInput * speed * Time.deltaTime;
        
        //rigidbody.MovePosition(rigidbody.position + movement);
    }

    private void Turn()
    {
        //float turn = turnInput * turnSpeed * Time.deltaTime;
        
        //Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        
        //rigidbody.MoveRotation(rigidbody.rotation * turnRotation);
    }

    private void TurnTurret()
    {
        float rotation = turretTurnInput * turretTurnSpeed;
        turret.transform.Rotate(0, rotation, 0);
        if (turretTurnInput != 0)
        {
            var update = new TurretRotation.Update
            {
                Rotation = rotation
            };
            turretRotationWriter.SendUpdate(update);
        }
    }
}
