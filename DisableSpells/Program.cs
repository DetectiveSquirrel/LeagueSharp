using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using LeagueSharp.Common;
using LeagueSharp;

namespace DisableSpells
{
    internal class Program
    {
        public struct SpellStruct
        {
            public string ChampionName;
            public SpellSlot AvailableSpell;

        }
        public static List<SpellStruct> Spells = new List<SpellStruct>();
        public static Menu Config;

        private static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {

            /*
             * Ashe:
             * -> Fiora: OnAttack: Instant ultimate / no duration limit / less damage / can be attacked
             * -> Twitch: OnAttack: Cast's W without CD except of AA
             * -> TwistedFate: OnAttack: Always shoots with red card
             * -> Ezreal: OnAttack: E particle, ways less damage, ways less attackspeed
             * -> Lucian: OnAttack: R particle, goes throguh enemys, ways less damage, ways less attackspeed
             * -> Brand: OnAttack: Ultimate
             * -> Pantheon: Weird shit.
             * -> Gragas: OnAttack: Ultimate with a cd of 10-15sec
             * -> Varus: Uses the area Damage on attack
             * -> Jax: Possible to stun everyone
             * -> Lulu: OnAttack: Lulu AA becomes her Q and Pix also CS
             */

            Config = new Menu("Exploit", "Exploit", true);
            Config.AddSubMenu(new Menu("Disable", "Disable"));
            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => !hero.IsMe))
            {
                Config.SubMenu("Disable")
                    .AddItem(
                        new MenuItem(hero.ChampionName, "Disable on " + hero.ChampionName).SetValue(false));
                Config.Item(hero.ChampionName).SetValue(false);
            }

            Config.AddSubMenu(new Menu("Spells", "Spells"));
            Config.SubMenu("Spells")
                .AddItem(new MenuItem("qqSpell", "Q").SetValue(false));
            Config.SubMenu("Spells")
                .AddItem(new MenuItem("wwSpell", "W").SetValue(false));
            Config.SubMenu("Spells")
                .AddItem(new MenuItem("eeSpell", "E").SetValue(false));
            Config.SubMenu("Spells")
                .AddItem(new MenuItem("rrSpell", "R").SetValue(false));
            Config.AddToMainMenu();

            Game.PrintChat("Exploit loaded!");
            Game.OnGameUpdate += Game_OnGameUpdate;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            foreach (var hero in from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => !hero.IsMe)
                let isEnabled = Config.Item(hero.ChampionName).GetValue<bool>()
                let championName = Config.Item(hero.ChampionName).Name
                where hero.ChampionName == championName & isEnabled && !hero.IsDead
                select hero)
            {
                if (Config.Item("qqSpell").GetValue<bool>())
                {
                    Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(hero.NetworkId, SpellSlot.Q)).Send();
                }

                if (Config.Item("wwSpell").GetValue<bool>())
                {
                    Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(hero.NetworkId, SpellSlot.W)).Send();
                }

                if (Config.Item("eeSpell").GetValue<bool>())
                {
                    Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(hero.NetworkId, SpellSlot.E)).Send();
                }

                if (Config.Item("rrSpell").GetValue<bool>())
                {
                    Packet.C2S.Cast.Encoded(new Packet.C2S.Cast.Struct(hero.NetworkId, SpellSlot.R)).Send();
                }
            }
        }
    }
}
