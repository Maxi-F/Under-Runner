namespace _Dev.UnderRunnerTest.Scripts.Health
{
    public interface ITakeDamage
    {
        /// <summary>
        /// Makes the object/entity take damage. Implemented internally
        /// by each entity.
        /// </summary>
        public void TakeDamage(int quantity);
    }
}
