
using System.ComponentModel;
using System.Numerics;
using System.Threading;
using System;

namespace Starfall.IO.Dataset
{
    public struct Monster
    {
        public int hp;
        public int atk;
        public int level;

        public enum MonsterType //enum 으로 번호매겨서 쉽게 호출하기 
        {
            Minion = 1, // 미니언
            Voidling = 2, //공허충
            SiegeMinion = 3 //대포미니언
        }
        
   
    }
}




