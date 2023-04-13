using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeidelMethod
{
    public class SystemLinearArithmeticEquation
    {
        public Matrix A { get; private set; }
        public Vector X { get; private set; }
        public Vector B { get; private set; }
        public int Size { get; private set; }

        public SystemLinearArithmeticEquation(Matrix a, Vector b)
        {
            A = a;
            X = new Vector(A.Columns);
            B = b;
        }

        public decimal[,] SolveSeidel(out Matrix outTable, int maxIterations = 1000, double relativeError = 1e-6)
        {
            int n = A.Rows;
            Vector x = new Vector(A.Rows);
            Vector xx = new Vector(A.Rows);
            int iteration;
            decimal[] gap = new decimal[n];
            decimal maxGap = 1;
            Matrix resultTable = new Matrix(maxIterations, A.Columns * 2 + 1);

            for (iteration = 0; iteration < maxIterations && maxGap > (decimal)relativeError; iteration++)
            {
                Array.Copy(x.Values, xx.Values, x.Values.Length);
                resultTable.Values[iteration, 0] = iteration;
                int colIndexBeforeError = 1;
                for (int i = 0; i < n; i++)
                {
                    double sum = 0;

                    for (int j = 0; j < n; j++)
                    {
                        if (j != i)
                        {
                            sum += (double)A.Values[i, j] * (double)x.Values[j, 0];
                        }
                    }

                    x.Values[i, 0] = (decimal)(-(sum - (double)B.Values[i, 0]) / (double)A.Values[i, i]);

                    resultTable.Values[iteration, colIndexBeforeError++] = x.Values[i, 0];
                }

                for (int i = 0; i < n; i++)
                {
                    gap[i] = Math.Abs(x.Values[i, 0] - xx.Values[i, 0]) / Math.Max(Math.Abs(x.Values[i, 0]), 1);
                    resultTable.Values[iteration, colIndexBeforeError++] = gap[i];
                }

                maxGap = gap.Max();
            }

            if (iteration == maxIterations && maxGap > (decimal)relativeError)
            {
                throw new Exception("The method did not converge within the specified number of iterations.");
            }

            outTable = resultTable;

            return x.Values;
        }

        /*public decimal[,] SolveSeidel(double tolerance = 1e-3, int maxIterations = 1000, double eps = 0.001)
        {
            int n = A.Rows;
            Vector x = new Vector(A.Rows);
            Vector xx = new Vector(A.Rows);
            int MAX_ITERATIONS = 1000;

            for (int iteration = 0; iteration < MAX_ITERATIONS; iteration++)
            {
                Array.Copy(x.Values, xx.Values, x.Values.Length);

                for (int i = 0; i < n; i++)
                {
                    decimal sum = 0;

                    for (int j = 0; j < n; j++)
                    {
                        if (j != i)
                        {
                            sum += A.Values[i, j] * x.Values[j, 0];
                        }
                    }

                    x.Values[i, 0] = -(sum - B.Values[i, 0]) / A.Values[i, i];
                }

                decimal[] gap = new decimal[n];

                for (int i = 0; i < n; i++)
                {
                    gap[i] = Math.Abs(x.Values[i, 0] - xx.Values[i, 0]);
                }

                if (gap.Max() < (decimal)eps)
                {
                    break;
                }
            }

            return x.Values;
        }*/
    }
}
