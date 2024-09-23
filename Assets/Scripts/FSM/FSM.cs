using System.Collections;
using System.Collections.Generic;

namespace FSM
{
    public class FSM
    {
        private List<State> _states;
        private State _currentStateSo;
        
        public FSM(List<State> states)
        {
            _states = states;
            _currentStateSo = states[0];
            _currentStateSo.Enter();
        }
        
        public void Update()
        {
            _currentStateSo.Update();
        }

        public void ChangeState(State to)
        {
            if (_currentStateSo == to)
            {
                _currentStateSo.Exit();
                _currentStateSo.Enter();
            }  else if (_currentStateSo.TryGetTransition(to, out Transition transition))
            {
                _currentStateSo = to;
                transition.Do();
            }
        }
    }
}