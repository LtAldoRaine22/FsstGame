using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FsstGame
{
    public class Timer
    {
        private bool state;
        private int value;
        private int freq;

        public bool State
        {
            get { return state; }
            set { state = value; }
        }

        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public int Freq
        {
            get { return freq; }
            set { freq = value; }
        }

        public Timer(int value, int freq, bool state)
        {
            this.value = value;
            this.freq = freq;
            this.state = state;
        }

        public void Update()
        {
            if (!state)
                value++;
            if (value >= freq)
            {
                value = 0;
                state = true;
            }
        }
    }
}