using System;
using R3;

namespace Game.Runtime.ReduxUtility
{
    internal sealed class Store<T>
    {
        private readonly ReactiveProperty<T> _state;
        private readonly Func<T, object, T> _reducer;
        
        public ReadOnlyReactiveProperty<T> State { get; }
        
        public Store(T initialState, Func<T, object, T> reducer)
        {
            _state = new ReactiveProperty<T>(initialState);
            _reducer = reducer;
            State = _state.ToReadOnlyReactiveProperty();
        }
        
        public void Dispatch(object action)
        {
            _state.Value = _reducer(_state.Value, action);
        }
    }
}