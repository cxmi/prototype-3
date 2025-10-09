using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerCombat : MonoBehaviour
    {
        [SerializeField] private Transform orientation;
        [SerializeField] private PlayerOrientation playerOrientation;

        [Header("Attack Settings")] 
        [SerializeField] private List<GameObject> attacks;
        private int _attackIndex;
    
        [Header("Timing Settings")] 
        [SerializeField] private float attackDuration;
        [SerializeField] private float gracePeriod;
        private float _attackTimer;
        private float _graceTimer;
        private bool _idle;
        private bool _attacking;

        private void Start()
        {
            DisableAllAttacks();
        }

        private void Update()
        {
            _graceTimer -= Time.deltaTime;
            if (_graceTimer < 0) _graceTimer = 0;
            if (_graceTimer == 0 && !_idle)
            {
                _attackIndex = 0;
                _idle = true;
            }
        
            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0) _attackTimer = 0;
            if (_attackTimer == 0 && _attacking)
            {
                _attacking = false;
                DisableAllAttacks();
            }
        }

        public void Attack()
        {
            //set idle false
            _idle = false;
            _attacking = true;
     
            //disable all attacks
            DisableAllAttacks();

            //trigger current attack
            GameObject currentAttack = attacks[_attackIndex];
            currentAttack.transform.rotation = orientation.rotation * Quaternion.Euler(0,0,90);
            currentAttack.SetActive(true);
        
        
            //start attack and grace period timers
            _attackTimer = attackDuration;
            _graceTimer = gracePeriod;

            _attackIndex++;
            if (_attackIndex >= attacks.Count) _attackIndex = 0;
        }

        private void DisableAllAttacks()
        {
            foreach (GameObject attack in attacks)
            {
                attack.SetActive(false);
            }
        }
    
        private PlayerOrientation GetPlayerOrientation()
        {
            //get rotation from orientation object
            float currentRotation = orientation.rotation.eulerAngles.z;
        
            //get orientation and return
            if (currentRotation < 45 || currentRotation > 315) return PlayerOrientation.Up;
            if (currentRotation > 45 && currentRotation < 135) return PlayerOrientation.Left;
            if (currentRotation <= 315 && currentRotation > 225) return PlayerOrientation.Right;
            return PlayerOrientation.Down;
        }
    }

    public enum PlayerOrientation
    {
        Up,
        Down,
        Left,
        Right
    }
}