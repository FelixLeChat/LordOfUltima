using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LordOfUltima
{
    class Gameboard
    {
        private static Gameboard m_ins = null;
        private int width = 830;
        private int start_width = 55;
        private int height = 560;
        private int start_height = 70;
        
        private int frame_width;
        private int frame_height;
        private int frame_count = 19;

        private int[,] map;

        private Gameboard()
        {
            map = new int[frame_count,frame_count];
            frame_width = (width-start_width)/frame_count;
            frame_height = (height - start_height)/frame_count;
        }

        public static Gameboard getInstance()
        {
            if( m_ins == null)
            {
                m_ins = new Gameboard();
            }
            return m_ins;
        }

        public int getXframe(int x)
        {
            int frame = (x-start_width);

            return frame;
        }
    }
}
