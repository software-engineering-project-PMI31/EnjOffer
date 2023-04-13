using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SeidelMethod
{
    public static class Output
    {
        public static DataTable CreateDataTableFromMatrix(Matrix matrix)
        {
            DataTable matrixTable = new DataTable();

            for (int i = 0; i < matrix.Columns; i++)
            {
                matrixTable.Columns.Add(new DataColumn($"Column {i}", typeof(double)));
            }

            for (int i = 0; i < matrix.Rows; i++)
            {
                DataRow row = matrixTable.NewRow();
                for (int j = 0; j < matrix.Columns; j++)
                {
                    row[j] = matrix.Values[i, j];
                }
                matrixTable.Rows.Add(row);
            }

            return matrixTable;
        }

        public static DataTable CreateDataTableFromMatrix(Vector vector)
        {
            DataTable matrixTable = new DataTable();
            matrixTable.Columns.Add(new DataColumn($"Column {1}", typeof(double)));

            for (int i = 0; i < vector.Rows; i++)
            {
                DataRow row = matrixTable.NewRow();
                for (int j = 0; j < vector.Columns; j++)
                {
                    row[j] = vector.Values[i, j];
                }
                matrixTable.Rows.Add(row);
            }

            return matrixTable;
        }

        public static void WriteMatrixToFile(Matrix matrix, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Path.Combine(Environment.CurrentDirectory, filePath)))
                {
                    int rows = matrix.Rows;
                    int cols = matrix.Columns;

                    for (int i = 0; i < rows; i++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            writer.Write(matrix.Values[i, j].ToString());
                            if (j < cols - 1)
                            {
                                writer.Write(" ");
                            }
                        }
                        writer.WriteLine();
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error while writing the matrix in the file: {ex.Message}");
            }
        }

        public static void WriteVectorToFile(Vector vector, string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    int size = vector.Size;

                    for (int i = 0; i < size; i++)
                    {
                        writer.Write(vector.Values[i, 0].ToString());
                        if (i < size - 1)
                        {
                            writer.Write(" ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Error writing vector to file: {ex.Message}");
            }
        }
    }
}
