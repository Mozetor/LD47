using Assets.Enemies;
using City;
using Economy;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayerBuilding.Tower {
    public class Tower : Building {

        private const int GROUNDED = 8;
        private const int AIRBORNE = 9;
        /// <summary> Contains tower damage, range and cooldown </summary>
        public TowerDamageData[] towerDamageData;
        /// <summary> Enemys </summary>
        public List<EnemyType> targets;
        /// <summary> Projectile type </summary>
        public TowerProjectile projectile;
        /// <summary> Tower head </summary>
        public Transform turretHead;

        private float currentAttackCooldown = 0;

        private void Awake() {
            if (buildCost.Length != towerDamageData.Length) {
                throw new System.ArgumentException("Upgrade arrays must have same lenght!");
            }
            if (upkeepCost.Length != 0) {
                if (upkeepCost.Length != buildCost.Length) {
                    throw new System.ArgumentException("Upgrade arrays must have same lenght!");
                }
            }
        }

        private void Update() {
            // Can improve performance
            if (currentAttackCooldown > 0) {
                currentAttackCooldown -= Time.deltaTime;
                return;
            }

            var enemies = FindObjectsOfType<Enemy>()
                .Where(HasValidType)
                .Where(IsInRange)
                .ToList();

            if (enemies.Count == 0)
                return;

            var target = enemies[0];
            StartCoroutine(Attack(target));
        }

        private bool HasValidType(Enemy enemy) => targets.Contains(enemy.enemyType);

        private bool IsInRange(Enemy enemy) => Vector3.Distance(enemy.transform.position, this.transform.position) <= towerDamageData[buildingLevel].range;

        private Vector3 GetPositionInFrontOfChildWithName(Enemy enemy, string name) {
            for (int i = 0; i < enemy.transform.childCount; i++) {
                if (enemy.transform.GetChild(i).name == name) {
                    var tran = enemy.transform.GetChild(i);
                    return tran.position + 0.5f * tran.up;
                }
            }
            return enemy.transform.position;
        }

        private IEnumerator Attack(Enemy target) {
            currentAttackCooldown = towerDamageData[buildingLevel].attackCooldown;
            var forward = GetPositionInFrontOfChildWithName(target, "Graphics") - this.transform.position;

            var startQuat = this.turretHead.rotation;
            var targetQuat = Quaternion.FromToRotation(Vector3.down, forward);

            for (int i = 0; i < 10; i++) {
                this.turretHead.rotation = Quaternion.Lerp(startQuat, targetQuat, (i + 1) / 9f);
                yield return new WaitForSeconds(0.001f);
            }

            var lifetime = 1.5f * towerDamageData[buildingLevel].range / projectile.speed;

            var proj = Instantiate(projectile, this.transform.position, Quaternion.identity);
            proj.direction = forward;
            proj.damage = towerDamageData[buildingLevel].damage;
            proj.lifeTime = lifetime;
            proj.enemyTypes = this.targets;
            proj.gameObject.layer = targets.Contains(EnemyType.GROUNDED) ? GROUNDED : AIRBORNE;
        }

        public override void FinishPlacement(GameObject placedObject) {
            PlayerBuildingPlacer.AddBuilding(placedObject);
            if (upkeepCost.Length != 0) {
                for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                    FindObjectOfType<CityController>().StrainBalance(upkeepCost[buildingLevel].BalanceCost[i]);
                }
            }
        }

        public override void PrepareRemoval() {
            if (upkeepCost.Length != 0) {
                for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                    BalanceResource bal = upkeepCost[buildingLevel].BalanceCost[i];
                    bal.resourceAmount = -bal.resourceAmount;
                    FindObjectOfType<CityController>().StrainBalance(bal);
                }
            }
        }

        public override void Upgrade() {
            if (buildingLevel + 1 >= buildCost.Length) {
                throw new System.ArgumentException("Tried to upgrade past maximum building level");
            }
            // change spirte
            CityController city = FindObjectOfType<CityController>();
            city.Buy(buildCost[buildingLevel + 1].ResourceCost);
            if (upkeepCost.Length != 0) {
                for (int i = 0; i < upkeepCost[buildingLevel].BalanceCost.Length; i++) {
                    BalanceResource bal = upkeepCost[buildingLevel].BalanceCost[i];
                    bal.resourceAmount = -bal.resourceAmount;
                    FindObjectOfType<CityController>().StrainBalance(bal);
                }
            }
            buildingLevel++;
            if (upkeepCost.Length != 0) {
                FindObjectOfType<CityController>().StrainBalance(upkeepCost[buildingLevel].BalanceCost);
            }
        }
    }
}