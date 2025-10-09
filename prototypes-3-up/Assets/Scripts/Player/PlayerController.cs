using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Input")] 
        public InputActionReference move;
        public InputActionReference look;
        public InputActionReference attack;
        public InputActionReference dash;
        //input privates
        private PlayerInput _playerInput;

        [Header("Movement")] 
        [SerializeField] [Range(1f, 25f)] private  float speed;
        [SerializeField] [Range(0.01f, 0.5f)] private float smoothness = 0.1f;
        //movement privates
        private Vector2 _movementInput;
        private Vector2 _smoothMovementInput;
        private Vector2 _smoothVelocity;
        
        [Header("Look Settings")] 
        [SerializeField] private Transform orientation;
        [SerializeField] private LookStyles lookStyle;
        [SerializeField] [Range(0.01f, 1f)] private float turnSpeed;
        //look privates
        private Camera _camera;
        private Vector2 _lookInput;
        private Quaternion _targetRotation;

        [Header("Knockback Settings")] 
        public float knockbackIntensity;
        public float knockbackDuration;
        public float durationLoseControl;
        //knockback privates
        private Vector2 _currentKnockback = Vector2.zero;
        private Vector2 _knockbackDirection = Vector2.zero;
        private float _knockbackForce;
        private float _loseControlTimer;

        [Header("Dash Settings")] 
        [SerializeField] [Range(5, 30)] private float dashIntensity = 3f;
        [SerializeField] [Range(0.1f, 0.5f)] private float dashDuration = 0.3f;
        [SerializeField] [Range(0.2f, 1f)] private float dashCooldown = 0.4f;
        [SerializeField] [Range(0.1f, 0.5f)] private float invincibleDuration = 0.2f;
        [SerializeField] [Range(0.1f, 0.3f)] private float invincibleGracePeriod = 0.1f;
        [SerializeField] private bool dashLocked = false;
        //dash privates
        private bool _dashing;
        private float _dashTimer;
        private float _cooldownTimer;
        private Vector2 _currentDash;
        private float _dashForce;
        private float _invincibleTimer;
        private bool _justDashed;
    
        [Header("After Effect")]
        public DashAfterEffect dashAfterEffect;

        [Header("Combat")] 
        public PlayerDamage damage;
        public PlayerCombat combat;
        [SerializeField] private BoxCollider2D damageCollider;
        [SerializeField] private BoxCollider2D worldCollider;
        private Rigidbody2D _rigidbody;

        [Header("Holding/Pushing/Pulling")] 
        public PlayerHolding holding;

        //[Header("Dialogue")] 
        //public PlayerTalking talking;

        private void Start()
        {
            //get references
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();
        }

        private void Update()
        {
            if (GameManager.IsPaused) return;
        
            HandleAttack();
            HandleDash();
        }

        private void FixedUpdate()
        {
            if  (GameManager.IsPaused) return;
        
            HandleMovement();
            HandleLook();
        }

        //HANDLE MOVEMENT: handles movement input
        private void HandleMovement()
        {
            //if in dialogue
            // if (talking.runner.IsDialogueRunning)
            // {
            //     //do something
            // }
            
            //read value, depending on if in dash or not dash locked
            if ((!_dashing && dashLocked) || !dashLocked) _movementInput = move.action.ReadValue<Vector2>();
        
            //calc dash
            CalculateDash(_movementInput);
        
            //get smoothed movement from input
            _smoothMovementInput = Vector2.SmoothDamp(_smoothMovementInput, _movementInput, ref _smoothVelocity, smoothness);
        
            //calc knockback (zero out other contributors)
            CalculateKnockback();
            if (_loseControlTimer > 0)
            {
                _currentDash = Vector2.zero;
                _smoothMovementInput = Vector2.zero;
            }
        
            //modify speed according to weight
            float modifiedSpeed = speed - holding.GetHeldWeight();
            if (modifiedSpeed < 2) modifiedSpeed = 2;
            
            //apply all velocities to rigidbody
            _rigidbody.linearVelocity = _smoothMovementInput * modifiedSpeed + _currentDash + _currentKnockback;
        }

        #region Look
    
        //HANDLE LOOK: handles look for controller and m+k
        private void HandleLook()
        {
            //set orientation according to stick vector
            _lookInput =  look.action.ReadValue<Vector2>();

            if (holding.isDragging)
            {
                HandleLookingAt(holding.objectDragging.transform);
                return;
            }
        
            switch (lookStyle)
            {
                case LookStyles.Independent:
                    HandleIndependentLookStyle();
                    break;
                case LookStyles.Locked:
                    HandleLockedLookStyle();
                    break;
                case LookStyles.Contextual:
                    HandleContextualLookStyle();
                    break;
            }
        }

        private void HandleIndependentLookStyle()
        {
            //determine rotation, depending on input type
            if (UsingMouse())
            {
                //get mouse position
                Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                
                //get temp degree math
                float angleRad = Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x);
                float angleDeg = (180 / Mathf.PI) * angleRad - 90;
                
                //set target
                _targetRotation = Quaternion.Euler(0, 0, angleDeg);
            }
            else
            {
                if (_lookInput == Vector2.zero) return;
            
                //set target
                _targetRotation = Quaternion.LookRotation(Vector3.forward, _lookInput);
            }
            
            //rotate the orientation
            orientation.rotation = Quaternion.Lerp(orientation.rotation, _targetRotation, turnSpeed);
        }

        private void HandleLockedLookStyle()
        {
            if (_movementInput != Vector2.zero)
            {
                _targetRotation = Quaternion.LookRotation(Vector3.forward, _movementInput);
            }
            
            //rotate the orientation
            orientation.rotation = Quaternion.Lerp(orientation.rotation, _targetRotation, turnSpeed);
        }

        private void HandleContextualLookStyle()
        {
            if (_lookInput == Vector2.zero || UsingMouse()) HandleLockedLookStyle();
            else HandleIndependentLookStyle();
        }

        private void HandleLookingAt(Transform target)
        {
            //get temp degree math
            float angleRad = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x);
            float angleDeg = (180 / Mathf.PI) * angleRad - 90;
                
            //set target
            _targetRotation = Quaternion.Euler(0, 0, angleDeg);
            
            //rotate the orientation
            orientation.rotation = Quaternion.Lerp(orientation.rotation, _targetRotation, turnSpeed);
        }

        //USING MOUSE: return true if using mouse
        private bool UsingMouse()
        {
            if (_playerInput.currentControlScheme.Contains("Mouse"))
            {
                return true;
            }

            return false;
        }
    
        #endregion
    
        #region Attack

        //HANDLE ATTACK: trigger attack
        private void HandleAttack()
        {
            if (attack.action.triggered)
            {
                combat.Attack();
            
                //SFX_INSERT_HERE
                //ATTACK SOUND
            }
        }
    
        #endregion
    
        #region Dash
    
        //CALCULATE DASH: calculates the dash, is found inside the HandleMovement function
        private void CalculateDash(Vector2 input)
        {
            _currentDash = input * _dashForce;
        
            _dashForce -= dashIntensity/dashDuration * Time.deltaTime;
            if (_dashForce < 0)
            {
                _dashing = false;
                _dashForce = 0;
            }
        
            //set cooldown timer
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer < 0) _cooldownTimer = 0;
        
            //set i frame timer
            _invincibleTimer -= Time.deltaTime;
            if (_invincibleTimer < 0) _invincibleTimer = 0;
        
            //enable or disable collider for invincibility
            _justDashed = false;
            if (_invincibleTimer == 0 && !damageCollider.enabled)
            {
                _justDashed = true;
                damageCollider.enabled = true;
            }
        }

        //HANDLE DASH: handle the dash input
        private void HandleDash()
        {
            if (_movementInput == Vector2.zero) return;
        
            if (dash.action.triggered && _cooldownTimer == 0)
            {
                //set force, duration, and cooldown
                _dashing = true;
                _dashForce = dashIntensity;
                _cooldownTimer = dashCooldown;
            
                //set invincible
                _invincibleTimer = invincibleDuration;
                damageCollider.enabled = false;
            
                //trigger after effect
                dashAfterEffect.AfterEffect();
            
                //SFX_INSERT_HERE
                //DASH SOUND
            }
        }
    
        #endregion
    
        #region Knock Back

        private void Knockback(Transform collisionTransform)
        {
            //get knockback direction and normalize
            _knockbackDirection = transform.position - collisionTransform.position;
            _knockbackDirection.Normalize();
        
            //set knockback 
            _knockbackForce = knockbackIntensity;
        
            //start lose control timer
            _loseControlTimer = durationLoseControl;
        }
    
        //CALCULATE KNOCKBACK: calculates knockback, decreasing it over time
        private void CalculateKnockback()
        {
            _currentKnockback = _knockbackDirection * _knockbackForce;
        
            //reduce knockback force to 0
            _knockbackForce -= knockbackIntensity/knockbackDuration * Time.deltaTime;
            if (_knockbackForce < 0) _knockbackForce = 0;
        
            //set loss control timer
            _loseControlTimer -= Time.deltaTime;
            if (_loseControlTimer < 0) _loseControlTimer = 0;
        }
    
        #endregion
    
        #region Collisions
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_justDashed)
            {
                _justDashed = false;
                _invincibleTimer = invincibleGracePeriod;
                damageCollider.enabled = false;
                return;
            }
        
            if (collision.gameObject.CompareTag("Enemy"))
            {
                Knockback(collision.transform);
                damage.Damage();
            }
        }
    
        #endregion
    }

    public enum LookStyles
    {
        Locked,
        Independent,
        Contextual
    }
}