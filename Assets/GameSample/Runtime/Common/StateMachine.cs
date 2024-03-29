using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSample.Common
{
    /// <summary>
    /// ステートマシンクラス
    /// </summary>
    /// <typeparam name="TOwner"></typeparam>
    public class StateMachine<TOwner>
    {
        /// <summary>
        /// ステート基底クラス
        /// 各ステートクラスはこのクラスを継承する
        /// </summary>
        public abstract class StateBase
        {
            public StateMachine<TOwner> StateMachine;
            protected TOwner Owner => StateMachine.Owner;
            public int StateId;

            public virtual void OnStart() { }
            public virtual void OnUpdate() { }
            public virtual void OnEnd() { }
        }
        private TOwner Owner { get; }

        /// <summary>
        /// 現在のステート
        /// </summary>
        private StateBase _currentState;
        private StateBase CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                OnChangedState?.Invoke(value.StateId); // ステート変更時のイベントを呼ぶ
            }
        }

        /// <summary>
        /// 前のステート
        /// </summary>
        private StateBase _prevState;
        private readonly Dictionary<int, StateBase> _states = new Dictionary<int, StateBase>(); // 全てのステート定義

        /// <summary>
        /// ステート変更時のイベント
        /// </summary>
        private event Action<int> OnChangedState;
        public void SetChangeStateEvent(Action<int> action)
        {
            OnChangedState = action;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="owner">StateMachineを使用するOwner</param>
        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }

        /// <summary>
        /// ステート定義登録
        /// ステートマシン初期化後にこのメソッドを呼ぶ
        /// </summary>
        /// <param name="stateId">ステートID</param>
        /// <typeparam name="T">ステート型</typeparam>
        public void Add<T>(int stateId) where T : StateBase, new()
        {
            if (_states.ContainsKey(stateId))
            {
                Debug.LogError($"already register stateId => {stateId}");
                return;
            }
            // ステート定義を登録
            var newState = new T
            {
                StateMachine = this,
                StateId = stateId,
            };
            _states.Add(stateId, newState);
        }

        /// <summary>
        /// ステート開始処理
        /// </summary>
        /// <param name="stateId">ステートID</param>
        public void OnStart(int stateId)
        {
            if (!_states.TryGetValue(stateId, out var nextState))
            {
                Debug.LogError($"not set stateId => {stateId}");
                return;
            }
            // 現在のステートに設定して処理を開始
            CurrentState = nextState;
            CurrentState.OnStart();
        }

        /// <summary>
        /// ステート更新処理
        /// </summary>
        public void OnUpdate()
        {
            CurrentState.OnUpdate();
        }

        /// <summary>
        /// 次のステートに切り替える
        /// </summary>
        /// <param name="stateId">切り替えるステートID</param>
        public void ChangeState(int stateId)
        {
            if (!_states.TryGetValue(stateId, out var nextState))
            {
                Debug.LogError($"not set stateId => {stateId}");
                return;
            }
            // 前のステートを保持
            _prevState = CurrentState;
            // ステートを切り替える
            CurrentState.OnEnd();
            CurrentState = nextState;
            CurrentState.OnStart();
        }

        /// <summary>
        /// 前回のステートに切り替える
        /// </summary>
        public void ChangePrevState()
        {
            if (_prevState == null)
            {
                Debug.LogError("prevState is null");
                return;
            }
            // 前のステートと現在のステートを入れ替える
            (_prevState, CurrentState) = (CurrentState, _prevState);
        }

        /// <summary>
        /// 指定IDが現在のステートか？
        /// </summary>
        /// <param name="stateId">ステートID</param>
        public bool IsCurrentState(int stateId)
        {
            if (!_states.TryGetValue(stateId, out var state))
            {
                return false;
            }
            return CurrentState == state;
        }
    }
}