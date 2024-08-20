using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.ParryProjectile
{
    public interface IDeflectable
    {
        /// <summary>
        /// Makes the object/entity get deflected changing it's target. Implemented internally
        /// by each entity.
        /// </summary>
        public void Deflect();
    }
}
