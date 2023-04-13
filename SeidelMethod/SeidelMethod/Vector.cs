using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeidelMethod
{
    public class Vector : Matrix
    {
        public int Size { get; private set; }
        public Vector(int m) : base(m, 1)
        {
            Size = Rows;
        }

        public Vector(decimal[,] matr) : base(matr)
        {
            Size = matr.GetLength(0);
        }

        public Vector(string file) : base(file)
        {
            //Size = File.ReadAllText(file).Split(' ').Length;
            Size = File.ReadAllLines(file).Length;
            //Size = lines.Length;
        }

        public static Vector operator *(Vector a, Vector b)
        {
            decimal[,] res = new decimal[a.Size, 1];
            if (a.Size == b.Size)
            {
                for (int i = 0; i < a.Size; ++i)
                {
                    res[i, 0] = a.Values[i, 0] * b.Values[i, 0];
                }
                return new Vector(res);
            }
            else
            {
                throw new ArgumentException("Incorrect sizes", nameof(a));
            }
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.Size != v2.Size)
            {
                throw new ArgumentException("Vectors must have the same length");
            }

            decimal[,] resultValues = new decimal[v1.Size, 1];
            for (int i = 0; i < v1.Size; i++)
            {
                resultValues[i, 0] = v1.Values[i, 0] - v2.Values[i, 0];
            }
            return new Vector(resultValues);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.Append($"{Values[i, j]} ");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
