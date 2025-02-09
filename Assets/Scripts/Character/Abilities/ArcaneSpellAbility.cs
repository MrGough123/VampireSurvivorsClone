using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class ArcaneSpellAbility : ProjectileAbility
    {
        [Header("Arcane Spell Stats")]
        [SerializeField] protected UpgradeableProjectileCount projectileCount;
        [SerializeField] protected float spellDelay;
        [SerializeField] protected float targetRadius = 5;

        protected override void Attack()
        {
            StartCoroutine(LaunchArcaneSpells());
        }

        protected IEnumerator LaunchArcaneSpells()
        {
            timeSinceLastAttack -= projectileCount.Value * spellDelay;
            for (int i = 0; i < projectileCount.Value; i++)
            {
                LaunchProjectileAtNearestEnemy();
                yield return new WaitForSeconds(spellDelay);
            }
        }

        protected void LaunchProjectileAtNearestEnemy()
        {
            ISpatialHashGridClient targetEntity = entityManager.Grid.FindClosestInRadius(playerCharacter.CenterTransform.position, targetRadius);
            Vector2 launchDirection = targetEntity == null ? Random.insideUnitCircle.normalized : (targetEntity.Position - (Vector2)playerCharacter.CenterTransform.position).normalized;

            Projectile projectile = entityManager.SpawnProjectile(projectileIndex, playerCharacter.CenterTransform.position, damage.Value, knockback.Value, speed.Value, monsterLayer);
            projectile.OnHitDamageable.AddListener(playerCharacter.OnDealDamage.Invoke);
            projectile.Launch(launchDirection);
        }
    }
}
