using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    // Mostly copied, read more at https://en.wikipedia.org/wiki/PID_controller

    private readonly VectorPid angularVelocityController = new VectorPid(33.7766f, 0, 0.2553191f);
    private readonly VectorPid headingController = new VectorPid(9.244681f, 0, 0.06382979f);

    //private readonly VectorPid angularVelocityController = new VectorPid(3.37766f, 0, 00.2553191f);
    //private readonly VectorPid headingController = new VectorPid(0.9244681f, 0, 00.06382979f);

    //private readonly VectorPid angularVelocityController = new VectorPid(337.766f, 0, 2.553191f);
    //private readonly VectorPid headingController = new VectorPid(92.44681f, 0, 0.6382979f);


    public Transform target;
    
    private new Rigidbody rigidbody;
    private FlightSettings flightSettings;

    public void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        flightSettings = GetComponent<FlightSettings>();
    }

    public void FixedUpdate()
    {
        if (Manager.Instance.gameWon)
        {
            return;
        }
        // Heading to target logic.

        var angularVelocityError = rigidbody.angularVelocity * -1;
        Debug.DrawRay(transform.position, rigidbody.angularVelocity * 10, Color.black);

        var angularVelocityCorrection = angularVelocityController.Update(angularVelocityError, Time.deltaTime);
        Debug.DrawRay(transform.position, angularVelocityCorrection, Color.green);

        rigidbody.AddTorque(angularVelocityCorrection);

        var desiredHeading = target.position - transform.position;
        Debug.DrawRay(transform.position, desiredHeading, Color.magenta);

        var currentHeading = transform.forward;
        Debug.DrawRay(transform.position, currentHeading * 15, Color.blue);

        var headingError = Vector3.Cross(currentHeading, desiredHeading);
        var headingCorrection = headingController.Update(headingError, Time.deltaTime);

        rigidbody.AddTorque(headingCorrection * (flightSettings.RollSpeed / 100));

        // flying forward logic.

        GetComponent<Rigidbody>().AddForce(transform.forward * flightSettings.BasicThrustSpeed * Time.fixedDeltaTime);
    }
}

public class VectorPid
{
    public float pFactor, iFactor, dFactor;

    private Vector3 integral;
    private Vector3 lastError;

    public VectorPid(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }

    public Vector3 Update(Vector3 currentError, float timeFrame)
    {
        integral += currentError * timeFrame;
        var deriv = (currentError - lastError) / timeFrame;
        lastError = currentError;
        return currentError * pFactor
            + integral * iFactor
            + deriv * dFactor;
    }
}