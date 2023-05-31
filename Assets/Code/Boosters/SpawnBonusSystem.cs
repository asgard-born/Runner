using System.Collections.Generic;
using System.Linq;
using Code.Boosters.Abstract;
using Code.Configs;
using Code.Configs.Essences;
using Code.ObjectsPool;
using Code.Platforms.Concrete;
using UnityEngine;

namespace Code.Boosters
{
    public class SpawnBonusSystem
    {
        private readonly Ctx _ctx;
        private List<Booster> _bonuses = new List<Booster>(128);

        public struct Ctx
        {
            public LevelConfigs levelConfigs;
        }

        public SpawnBonusSystem(Ctx ctx)
        {
            _ctx = ctx;
        }

        public void TrySpawnBonus(StandardPlatform platform)
        {
            var chances = _ctx.levelConfigs.bonusChances.OrderBy(bonusChance => bonusChance.chance);
            var sum = chances.Sum(x => x.chance);
            var randomNumberToChooseBonusType = Random.Range(0, sum);
            var currentSumm = 0f;

            foreach (var bonusChance in chances)
            {
                currentSumm += bonusChance.chance;

                if (randomNumberToChooseBonusType <= currentSumm)
                {
                    BonusChance bonus = bonusChance;

                    var randomNumberToChooseIfCanSpawn = Random.Range(0, 1f);

                    if (bonus.chance >= randomNumberToChooseIfCanSpawn)
                    {
                        SpawnBonus(bonus._booster, platform.bonusSpawnPoint.position);
                        return;
                    }
                }
            }
        }

        private void SpawnBonus(Booster booster, Vector3 position)
        {
            var type = booster.GetType();
            var newBonus = (Booster)PoolsManager.GetObject(type);
            newBonus.transform.position = position;
            _bonuses.Add(newBonus);
        }
    }
}