using UnityEngine;

namespace GameSample.Objects
{
    /// <summary>
    /// プレイヤー追従カメラクラス
    /// </summary>
    public class HumanCameraBehaviour : MonoBehaviour
    {
        /// <summary>
        /// プレイヤー
        /// </summary>
        [SerializeField] private HumanBehaviour _player;

        /// <summary>
        /// 移動可能なZ位置の範囲
        /// </summary>
        [SerializeField] private float _maxMovePosZ = 61f;
        [SerializeField] private float _minMovePosZ = -5f;

        private void Update()
        {
            // プレイヤーの後ろを追従させる
            const float offsetPosZ = 5f;

            var movePosZ = _player.transform.position.z - offsetPosZ;
            movePosZ = Mathf.Min(movePosZ, _maxMovePosZ);
            movePosZ = Mathf.Max(movePosZ, _minMovePosZ);

            var position = transform.position;
            position.z = movePosZ;
            transform.position = position;
        }
    }
}
