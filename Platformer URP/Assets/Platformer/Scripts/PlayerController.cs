using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using KBCore.Refs;
using Utilties;
using System;
using TMPro;
using UnityEngine.SceneManagement;

//you can reference other scripts if you change the namespace!!




namespace Platformer
{

    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        //[SerializeField, Self] CharacterController controller;
        [SerializeField, Self] Animator animator;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Self] Rigidbody rb;
        [SerializeField, Self] GroundChecker groundChecker;
   


        [Header("Settings")]
        [SerializeField] float walkSpeed = 5f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpMaxHeight = 2f;
        [SerializeField] float Invuln = 2f; 
        [SerializeField] float gravityMultiplier = 2.5f;
        [SerializeField] private TextMeshProUGUI starCount;


        Transform mainCameraTransform;
        float currentSpeed;
        float velocity;
        
        public float jumpVelocity;

        public float playerHealth;
        public float maxHealth;
        public AudioClip pained;
        public AudioSource audioSource;
        public AudioClip jump;
        public AudioSource audioSource2;
        public float stars;
        public GameObject Star1; 
        public AudioSource audioSource3;
        public AudioClip starCollected;
        public GameObject youNeedStarsText;

        private bool StarsActive = false;
       



        Vector3 movement;

        List<Timer> timers;

        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        CountdownTimer invulnerabilityTimer;


        static readonly int Speed = Animator.StringToHash("Speed");

        void Awake()
        {
            mainCameraTransform = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            freeLookVCam.OnTargetObjectWarped(transform, transform.position - Vector3.forward);
            rb.freezeRotation = true;

            //start timers
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);
            invulnerabilityTimer = new CountdownTimer(Invuln);
            timers = new List<Timer>(capacity: 3) { jumpTimer, jumpCooldownTimer, invulnerabilityTimer};
            
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();
        }
        void Start()
        {
            input.EnablePlayerActions();
        }

        void OnEnable()
        {
            input.Jump += OnJump;
        }

        void OnDisable()
        {
            input.Jump -= OnJump;
        }
        public void DrawStars()
        {

        }
        public void OnJump(bool performed)
        {
            if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpTimer.Start();
                audioSource2.PlayOneShot(jump, 1.0F);

            } 
            else if (!performed && jumpTimer.IsRunning)
            {
                jumpTimer.Stop();
               
            }
        }


        void Update()
        {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            //HandleMovement();
            UpdateAnimator();
            HandleTimers();
            Debug.Log(jumpCooldownTimer.Progress);
        }

        void FixedUpdate()
        {
            HandleMovement();
            HandleJump();
        }
        void HandleTimers()
        {
            foreach (var timer in timers)
            {
                timer.Tick(Time.deltaTime);
            }
        }
       
        void HandleMovement()
        {
            //var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;
            var adjustedDirection = Quaternion.AngleAxis(mainCameraTransform.eulerAngles.y, Vector3.up) * movement;
            if (adjustedDirection.magnitude >= 0f)
            {
                HandleHorizontalMovement(adjustedDirection);
                var targetRotation = Quaternion.LookRotation(adjustedDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.LookAt(transform.position + adjustedDirection);

                //var adjustedMovment = adjustedDirection * (walkSpeed * Time.deltaTime);
                //controller.Move(adjustedMovment);

                currentSpeed = Mathf.SmoothDamp(currentSpeed, adjustedDirection.magnitude, ref velocity, smoothTime);

            }
            else
            {
                currentSpeed = Mathf.SmoothDamp(currentSpeed, 0, ref velocity, smoothTime);
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }

            void HandleHorizontalMovement(Vector3 adjustedDirection)
            {
                Vector3 velocity = adjustedDirection * walkSpeed * Time.fixedDeltaTime;
                rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);
            }

        }

        void UpdateAnimator()
        {
            animator.SetFloat(Speed, currentSpeed);
        }

         public void HandleJump()
        {
           
            if (!jumpTimer.IsRunning && groundChecker.IsGrounded)
            {
                jumpVelocity = 0f;
                jumpTimer.Stop();
                return;
            }
            if (jumpTimer.IsRunning)
            {
                float launchPoint = 0.9f;
                if (jumpTimer.Progress > launchPoint)
                {
                    //weird maths i dont get to calculate the jump height, who the fuck is squirt?
                    jumpVelocity = Mathf.Sqrt(2 * jumpMaxHeight * Mathf.Abs(Physics.gravity.y));
                }
                else
                {
                    jumpVelocity += (1 - jumpTimer.Progress) * jumpForce * Time.fixedDeltaTime;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    walkSpeed += 100;
                }

                rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        

            }

           
        }
        public void TakeDamage(float amount)
        {
            if (!invulnerabilityTimer.IsRunning)
            {
                playerHealth -= amount;
                OnPlayerDamaged?.Invoke();
                animator.Play("Damaged");
                audioSource.PlayOneShot(pained, 1.0F);
                invulnerabilityTimer.Start();
            }
         
            
            
        }

        public static event Action OnPlayerDamaged;

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Lava"))
            {
                jumpTimer.Stop();
                jumpTimer.Start();
                TakeDamage(1);


            }

            if (collision.gameObject.CompareTag("QuickSand"))
            {
                TakeDamage(1);
            }

            if (collision.gameObject.CompareTag("Star"))
            {
                Destroy(collision.gameObject);

                stars += 1;
                DrawStars();
                audioSource3.PlayOneShot(starCollected, 1.0F);
                starCount.text = stars.ToString();
            }

            if (collision.gameObject.CompareTag("enemy heads"))
            {
                Destroy(collision.transform.parent.parent.gameObject);
            }

             if (collision.gameObject.CompareTag("Mini boss heads"))
            {
                jumpTimer.Stop();
                jumpTimer.Start();
                


            }

            if (collision.gameObject.CompareTag("Door"))
            {
                if (stars >= 4)
                {
                    SceneManager.LoadScene("Area 2");
                } 

                else
                {
                    youNeedStarsText.SetActive(true);
                    StarsActive = true;
                    
                }
            }

        }
        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("Door"))
            {
                if (StarsActive == true)
                {
                    youNeedStarsText.SetActive(false);
                    StarsActive = false;
                }
            }
        }
       
    }
}

    







