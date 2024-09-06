namespace Enemy.Attacks
{
    public interface IEnemyAttack
    {
        public bool CanExecute();
        public void Execute();
        public bool IsExecuting();
    }
}
