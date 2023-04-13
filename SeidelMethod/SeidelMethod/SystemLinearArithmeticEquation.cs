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

        public double[,] SolveSeidel(out Matrix outTable, int maxIterations = 100000, double relativeError = 0.0001)
        {
            int n = A.Rows;
            Vector x = new Vector(A.Rows);
            Vector xx = new Vector(A.Rows);
            int iteration;
            double[] gap = new double[n];
            double maxGap = 1;
            Matrix resultTable = new Matrix(maxIterations, A.Columns * 2 + 1);

            for (iteration = 0; iteration < maxIterations && maxGap > relativeError; iteration++)
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
                            sum += A.Values[i, j] * x.Values[j, 0];
                        }
                    }

                    x.Values[i, 0] = (-(sum - B.Values[i, 0]) / A.Values[i, i]);

                    if (x.Values[i, 0] is double.NaN || double.IsInfinity(x.Values[i, 0]))
                    {
                        throw new Exception("The method did not converge within the specified number of iterations.");
                    }

                    resultTable.Values[iteration, colIndexBeforeError++] = x.Values[i, 0];
                }

                for (int i = 0; i < n; i++)
                {
                    gap[i] = Math.Abs(x.Values[i, 0] - xx.Values[i, 0]) / Math.Max(Math.Abs(x.Values[i, 0]), 1);
                    resultTable.Values[iteration, colIndexBeforeError++] = gap[i];
                }

                maxGap = gap.Max();
            }

            if (iteration == maxIterations && maxGap > relativeError)
            {
                throw new Exception("The method did not converge within the specified number of iterations.");
            }

            outTable = resultTable;

            return x.Values;
        }
    }
}
