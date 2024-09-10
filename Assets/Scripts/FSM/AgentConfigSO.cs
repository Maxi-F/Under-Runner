using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    [CreateAssetMenu(fileName = "AgentConfig", menuName = "FSM/AgentConfig", order = 0)]
    public class AgentConfigSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}