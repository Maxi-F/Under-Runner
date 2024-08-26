using System.Collections.Generic;

namespace _Dev.UnderRunnerTest.Scripts.FSM
{
    public class FSM
    {
        private List<StateSO> states;
        private StateSO _currentStateSo;

        public FSM(List<StateSO> states)
        {
            this.states = states;
            _currentStateSo = states[0];
            _currentStateSo.Enter();
        }

        public void Update()
        {
            _currentStateSo.Update();
        }

        public void ChangeState(StateSO to)
        {
            if (_currentStateSo == to)
            {
                _currentStateSo.Exit();
                _currentStateSo.Enter();
                return;
            }

            if (_currentStateSo.TryGetTransition(to, out TransitionSO transition))
            {
                _currentStateSo = to;
                transition.Do();
            }
        }
        
        public StateSO FindState<T>() where T : StateSO
        {
            foreach (StateSO state in states)
            {
                if (state as T != null)
                {
                    return state;
                }
            }

            return null;
        }
    }
}