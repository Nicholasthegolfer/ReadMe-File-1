using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    public AudioClip puttSound, holeSound, peacefulSound;
    public float maxPower;
    public float changeAngleSpeed;
    public float lineLength;
    public Slider powerslider;
    public TextMeshProUGUI  puttCountLabel;
    public float minHoleTime;
    public Transform startPosition;
    public LevelManager levelManager;

    private LineRenderer line;
    private Rigidbody ball;
    private float angle;
    private float PowerUPTime;
    private float power;
    private int putts;
    private float holeTime;
    private Vector3 lastPosition;
    private AudioSource audioSource;

    void Start()
    {
       audioSource.PlayOneShot(peacefulSound);
    }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ball = GetComponent<Rigidbody>();
        ball.maxAngularVelocity = 1000;
        line = GetComponent<LineRenderer>();
        startPosition.GetComponent<MeshRenderer>().enabled = false;
    }
    
    //Aligning the ball with A or D and stroking the ball using a powerslider, by pressing spacebar.
    void Update()
    {
        if (ball.linearVelocity.magnitude < 0.01f)
        {
            if (Input.GetKey(KeyCode.A))
            {
                ChangeAngle(-1);
            }
            if (Input.GetKey(KeyCode.D))
            {
                ChangeAngle(1);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Putt();
            }
            if (Input.GetKey(KeyCode.Space))
            {
                PowerUP();
            }
            UpdateLinePositions();
        }
        else
        {
            line.enabled = false;
        }
    }

    private void ChangeAngle(int direction)
    {
           angle += changeAngleSpeed * Time.deltaTime * direction;
    }

     private void UpdateLinePositions()
    {
        if (holeTime == 0) { line.enabled = true; }
        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position + Quaternion.Euler(0, angle,0) * Vector3.forward * lineLength);
    }

    private void Putt()
    {
        audioSource.PlayOneShot(puttSound);
        lastPosition = transform.position;
        ball.AddForce(Quaternion.Euler(0, angle,0) * Vector3.forward * maxPower * power, ForceMode.Impulse);
        power = 0;
        powerslider.value = 0;
        PowerUPTime = 0;
        putts++;
        puttCountLabel.text = putts.ToString();
    }

    private IEnumerator PuttSound;

    private void PowerUP()
    {
        PowerUPTime += Time.deltaTime;
        power = Mathf.PingPong(PowerUPTime, 1);
        powerslider.value = power;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hole")
        {
            CountHoleTime();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hole")
        {
            audioSource.PlayOneShot(holeSound);
            
        }
       
    }

    private void CountHoleTime()
    {
        holeTime += Time.deltaTime;

        if (holeTime >= minHoleTime)
        {
            levelManager.NextPlayer(putts);
            Debug.Log("I'm in the hole and it only took me " + putts + " putts to get me in.");
            holeTime = 0;
        }
    }


    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hole")
        {
            LeftHole();
        }
    }

    private void LeftHole()
    {
        holeTime = 0;
    }

  //Collision for Out Of Bounds.
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Out Of Bounds")
        {
            transform.position = lastPosition;
            ball.linearVelocity = Vector3.zero;
            ball.angularVelocity = Vector3.zero;
        }
    }

    public void SetupBall(Color color)
    {
        Debug.Log("SetupBall called with color: " + color);
        transform.position = startPosition.position;
        angle = startPosition.rotation.eulerAngles.y;
        ball.linearVelocity = Vector3.zero;
        ball.angularVelocity = Vector3.zero;
        GetComponent<MeshRenderer>().material.SetColor("_Color", color);
        line.material.SetColor("_Color", color);
        line.enabled = true;
        putts = 0;
        puttCountLabel.text = "0";
    }
}

