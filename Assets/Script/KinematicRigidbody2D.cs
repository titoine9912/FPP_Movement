using UnityEngine;

namespace Pixel2018
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KinematicRigidbody2D : MonoBehaviour
    {
        private const int NbPreallocatedRaycastHit = 16;

        [Header("Physics")] [SerializeField] [Tooltip("Gravity force.")]
        private Vector2 gravity = new Vector2(0, -9.81f);
        
        [SerializeField] [Tooltip("How much gravity affects this object.")]
        private float gravityMultiplier = 1f;

        [SerializeField] [Tooltip("Arctan value of the maximum slope angle considered as ground.")]
        private float maxGroundSlopeAngleArctan = 1 - 0.65f; //About 33°.

        [SerializeField] private float maxWallSlopeAngleArctan = 1 - 0.65f;

        [SerializeField] [Tooltip("Simulation is ignored when velocity is bellow this threshold.")]
        private float sleepVelocity = 0.001f;

        [SerializeField] [Tooltip("Precision of the simulation. Don't make it lower than 0.01.")]
        private float deltaPrecision = 0.01f;

        [SerializeField] private Vector2 wallJumpVector = new Vector2(-50, 10);
        
        public Sprite sprite1;
        public Sprite sprite2;
        private SpriteRenderer spriteRenderer;
        private bool facingRight = true;

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        [Tooltip("Show debug informatio like velocity, ground normal and ground movement vector.")]
        private bool showDebugInformation;
#endif
        //
        private Vector2 velocity;
        private Vector2 targetVelocity;
        private Vector2 latestVelocity;
        private bool isGrounded;
        private bool isOnWall;
        private float direction=0;
        private float lastGroundedTime;
        private Vector2 groundNormal; //Vector perpenticular to current ground surface.

        private new Rigidbody2D rigidbody;
        private ContactFilter2D contactFilter;
        private RaycastHit2D[] preallocaRaycastHits;

        public Vector2 Velocity
        {
            get { return latestVelocity; }
            set
            {
                targetVelocity = value;
                latestVelocity = value;
            }
        }

        public bool IsGravityIgnored { get; set; }

        public bool IsGrounded
        {
            get { return isGrounded; }
        }

        public bool IsOnWall
        {
            get { return isOnWall; }
        }

        public float TimeSinceAirborne
        {
            get { return Time.time - lastGroundedTime; }
        }

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            contactFilter.useTriggers = false;
            contactFilter.useLayerMask = true;
            preallocaRaycastHits = new RaycastHit2D[NbPreallocatedRaycastHit];

            IsGravityIgnored = false;
            
            if (spriteRenderer.sprite == null)
            {
                spriteRenderer.sprite = sprite1;
            }
        }

        private void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");

            if (horizontal < 0 && !facingRight)
            {
                ReverseSprite();
            }
            else if (horizontal > 0 && facingRight)
            {
                ReverseSprite();
            }
            
            var newVelocity = Vector2.zero;

            if (Input.GetKey(KeyCode.A))
            {
                newVelocity += Vector2.left*5;
            }
            if (Input.GetKey(KeyCode.D))
            {
                newVelocity += Vector2.right*5;
            }
            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                newVelocity += Vector2.up * 5;      
            }
            if(Input.GetKey(KeyCode.Space) && isOnWall)
            {
                newVelocity += new Vector2(direction * wallJumpVector.x, wallJumpVector.y);                
            }

            Velocity = newVelocity;
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChangeSprite();
            }
        }

        private void FixedUpdate()
        {
            ResetValuesBeforeSimulation();

            AddGravityToVelocity();
            AddTargetVelocityToVelocity();

            var deltaPosition = GetVelocityDeltaPosition();
            var groundMovementVector = GetGroundMovementVector();
            var horizontalDeltaPosition = groundMovementVector * deltaPosition.x;
            var verticalDeltaPosition = Vector2.up * deltaPosition.y;

            ApplyDeltaPosition(horizontalDeltaPosition, false);
            ApplyDeltaPosition(verticalDeltaPosition, true);

            latestVelocity = velocity;

#if UNITY_EDITOR
            if (showDebugInformation)
            {
                Debug.DrawLine(rigidbody.position, rigidbody.position + velocity, Color.red);
            }
#endif
        }

        private void ResetValuesBeforeSimulation()
        {
            isGrounded = false;
            isOnWall = false;
            contactFilter.layerMask = Physics2D.GetLayerCollisionMask(rigidbody.gameObject.layer);
        }

        private void AddGravityToVelocity()
        {
            velocity += GetGravityDeltaPosition();
        }

        private void AddTargetVelocityToVelocity()
        {
            //X velocity is entirely controlled by the object (like the player or an ennemy)
            velocity.x = targetVelocity.x;
            //Y velocity is controlled by the object when it's target y velocity is greater than 0 or if gravity is ignored.
            //Otherwise, current velocity is used.
            velocity.y = targetVelocity.y > 0 || IsGravityIgnored ? targetVelocity.y : velocity.y;
        }

        private Vector2 GetGravityDeltaPosition()
        {
            return gravity * (IsGravityIgnored ? 0f : gravityMultiplier) * Time.fixedDeltaTime;
        }

        private Vector2 GetVelocityDeltaPosition()
        {
            return velocity * Time.fixedDeltaTime;
        }

        private Vector2 GetGroundMovementVector()
        {
            return new Vector2(groundNormal.y, -groundNormal.x);
        }

        private void ApplyDeltaPosition(Vector2 deltaPosition, bool isVerticalDelta)
        {
            var deltaMagnitude = deltaPosition.magnitude;

            if (deltaMagnitude > sleepVelocity)
            {
                var nbCollidersDetected = rigidbody.Cast(deltaPosition,
                                                         contactFilter,
                                                         preallocaRaycastHits,
                                                         deltaMagnitude + deltaPrecision);

                for (int i = 0; i < nbCollidersDetected; i++)
                {
                    var collider = preallocaRaycastHits[i];
                    var colliderNormal = collider.normal;

                    //If this a useable ground ?
                    if (colliderNormal.y > 1 - maxGroundSlopeAngleArctan)
                    {
                        isGrounded = true;
                        lastGroundedTime = Time.time;
                        if (isVerticalDelta)
                        {
                            groundNormal = colliderNormal;
                            colliderNormal.x = 0;
#if UNITY_EDITOR
                            if (showDebugInformation)
                            {
                                Debug.DrawLine(collider.point, collider.point + colliderNormal, Color.green);
                                Debug.DrawLine(collider.point, collider.point + GetGroundMovementVector(), Color.yellow);
                            }
#endif
                        }
                    }
                    else if(Mathf.Abs(colliderNormal.x)>1-maxWallSlopeAngleArctan)
                    {
                        if(colliderNormal.x<0)
                        {
                            direction = 1;
                        }
                        else if(colliderNormal.x>0)
                        {
                            direction = -1;
                        }
                        isOnWall = true;
                        Debug.Log("direction"+direction);
                        Debug.Log("directionX:" + wallJumpVector.x);
                    }

                    //How much this collider should affect the velocity. The more the velocity vector
                    //and the collider normal vector are opposed, the more the collider should absorb the velocity
                    //
                    //Using the Dot product, we know how much theses two vectors are opposed (if they are).
                    //Negative number means vectors are opposed.
                    var velocityNegationForce = Vector2.Dot(velocity, colliderNormal);
                    if (velocityNegationForce < 0)
                    {
                        velocity -= velocityNegationForce * colliderNormal;
                    }

                    //Snap object to collider bound if distance between the object and the collider is less than
                    //the delta precison. This prevent the object from going though the collider.
                    var snappedDeltaMagnitude = collider.distance - deltaPrecision;
                    deltaMagnitude = snappedDeltaMagnitude < deltaMagnitude ? snappedDeltaMagnitude : deltaMagnitude;
                }
            }

            rigidbody.position += deltaPosition.normalized * deltaMagnitude;
        }
        
        private void ChangeSprite()
        {
            if (spriteRenderer.sprite == sprite1)
            {
                spriteRenderer.sprite = sprite2;
            }
            else
            {
                spriteRenderer.sprite = sprite1;
            }
        }

        private void ReverseSprite()
        {
            facingRight = !facingRight;

            Vector2 theScale = rigidbody.transform.localScale;
            theScale.x *= -1;
            rigidbody.transform.localScale = theScale;
        }
    }
}