#region License
//  Copyright (c) 2009 Lev Danielyan
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of 
//  this software and associated documentation files (the "Software"), to deal in the 
//  Software without restriction, including without limitation the rights to use, copy, 
//  modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the 
//  following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies 
//  or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//  PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//  FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.

//Note:代码有修改，勿覆盖

#endregion

using System;

namespace Yintai.Hangzhou.Contract.Images.Exif
{
    internal sealed class Rational
    {
        private readonly Int32 _num;
        private readonly Int32 _denom;

        public Rational(byte[] bytes)
        {
            byte[] n = new byte[4];
            byte[] d = new byte[4];
            Array.Copy(bytes, 0, n, 0, 4);
            Array.Copy(bytes, 4, d, 0, 4);
            _num = BitConverter.ToInt32(n, 0);
            _denom = BitConverter.ToInt32(d, 0);
        }

        public double ToDouble()
        {
            return Math.Round(Convert.ToDouble(_num) / Convert.ToDouble(_denom), 2);
        }

        public string ToString(string separator)
        {
            return _num.ToString() + separator + _denom.ToString();
        }

        public override string ToString()
        {
            return ToString("/");
        }
    }

    internal sealed class URational
    {
        private readonly UInt32 _num;
        private readonly UInt32 _denom;

        public URational(byte[] bytes)
        {
            byte[] n = new byte[4];
            byte[] d = new byte[4];
            Array.Copy(bytes, 0, n, 0, 4);
            Array.Copy(bytes, 4, d, 0, 4);
            _num = BitConverter.ToUInt32(n, 0);
            _denom = BitConverter.ToUInt32(d, 0);
        }

        public double ToDouble()
        {
            return Math.Round(Convert.ToDouble(_num) / Convert.ToDouble(_denom), 2);
        }

        public override string ToString()
        {
            return ToString("/");
        }

        public string ToString(string separator)
        {
            return _num.ToString() + separator + _denom.ToString();
        }
    }

    internal sealed class GPSRational
    {
        private Rational _hours;
        private Rational _minutes;
        private Rational _seconds;

        public Rational Hours
        {
            get
            {
                return _hours;
            }
            set
            {
                _hours = value;
            }
        }
        public Rational Minutes
        {
            get
            {
                return _minutes;
            }
            set
            {
                _minutes = value;
            }
        }
        public Rational Seconds
        {
            get
            {
                return _seconds;
            }
            set
            {
                _seconds = value;
            }
        }

        public GPSRational(byte[] bytes)
        {
            byte[] h = new byte[8]; byte[] m = new byte[8]; byte[] s = new byte[8];

            Array.Copy(bytes, 0, h, 0, 8); Array.Copy(bytes, 8, m, 0, 8); Array.Copy(bytes, 16, s, 0, 8);

            _hours = new Rational(h);
            _minutes = new Rational(m);
            _seconds = new Rational(s);
        }

        public override string ToString()
        {
            return _hours.ToDouble() + "° "
                + _minutes.ToDouble() + "\' "
                + _seconds.ToDouble() + "\"";
        }

        public string ToString(string separator)
        {
            return _hours.ToDouble() + separator
                + _minutes.ToDouble() + separator +
                _seconds.ToDouble();
        }
    }
}
