using System.Collections;

namespace Enemy.Attacks
{
    public interface IEnemyAttack
    {
        public bool CanExecute();
        public IEnumerator Execute();
    }
}
