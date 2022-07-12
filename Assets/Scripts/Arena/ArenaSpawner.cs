using System.Collections.Generic;

namespace Assets.Scripts.Arena
{
    public class ArenaSpawner : EnemyPool
    {
        public List<ArenaWave> Waves;
        public int RemainingEnemies;
        public int WaveNum = 0;
        
        void Start()
        {
            OnEnemyDestroyed += ReduceEnemies;
            Create();
            SpawnWave();
        }

        private void SpawnWave()
        {
            RemainingEnemies = Waves[WaveNum].Enemies.Count;

            foreach (ArenaEnemy enemy in Waves[WaveNum].Enemies)
            {
                EnemyPoolItem item = Take(enemy.EnemyName);
                item.Spawn(enemy.SpotPoint);
            }
        }

        private void ReduceEnemies()
        {
            RemainingEnemies--;

            if (RemainingEnemies == 0)
            {
                WaveNum = (WaveNum == Waves.Count - 1) ? 0 : WaveNum + 1;
                SpawnWave();
            }
        }
    }
}