using ExampleMod.Content.Dusts;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ExampleMod.Content.Projectiles
{
	// This example shows how to implement simple homing projectile
	// Can be tested with ExampleCustomAmmoGun
	public class ExampleHomingProjectile : ModProjectile
	{
		public override void SetStaticDefaults() {
			DisplayName.SetDefault("Example Homing Projectile"); // Name of the projectile that appears in chat

			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
		}

		// Setting the default parameters of the projectile
		// You can check most of the fields and properties at https://github.com/tModLoader/tModLoader/wiki/Projectile-Class-Documentation
		public override void SetDefaults() {
			Projectile.width = 8; // Width of the projectile's hitbox
			Projectile.height = 8; // Height of the projectile hitbox

			Projectile.aiStyle = 0; // The AI style of the projectile (0 means custom AI). For more please reference the source code of Terraria
			Projectile.DamageType = DamageClass.Ranged; // The type of damage this projectile inflicts
			Projectile.friendly = true; // Can the projectile deal damage to enemies?
			Projectile.hostile = false; // Can the projectile deal damage to the player?
			Projectile.ignoreWater = true; // Is the projectile's speed influenced by water?
			Projectile.light = 1f; // How much light the projectile emits
			Projectile.tileCollide = false; // Can the projectile collide with tiles?
			Projectile.timeLeft = 600; // The projectile's lifetime (60 = 1 second, so 600 is 10 seconds)
		}

		// Custom AI
		public override void AI() {
			float maxDetectRadius = 400f; // The maximum radius at which the projectile can detect a target
			float projSpeed = 5f; // The speed at which the projectile moves towards the target

			// Try to find NPC closest to the projectile
			NPC closestNPC = FindClosestNPC(maxDetectRadius);
			if (closestNPC == null)
				return;

			// If found, change the velocity of the projectile and turn it in the direction of the target
			// Use the SafeNormalize extension method to avoid NaNs returned by Vector2.Normalize when the vector is zero
			Projectile.velocity = (closestNPC.Center - Projectile.Center).SafeNormalize(Vector2.Zero) * projSpeed;
			Projectile.rotation = Projectile.velocity.ToRotation();
		}

		// Find the closest NPC to attack within maxDetectDistance range
		// If none found, return null
		public NPC FindClosestNPC(float maxDetectDistance) {
			NPC closestNPC = null;

			// Using squared values in distance checks will let us skip expensive square root calculations, drastically increasing this method's speed.
			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			// Loop through all NPCs(max always 200)
			for (int k = 0; k < Main.maxNPCs; k++) {
				NPC target = Main.npc[k];
				// Check if the NPC is able to be targeted, meaning they are:
				// 1. Active (alive)
				// 2. Chaseable (e.g. not a cultist archer)
				// 3. Max life bigger than 5 (e.g. not a critter)
				// 4. Can take damage (e.g. moon lord core after all its parts are downed)
				// 5. Hostile (!friendly)
				// 6. Not immortal (e.g. not a target dummy)
				if (target.CanBeChasedBy()) {
					// The DistanceSquared function returns a squared distance between 2 points, skipping relatively expensive square root calculations
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					// Check if it is within the radius
					if (sqrDistanceToTarget < sqrMaxDetectDistance) {
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}

			return closestNPC;
		}
	}
}
