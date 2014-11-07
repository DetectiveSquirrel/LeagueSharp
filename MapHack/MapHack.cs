using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

/*
    Copyright (C) 2014 Nikita Bernthaler

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace MapHack
{
    internal class MapHack
    {
        public static Menu Config;
        private readonly List<Hero> _heroes = new List<Hero>();

        private readonly Action _onLoadAction;

        public MapHack()
        {
            _onLoadAction = new CallOnce().A(OnLoad);
            Game.OnGameUpdate += OnGameUpdate;
        }

        private void OnLoad()
        {
            Drawing.OnDraw += OnDraw;
            Game.PrintChat(
                string.Format(
                    "<font color='#F7A100'>{0} v{1} loaded.</font>",
                    Assembly.GetExecutingAssembly().GetName().Name,
                    Assembly.GetExecutingAssembly().GetName().Version
                    )
                );
            (Config = new Menu("MapHack", "Map Hack", true)).AddToMainMenu();
            Config.AddItem(new MenuItem("TextColorMH", "Text Color").SetValue(new Circle(true, Color.FromArgb(255, 255, 255, 0))));
            Config.AddItem(new MenuItem("OutlineColorMH", "Outline Color").SetValue(new Circle(true, Color.FromArgb(255, 0, 0, 0))));
        }

        private void OnGameUpdate(EventArgs args)
        {
            try
            {
                _onLoadAction();
                foreach (
                    Obj_AI_Hero hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValid && hero.IsEnemy))
                {
                    if (_heroes.All(t => t.Name != hero.BaseSkinName))
                    {
                        _heroes.Add(new Hero
                        {
                            Name = hero.BaseSkinName,
                            Visible = true,
                            Dead = hero.IsDead,
                            LastPosition = hero.Position
                        });
                    }
                    Hero h = _heroes.FirstOrDefault(heroes => heroes.Name == hero.BaseSkinName);
                    if (h != null)
                    {
                        h.Visible = hero.IsVisible;
                        h.Dead = hero.IsDead;
                        h.LastPosition = hero.IsVisible ? hero.Position : h.LastPosition;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void OnDraw(EventArgs args)
        {
            try
            {
                foreach (Hero hero in _heroes)
                {
                    if (!hero.Dead && !hero.Visible)
                    {
                        Vector2 pos = Drawing.WorldToMinimap(hero.LastPosition);

                        var OutlineColor = Config.Item("OutlineColorMH").GetValue<Circle>();
                        var TextColor = Config.Item("TextColorMH").GetValue<Circle>();

                        Drawing.DrawText(pos.X - Convert.ToInt32(hero.Name.Substring(0, 4).Length*5 - 1), pos.Y - 6, OutlineColor.Color, hero.Name.Substring(0, 3));
                        Drawing.DrawText(pos.X - Convert.ToInt32(hero.Name.Substring(0, 4).Length*5 + 1), pos.Y - 8, OutlineColor.Color, hero.Name.Substring(0, 3));
                        Drawing.DrawText(pos.X - Convert.ToInt32(hero.Name.Substring(0, 4).Length*5 + 1), pos.Y - 6, OutlineColor.Color, hero.Name.Substring(0, 3));
                        Drawing.DrawText(pos.X - Convert.ToInt32(hero.Name.Substring(0, 4).Length*5 - 1), pos.Y - 8, OutlineColor.Color, hero.Name.Substring(0, 3));

                        Drawing.DrawText(pos.X - Convert.ToInt32(hero.Name.Substring(0, 4).Length*5), pos.Y - 7, TextColor.Color, hero.Name.Substring(0, 3));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
