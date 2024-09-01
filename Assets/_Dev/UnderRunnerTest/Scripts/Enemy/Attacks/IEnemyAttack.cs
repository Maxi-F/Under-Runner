namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public interface IEnemyAttack
    {
        public bool CanExecute();
        public void Execute();
        public bool IsExecuting();
    }
}