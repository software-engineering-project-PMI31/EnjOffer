using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
//using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SeidelMethod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Matrix> matrixList = new List<Matrix>(3);
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            FirstMatrix.CanUserAddRows = false;
            VectorForSeidel.CanUserAddRows = false;
            ResultMatrix.CanUserAddRows = false;
        }

        private void Button_Click_GenerateRandomly(object sender, RoutedEventArgs e)
        {
            if (!IsNotEmptyInputs(RowsInput, ColsInput, MinimumElInput, MaximumElInput))
            {
                ResultBox.Text = "There are empty inputs";
                return;
            }

            if (Convert.ToInt32(RowsInput.Text) <= 0 || Convert.ToInt32(ColsInput.Text) <= 0)
            {
                ResultBox.Text = "The number of rows and columns are required to be greater or equal to 0";
                return;
            }

            if (Convert.ToInt32(MinimumElInput.Text) > Convert.ToInt32(MaximumElInput.Text))
            {
                ResultBox.Text = "Minimum element is required to be greater or equal to maximum element";
                return;
            }

            switch (ActionComboBox.Text)
            {
                case "Fill matrix 1":
                    Matrix matrix = new Matrix(Convert.ToInt32(RowsInput.Text), Convert.ToInt32(ColsInput.Text));
                    matrix.FillMatrixRandomly(Convert.ToInt32(MinimumElInput.Text), Convert.ToInt32(MaximumElInput.Text));
                    try
                    {
                        matrixList[0] = matrix;

                        Vector solutionVector = new Vector(matrixList[0].Rows);
                        VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        matrixList.Add(matrix);

                        Vector solutionVector = new Vector(matrixList[0].Rows);
                        VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;
                    }

                    DataTable matrixTable = Output.CreateDataTableFromMatrix(matrix);

                    FirstMatrix.ItemsSource = matrixTable.DefaultView;
                    ResultBox.Text = matrix.ToString();

                    break;

                case "Fill vector":
                    try
                    {
                        Vector solutionVector = new Vector(matrixList[0].Rows);
                        solutionVector.FillMatrixRandomly(Convert.ToInt32(MinimumElInput.Text), Convert.ToInt32(MaximumElInput.Text));
                        VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;
                        try
                        {
                            matrixList[1] = solutionVector;
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            matrixList.Add(solutionVector);
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ResultBox.Text = $"Firstly, you need to fill matrix";
                    }

                    break;
            }
        }

        private void Button_Click_AddColumnToFirstMatrix(object sender, RoutedEventArgs e)
        {
            try
            {
                matrixList[0].AddColumnOfZeros();
                FirstMatrix.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[0]).DefaultView;

                Vector solutionVector = new Vector(matrixList[0].Rows);
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;

                ResultBox.Text = matrixList[0].ToString();

            }
            catch (ArgumentOutOfRangeException)
            {
                Matrix matrix = new Matrix(0, 0);
                matrix.AddColumnOfZeros();
                matrixList.Add(matrix);

                Vector solutionVector = new Vector(matrixList[0].Rows);
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;
            }
        }

        private void Button_Click_RemoveColumnInFirstMatrix(object sender, RoutedEventArgs e)
        {
            try
            {
                matrixList[0].RemoveLastColumn();
                FirstMatrix.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[0]).DefaultView;

                Vector solutionVector = new Vector(matrixList[0].Rows);
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;

                ResultBox.Text = matrixList[0].ToString();

            }
            catch (ArgumentOutOfRangeException ex)
            {
                ResultBox.Text = $"You must add any matrix before adding a column. Error: {ex.Message}";
            }
        }

        private void Button_Click_AddRowToFirstMatrix(object sender, RoutedEventArgs e)
        {
            try
            {
                matrixList[0].AddRowOfZeros();
                FirstMatrix.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[0]).DefaultView;

                Vector solutionVector = new Vector(matrixList[0].Rows);
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;

                ResultBox.Text = matrixList[0].ToString();

            }
            catch (ArgumentOutOfRangeException)
            {
                Matrix matrix = new Matrix(0, 0);
                matrix.AddRowOfZeros();
                matrixList.Add(matrix);

                Vector solutionVector = new Vector(matrixList[0].Rows);
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;
            }
        }

        private void Button_Click_RemoveRowInFirstMatrix(object sender, RoutedEventArgs e)
        {
            try
            {
                matrixList[0].RemoveLastRow();
                FirstMatrix.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[0]).DefaultView;

                Vector solutionVector = new Vector(matrixList[0].Rows);
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(solutionVector).DefaultView;

                ResultBox.Text = matrixList[0].ToString();

            }
            catch (ArgumentOutOfRangeException ex)
            {
                ResultBox.Text = $"You must add any matrix before adding a column. Error: {ex.Message}";
            }
        }

        private void Button_Click_SaveChanges(object sender, RoutedEventArgs e)
        {
            Matrix newMatrixGaussian = new Matrix(FirstMatrix.Items.Count, FirstMatrix.Columns.Count);
            Vector vector = new Vector(VectorForSeidel.Items.Count);

            TextBlock? x;
            for (int i = 0; i < FirstMatrix.Items.Count; i++)
            {
                for (int j = 0; j < FirstMatrix.Columns.Count; j++)
                {
                    x = FirstMatrix.Columns[j].GetCellContent(FirstMatrix.Items[i]) as TextBlock;
                    newMatrixGaussian.Values[i, j] = x is not null ? Convert.ToDecimal(x.Text) : 0;
                }
            }

            for (int i = 0; i < VectorForSeidel.Items.Count; i++)
            {
                x = VectorForSeidel.Columns[0].GetCellContent(VectorForSeidel.Items[i]) as TextBlock;
                vector.Values[i, 0] = x is not null ? Convert.ToDecimal(x.Text) : 0;
            }

            try
            {
                matrixList[0] = newMatrixGaussian;
            }
            catch (ArgumentOutOfRangeException)
            {
                matrixList.Add(newMatrixGaussian);
            }

            try
            {
                matrixList[1] = vector;
            }
            catch (ArgumentOutOfRangeException)
            {
                matrixList.Add(vector);
            }
        }

        private void Button_Click_GenerateFirstMatrixFromFile(object sender, RoutedEventArgs e)
        {
            try
            {
                matrixList[0] = new Matrix(PathFirstMatrix.Text);
                FirstMatrix.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[0]).DefaultView;
                ResultBox.Text = matrixList[0].ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                matrixList.Add(new Matrix(PathFirstMatrix.Text));
                FirstMatrix.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[0]).DefaultView;
                ResultBox.Text = matrixList[0].ToString();
            }
        }

        private void Button_Click_GenerateVectorForSeidelFromFile(object sender, RoutedEventArgs e)
        {
            try
            {
                matrixList[1] = new Vector(PathVectorForSeidel.Text);
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[1]).DefaultView;
                ResultBox.Text = matrixList[1].ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                matrixList.Add(new Vector(PathVectorForSeidel.Text));
                VectorForSeidel.ItemsSource = Output.CreateDataTableFromMatrix(matrixList[1]).DefaultView;
                ResultBox.Text = matrixList[1].ToString();
            }
        }

        private void Button_Click_FindEvklidNormFirstMatrix(object sender, RoutedEventArgs e)
        {
            if (matrixList.ElementAtOrDefault(0) is null)
            {
                ResultBox.Text = "Please, fill matrices";
            }
            else
            {
                ResultBox.Text = $"Evklid norm: { matrixList[0].FindEvklidNorm()}";
            }
        }

        private void Button_Click_FindMaxModuleNormFirstMatrix(object sender, RoutedEventArgs e)
        {
            if (matrixList.ElementAtOrDefault(0) is null)
            {
                ResultBox.Text = "Please, fill matrices";
            }
            else
            {
                ResultBox.Text = $"Max module: { matrixList[0].FindMaxModul()}";
            }
        }

        private void Button_Click_SaveResultMatrixToFile(object sender, RoutedEventArgs e)
        {
            if (matrixList.ElementAtOrDefault(2) is null)
            {
                ResultBox.Text = $"There isn't any result";
            }
            else
            {
                Output.WriteMatrixToFile(matrixList[2], $"{ResultMatrixAddress.Text}");
            }
        }

        private bool IsNotEmptyInputs(params TextBox[] inputs)
        {
            return inputs.All(x => !string.IsNullOrEmpty(x.Text));
        }

        private void Button_Click_Solve(object sender, RoutedEventArgs e)
        {
            try
            {
                if (typeof(Vector) != matrixList[1].GetType())
                {
                    throw new Exception("matrixList[1] isn't a Vector type");
                }

                SystemLinearArithmeticEquation slar = new SystemLinearArithmeticEquation(matrixList[0], (Vector)matrixList[1]);
                Matrix resultTable = new Matrix(1000, slar.A.Columns * 2 + 1);

                decimal[,] resultSeidel = slar.SolveSeidel(out resultTable);

                ResultMatrix.ItemsSource = Output.CreateDataTableFromMatrix(resultTable).DefaultView;
                ResultMatrix.Columns[0].Header = "Iteration";
                int indx = 1;
                for (; indx < slar.A.Columns; indx++)
                {
                    ResultMatrix.Columns[0].Header = $"x{indx}";
                }

                for (; indx < slar.A.Columns * 2; indx++)
                {
                    ResultMatrix.Columns[0].Header = $"Delta x{indx}";
                }

                ResultBox.Text = "";
                for (int i = 0; i < resultSeidel.GetLength(0); i++)
                {
                    ResultBox.Text += $"{resultSeidel[i, 0]}\n";
                }


            }
            catch (ArgumentOutOfRangeException)
            {
                ResultBox.Text = "You need to fill matrix and vector";
            }

        }
    }
}
