using System;
using System.Collections.Generic;
using SharpDX;

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

namespace JungleTimers
{
    internal class JungleCamp
    {
        public TimeSpan SpawnTime { get; set; }
        public TimeSpan RespawnTimer { get; set; }
        public Vector3 Position { get; set; }
        public List<JungleMinion> Minions { get; set; }
        public JungleCampState State { get; set; }
        public float ClearTick { get; set; }
    }
}