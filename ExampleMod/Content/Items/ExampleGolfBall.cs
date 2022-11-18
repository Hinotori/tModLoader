﻿using ExampleMod.Content.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.ModLoader;

namespace ExampleMod.Content.Items
{
	public class ExampleGolfBall : ModItem
	{
		public override void SetStaticDefaults() {
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
		}

		public override void SetDefaults() {
			// DefaultToGolfBall sets various properties common to golf balls. Hover over DefaultToGolfBall in Visual Studio to see the specific properties set.
			// ModContent.ProjectileType<ExampleGolfBallProjectile>() is the projectile that is placed on the golf tee.
			Item.DefaultToGolfBall(ModContent.ProjectileType<ExampleGolfBallProjectile>());
		}
	}
}
