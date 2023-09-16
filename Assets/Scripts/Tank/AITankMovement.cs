using System;
using UnityEngine;
public enum pathType
{
    path_simple,
    path_loop,
    path_patrol,
}

public class AITankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
    public float m_Speed = 12f;                 // How fast the tank moves forward and back.
    public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
    public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
    public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.


    private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.

    [SerializeField] private Path path;


    [SerializeField] private pathType path_type;
    private bool patrollingFoward = true;
    [SerializeField] private float slowingDistanceThreshold = 7f;
    [SerializeField] private float mass = 8f;
    [SerializeField] private float speed = 5f;
    private int currentPoint = 0;

    public int minDistToAvoid = 5;
    public LayerMask layerMask;

    private float speedMultiplier = 0.0f;

    private float currSpeed;

    private Vector3 velocity;
    private void Awake()
    {
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
    }


    private void Start()
    {
        // The axes names are based on player number.
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update()
    {

        // Calculate current speed
        currSpeed = speed * Time.deltaTime;


        //calculate velocity - Steering



        if (currentPoint >= path.Length - 1 && path_type == pathType.path_simple)
        {
            velocity += Steer(path.GetPoint(currentPoint), true);
        }
        else velocity += Steer(path.GetPoint(currentPoint));

        //Move






        SearchNextPoint();

        transform.position += velocity;

        //Rotate

        transform.rotation = Quaternion.LookRotation(velocity);


        EngineAudio();


    }

    private void SearchNextPoint()
    {
        //If near to the pathpoint, search for next pathpoint based on pathtype
        Vector3 distanceToPoint = transform.position - path.GetPoint(currentPoint);
        if (distanceToPoint.magnitude < path.radius)
        {
            switch (path_type)
            {
                case pathType.path_simple:
                    if (currentPoint < path.Length - 1)
                        currentPoint++;
                    break;
                case pathType.path_loop:
                    currentPoint++;
                    currentPoint %= path.Length;
                    break;
                case pathType.path_patrol:
                    if (patrollingFoward)
                    {
                        //its going 0-path.Length - 1;
                        if (currentPoint == path.Length - 1)
                            patrollingFoward = false;
                        else currentPoint++;
                    }
                    else
                    {
                        if (currentPoint == 0)
                            patrollingFoward = true;
                        else currentPoint--;
                    }
                    break;
                default:
                    currentPoint = 0;
                    break;
            }
        }
    }

    private void EngineAudio()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and play.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    private Vector3 Steer(Vector3 target, bool isEndpoint = false)
    {
        Vector3 desiredVelocity = Vector3.zero;
        if (Physics.BoxCast(
            transform.position - transform.forward,
            transform.localScale * 1.75f,
            transform.forward,
            out RaycastHit hit,
            Quaternion.identity,
            minDistToAvoid,
            layerMask))
        {
            //Get the normal of the hitpoint (of the boxCast)
            Vector3 hitNormal = hit.normal;
            hitNormal.y = 0f;
            desiredVelocity = transform.forward + hitNormal;
        }
        else
        {
            desiredVelocity = target - transform.position;
        }
        float dist = desiredVelocity.magnitude;
        desiredVelocity = desiredVelocity.normalized;

        if (isEndpoint && dist < slowingDistanceThreshold)
            desiredVelocity *= (currSpeed * dist / 10f);
        else desiredVelocity *= currSpeed;

        Vector3 steeringForce = desiredVelocity - velocity;
        Vector3 acceleration = steeringForce / mass;

        return acceleration;
    }

    private void FixedUpdate()
    {

    }

    public void sendPathInfo(Path path, pathType type){
        this.path = path;
        path_type = type;
    }




}