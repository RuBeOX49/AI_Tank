using System;
using UnityEngine;

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
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.

    [SerializeField] private Path path;
    [SerializeField] private bool isLooping;
    [SerializeField] private float slowingDistanceThreshold = 7f;

    private int currentPoint = 0;

    public int minDistToAvoid = 5;
    public LayerMask layerMask;

    private float speedMultiplier = 0.0f;
    private void Awake ()
    {
        m_Rigidbody = GetComponent<Rigidbody> ();
    }


    private void OnEnable ()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }


    private void Start ()
    {
        // The axes names are based on player number.
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        // Store the original pitch of the audio source.
        m_OriginalPitch = m_MovementAudio.pitch;
    }


    private void Update ()
    {

        speedMultiplier += Time.deltaTime;

        //Search for the current pathpoint
        Vector3 currPointPos = path.GetPoint(currentPoint);
        float newAngle;
        //Search for any obstacles
        if(Physics.BoxCast(
            transform.position - transform.forward,
            transform.localScale * 1.75f,
            transform.forward,
            out RaycastHit hit,
            Quaternion.identity,
            minDistToAvoid,
            layerMask)){
                //If obstacles, avoid
                var myHit = hit.normal;
                myHit.y = 0;

                var myFoward = transform.forward;
                myFoward.y = 0;
                newAngle = Vector3.SignedAngle(myFoward, myFoward + myHit, Vector3.up);
                DrawArrow.ForDebug(transform.position, myFoward+myHit);
            } else {
                var myFoward = transform.forward;
                myFoward.y = 0;
                newAngle = Vector3.SignedAngle(myFoward, myFoward + (path.GetPoint(currentPoint) - myFoward), Vector3.up);
                DrawArrow.ForDebug(transform.position,myFoward + (path.GetPoint(currentPoint) - myFoward));
            }
        
        //If near to the pathpoint, search for next pathpoint (if looping)
                Vector3 distanceToPoint = transform.position - path.GetPoint(currentPoint);
                if(distanceToPoint.magnitude < path.radius){
                    Debug.Log("Swapping to next point");
                    currentPoint++;
                }
                if(isLooping)
                    currentPoint %= path.Length;
                else {
                    if(currentPoint == path.Length - 1){
                        speedMultiplier -= Time.deltaTime;
                    }
                }

        speedMultiplier = Math.Clamp(speedMultiplier, 0f, 1f);

        m_TurnInputValue = newAngle;

        EngineAudio ();

    
    }


    private void EngineAudio ()
    {
        // If there is no input (the tank is stationary)...
        if (Mathf.Abs (m_MovementInputValue) < 0.1f && Mathf.Abs (m_TurnInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = UnityEngine.Random.Range (m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play ();
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


    private void FixedUpdate ()
    {
        
    }


    private void Move ()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * speedMultiplier * m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void Turn ()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation);
    }
}