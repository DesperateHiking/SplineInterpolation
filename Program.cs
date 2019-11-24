using System;

namespace SplineInterpolation
{
    class Program
    {
        static double[] ThomasMethod(double[,] mat)
        {
            var rows = mat.GetLength(0);
            var uArr = new double[rows + 1];
            var vArr = new double[rows + 1];
            var solution = new double[rows + 2];

            for (var i = 0; i < rows; i++)
            {
                uArr[i + 1] = GetU(uArr[i], mat[i, 0], mat[i, 1], mat[i, 2]);
                vArr[i + 1] = GetV(uArr[i], vArr[i], mat[i, 0], mat[i, 1], mat[i, 2], mat[i, 3]);
            }

            for (var i = rows; i >= 0; i--)
            {
                solution[i] = Math.Round(uArr[i] * solution[i + 1] + vArr[i], 3);
            }

            return solution;
        }

        static double GetU(double U0, double a, double b, double c)
        {
            return -(c / (a * U0 + b));
        }

        static double GetV(double U0, double V0, double a, double b, double c, double d)
        {
            return (d - a * V0) / (a * U0 + b);
        }

        static void Interpolation(double[] xArr, double[] yArr)
        {
            var tridiagonalMatrix = new double[5, 4];

            for (var i = 1; i < xArr.Length - 1; i++)
            {
                for (var j = 0; j < tridiagonalMatrix.GetLength(1); j++)
                    tridiagonalMatrix[i - 1, j] = GetEssentialSpline(xArr, yArr, i)[j];
            }
            var cArr = ThomasMethod(tridiagonalMatrix);

            var bArr = new double[tridiagonalMatrix.GetLength(0)];
            var dArr = new double[tridiagonalMatrix.GetLength(0)];

            for (var i = 1; i < xArr.Length - 1; i++)
            {
                bArr[i - 1] = Math.Round((yArr[i + 1] - yArr[i]) / (xArr[i + 1] - xArr[i]) - ((xArr[i + 1] - xArr[i]) / 3 * (cArr[i + 1] + 2 * cArr[i])), 3);
                dArr[i - 1] = Math.Round((cArr[i + 1] - cArr[i]) / (3 * (xArr[i + 1] - xArr[i])), 3);
                Console.WriteLine("f{0}(x) = {1} + {2}(x - {3}) + {4}(x - {3})^2 + {5}(x - {3})^3", i, yArr[i], bArr[i - 1], xArr[i], cArr[i], dArr[i - 1]);
            }
        }

        static double[] GetEssentialSpline(double[] xArr, double[] yArr, int i)
        {
            var res = new double[4];
            res[0] = xArr[i] - xArr[i - 1];
            res[1] = 2 * (xArr[i + 1] - xArr[i - 1]);
            res[2] = xArr[i + 1] - xArr[i];
            res[3] = 3 * ((yArr[i + 1] - yArr[i]) / (xArr[i + 1] - xArr[i]) - (yArr[i] - yArr[i - 1]) / (xArr[i] - xArr[i - 1]));
            if (i == 1)
                res[0] = 0;
            if (i == 5)
                res[2] = 0;

            return res;
        }

        static void Main(string[] args)
        {
            var xArr = new double[] { 0, 1, 1.2, 1.4, 1.6, 1.8, 2 };
            var yArr = new double[] { 0, 0.9, 2, 3, 3.8, 5.1, 5.8 };
            Interpolation(xArr, yArr);
            Console.ReadKey();
        }
    }
}
