using System;
using TSS.Extras.AI.StateMachines;
using UnityEngine;

namespace TSS.Extras.PlayerControl.Third3D
{
    public class Third3DController : MonoBehaviour
    {
        public Rigidbody Rigidbody => _rigidbody;
        public Animator Animator => _animator;
        
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Third3DComponent[] _components;

        private StateMachine _stateMachine;
        private void Awake()
        {
            
        }
    }
}