using System;
using UnityEngine;

namespace GameSample.Objects
{
    public class HumanBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 移動速度
        /// </summary>
        [SerializeField] private float _moveSpeed = 0.01f;

        /// <summary>
        /// Components
        /// </summary>
        private Rigidbody _rigidbody;
        private Animator _animator;

        /// <summary>
        /// 移動の準備ができた？
        /// </summary>
        [NonSerialized] public bool IsReadyMove;

        /// <summary>
        /// 移動した？
        /// </summary>
        private bool _isMoved = false;
        public bool IsMoved => _isMoved;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _animator = gameObject.GetComponent<Animator>();
            IsReadyMove = false;
            _isMoved = false;
        }

        private void Update()
        {
            if (!IsReadyMove)
            {
                return;
            }

            // 移動と向き変更
            var moveVelocity = GetInputMoveVelocity();
            if (moveVelocity.magnitude < Mathf.Epsilon)
            {
                _rigidbody.velocity = Vector3.zero;
                SetAnimationMoveVelocity(0.0f);
                return;
            }

            // 位置を移動させるだけの手抜き...
            var targetPosition = transform.position;
            targetPosition += Vector3.right * moveVelocity.x * _moveSpeed;
            targetPosition += Vector3.forward * moveVelocity.y * _moveSpeed;
            transform.LookAt(targetPosition);
            transform.position = targetPosition;
            SetAnimationMoveVelocity(moveVelocity.magnitude * 10);
            _isMoved = true;
        }

        /// <summary>
        /// 入力された移動方向を取得
        /// </summary>
        /// <returns></returns>
        private Vector2 GetInputMoveVelocity()
        {
            var moveVelocity = Vector2.zero;
            if (UnityEngine.Input.GetKey(KeyCode.A)) moveVelocity.x -= 1.0f;
            if (UnityEngine.Input.GetKey(KeyCode.D)) moveVelocity.x += 1.0f;
            if (UnityEngine.Input.GetKey(KeyCode.S)) moveVelocity.y -= 1.0f;
            if (UnityEngine.Input.GetKey(KeyCode.W)) moveVelocity.y += 1.0f;
            return moveVelocity;
        }

        /// <summary>
        /// アニメーションパラメータ
        /// </summary>
        private static readonly string AnimParamVelocity = "VelocityFloat";
        private void SetAnimationMoveVelocity(float velocity)
        {
            _animator.SetFloat(AnimParamVelocity, velocity);
        }
    }
}
