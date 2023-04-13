using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeidelMethod
{
    public class Matrix
    {
        public decimal[,] Values { get; set; }
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public Matrix(int rows, int columns)
        {
            if (rows < 0 || columns < 0)
            {
                throw new ArgumentException("Invalid number of rows or columns");
            }

            Rows = rows;
            Columns = columns;
            Values = new decimal[rows, columns];
        }

        public Matrix(decimal[,] matrix)
        {
            Rows = matrix.GetLength(0);
            Columns = matrix.GetLength(1);
            Values = matrix;
        }

        public Matrix(string filePath)
        {
            try
            {
                ReadMatrixFromFile(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading matrix from file: {ex.Message}");
            }
        }

        public static Vector operator *(Matrix matrix, Vector vector)
        {
            if (matrix.Columns != vector.Size)
                throw new ArgumentException("Matrix and vector dimensions are not compatible for multiplication.");

            Vector result = new Vector(matrix.Rows);

            for (int i = 0; i < matrix.Rows; i++)
            {
                decimal sum = 0;
                for (int j = 0; j < matrix.Columns; j++)
                {
                    sum += matrix.Values[i, j] * vector.Values[j, 0];
                }
                result.Values[i, 0] = sum;
            }

            return result;
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            if (a.Rows == b.Rows && a.Columns == b.Columns)
            {
                decimal[,] res = new decimal[a.Rows, a.Columns];
                for (int i = 0; i < a.Rows; ++i)
                {
                    for (int j = 0; j < a.Columns; ++j)
                    {
                        res[i, j] = a.Values[i, j] + b.Values[i, j];
                    }

                }
                return new Matrix(res);
            }
            else
            {
                throw new ArgumentException("Incorrect sizes", nameof(a));
            }
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            if (a.Rows == b.Rows && a.Columns == b.Columns)
            {
                decimal[,] res = new decimal[a.Rows, a.Columns];
                for (int i = 0; i < a.Rows; ++i)
                {
                    for (int j = 0; j < a.Columns; ++j)
                    {
                        res[i, j] = a.Values[i, j] - b.Values[i, j];
                    }

                }
                return new Matrix(res);
            }
            else
            {
                throw new ArgumentException("Incorrect sizes", nameof(a));
            }
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            decimal sum = 0;
            decimal[,] result = new decimal[a.Rows, b.Columns];
            if (a.Columns == b.Rows)
            {
                for (int i = 0; i < a.Rows; i++)
                {
                    for (int j = 0; j < b.Columns; j++)
                    {
                        sum = 0;
                        for (int k = 0; k < a.Columns; k++)
                        {
                            sum += a.Values[i, k] * b.Values[k, j];
                        }
                        result[i, j] = sum;
                    }

                }
                return new Matrix(result);
            }
            else
            {
                throw new ArgumentException("Non-conformable matrices in MultiplyMatrix", nameof(b));
            }
        }

        public void AddRowOfZeros()
        {
            decimal[,] newValues = new decimal[Rows + 1, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    newValues[i, j] = Values[i, j];
                }
            }
            Values = newValues;
            Rows++;
            for (int j = 0; j < Columns; j++)
            {
                Values[Rows - 1, j] = 0;
            }
        }

        public void RemoveLastRow()
        {
            if (Rows == 0)
            {
                throw new InvalidOperationException("You can't remove a row since there aren't rows in the matrix");
            }

            decimal[,] newValues = new decimal[Rows - 1, Columns];
            for (int i = 0; i < Rows - 1; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    newValues[i, j] = Values[i, j];
                }
            }
            Values = newValues;
            Rows--;
        }

        public void AddColumnOfZeros()
        {
            decimal[,] newValues = new decimal[Rows, Columns + 1];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    newValues[i, j] = Values[i, j];
                }

                newValues[i, Columns] = 0;
            }

            Columns++;
            Values = newValues;
        }

        public void RemoveLastColumn()
        {
            if (Columns == 0)
            {
                throw new InvalidOperationException("You can't remove column since there aren't columns in the matrix");
            }

            decimal[,] newValues = new decimal[Rows, Columns - 1];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns - 1; j++)
                {
                    newValues[i, j] = Values[i, j];
                }

            }

            Columns--;
            Values = newValues;
        }

        public void ReadMatrixFromFile(string filePath)
        {
            try
            {
                string[] lines = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, filePath));
                int rows = lines.Length;
                int cols = lines[0].Split(' ').Length;

                Values = new decimal[rows, cols];
                Columns = cols;
                Rows = rows;


                for (int i = 0; i < rows; i++)
                {
                    string[] rowValues = lines[i].Split(' ');
                    if (rowValues.Length != cols)
                    {
                        throw new ArgumentException("Invalid file format. Number of columns in the matrix is inconsistent.");
                    }
                    for (int j = 0; j < cols; j++)
                    {
                        decimal cellValue;
                        if (!decimal.TryParse(rowValues[j], out cellValue))
                        {
                            throw new ArgumentException($"Invalid value in row {i + 1}, column {j + 1}: '{rowValues[j]}' is not a valid number.");
                        }
                        Values[i, j] = cellValue;
                    }
                }
            }
            catch (DirectoryNotFoundException e)
            {
                throw new DirectoryNotFoundException($"File path hasn't been founded: {e.Message}");
            }
        }

        public void FillMatrixRandomly(int start, int end)
        {
            Random rnd = new Random();
            for (int i = 0; i < Rows; ++i)
            {
                for (int j = 0; j < Columns; ++j)
                {
                    Values[i, j] = Convert.ToInt32(rnd.Next(start, end));
                }
            }
        }

        public decimal FindMaxModul()
        {
            decimal max = 0;
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    max = Math.Abs(Values[i, j]) > max ? Math.Abs(Values[i, j]) : max;
                }
            }

            return max;
        }

        public decimal FindEvklidNorm()
        {
            decimal normSquared = 0;
            for (var i = 0; i < Rows; i++)
            {
                for (var j = 0; j < Columns; j++)
                {
                    normSquared += (decimal)Math.Pow((double)Values[i, j], 2);
                }
            }

            return (decimal)Math.Sqrt((double)normSquared);
        }

        public void SwapRows(int row1, int row2)
        {
            if (row1 == row2)
            {
                return;
            }

            decimal temp;
            for (int j = 0; j < Columns; j++)
            {
                temp = Values[row1, j];
                Values[row1, j] = Values[row2, j];
                Values[row2, j] = temp;
            }
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
