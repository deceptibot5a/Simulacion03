using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class myBall : MonoBehaviour
{
    public enum BallRunMode {
        Friction,
        FluidFriction,
        Gravity
    }
    public float Mass => mass;
    private MyVector position;
    [SerializeField] private MyVector velocity;
    [SerializeField] private MyVector acceleration;
    [SerializeField] private float mass = 1f;
    [SerializeField] private BallRunMode runMode;

    [SerializeField] private MyVector wind;
    [SerializeField] private MyVector gravity;

    [SerializeField] private myBall otherBall;
    [Range(0f, 1f)] [SerializeField] private float fricctionCoefficient;
    [Range(0f, 1f)] [SerializeField] private float dampingFactor;
    [SerializeField] Camera camera;

    void Start() {
        position = new MyVector(transform.position.x, transform.position.y);
    }
    private void FixedUpdate() {
        Vector2 p = Vector2.one;
        p.Normalize();
        Vector2 pUnit = p.normalized;
        acceleration *= 0f;
        if (runMode != BallRunMode.Gravity) {
            MyVector weight = gravity * mass; //Weight
            ApplyForce(weight);
        } if (runMode == BallRunMode.FluidFriction) {
            ApplyFluidFriction();             //Fluid friction
        } else if (runMode == BallRunMode.Friction) {
            ApplyFriction();                  // Friction
        } else if (runMode == BallRunMode.Gravity) {
            ApplyGravity();
        }

        //ApplyForce(wind);
        Move();
    }

    private void Update() {
        position.Draw(Color.blue);
        velocity.Draw(position, Color.red);
        //acceleration.Draw(position, Color.green);
    }

    public void Move() {
        velocity = velocity + acceleration * Time.fixedDeltaTime;
        position = position + velocity * Time.fixedDeltaTime;   //Euler integrator
        if (runMode == BallRunMode.Gravity) {
            CheckLimitSpeed();
        } else CheckWorldBoxBounds();

        transform.position = new Vector3(position.x, position.y);
    }
    private void ApplyForce(MyVector force) {
        acceleration += force / mass;
    }
    private void ApplyFriction() {
        float N = -mass * gravity.y;
        MyVector friction = -fricctionCoefficient * N * velocity.normalized;
        ApplyForce(friction);
        friction.Draw(position, Color.blue);
    }
    private void ApplyFluidFriction() {
        if (transform.localPosition.y <= 0) {
            float frontalArea = transform.localScale.x;
            float rho = 1;
            float fluidDrag = 1;
            float velocityMag = velocity.magnitude;
            float scalarPart = -0.5f * rho * velocityMag * velocityMag * frontalArea * fluidDrag;
            MyVector friction = scalarPart * velocity.normalized;
            ApplyForce(friction);
        }
    }
    private void ApplyGravity() {
        MyVector diff = otherBall.position - position;
        float distance = diff.magnitude;
        float scalarPart = (mass * otherBall.mass) / (distance * distance);
        MyVector gravity = scalarPart * diff.normalized;
        ApplyForce(gravity);
    }
    private void CheckLimitSpeed(float maxSpeed = 10) {
        if (velocity.magnitude > maxSpeed) {
            velocity = maxSpeed * velocity.normalized;
        }
    }
    private void CheckWorldBoxBounds() {
        if (position.x > camera.orthographicSize) {             //Right
            velocity.x *= -1;
            position.x = camera.orthographicSize;
            velocity *= dampingFactor; //Damping factor
        } else if (position.x < -camera.orthographicSize) {     //Left
            velocity.x *= -1;
            position.x = -camera.orthographicSize;
            velocity *= dampingFactor; //Damping factor
        } else if (position.y < -camera.orthographicSize) {     //Down
            velocity.y *= -1;
            position.y = -camera.orthographicSize;
            velocity *= dampingFactor; //Damping factor
        } else if (position.y > camera.orthographicSize) {      //Up
            velocity.y *= -1;
            position.y = camera.orthographicSize;
            velocity *= dampingFactor; //Damping factor
        }
    }
}
