using UnityEngine;

public class Try : MonoBehaviour
{
    public GameObject prefab; // Reference to your prefab
    public int numberOfPoints = 100; // Number of points to spawn

    void Start()
    {
        // Given data
        double[] x = { 0.01, 0.0025, 0.0011, 0.000625, 0.0004, 0.000278, 0.000204, 0.000156, 0.000123, 0.0001 };
        double[] r = { 0.5747, 1.0977, 2.02, 3.3437, 5.0667, 7.1897, 9.7127, 12.6357, 15.9587, 19.6817 };

        // Fit a polynomial of degree 9
        double[] coefficients = FitPolynomial(x, r, 9);

        // Plot points and spawn prefabs
        for (int i = 0; i < numberOfPoints; i++)
        {
            float randomX = Random.Range(0.0001f, 0.01f); // Random x value within the given range
            float y = (float)EvaluatePolynomial(coefficients, randomX); // Explicit cast to float

            // Spawn prefab at the point
            Vector3 spawnPosition = new Vector3(randomX, y, 0f);
            Instantiate(prefab, spawnPosition, Quaternion.identity);
        }
    }

    double[] FitPolynomial(double[] x, double[] y, int degree)
    {
        // Fit a polynomial using the least squares method
        int n = x.Length;
        int m = degree + 1;

        double[] X = new double[n * m];
        double[] Y = new double[n];

        for (int i = 0; i < n; i++)
        {
            double xi = 1.0;
            for (int j = 0; j < m; j++)
            {
                X[i * m + j] = xi;
                xi *= x[i];
            }
            Y[i] = y[i];
        }

        double[] result = new double[m];
        MatrixSolve(X, Y, m, result);

        return result;
    }

    void MatrixSolve(double[] A, double[] B, int n, double[] result)
    {
        // Solve Ax = B for x using Gaussian elimination with back substitution
        for (int i = 0; i < n; i++)
        {
            int pivot = i;
            for (int j = i + 1; j < n; j++)
            {
                if (Mathf.Abs((float)A[j * n + i]) > Mathf.Abs((float)A[pivot * n + i]))
                    pivot = j;
            }

            // Swap rows
            for (int k = 0; k < n; k++)
            {
                double temp = A[i * n + k];
                A[i * n + k] = A[pivot * n + k];
                A[pivot * n + k] = temp;
            }

            double tempB = B[i];
            B[i] = B[pivot];
            B[pivot] = tempB;

            // Eliminate
            for (int j = i + 1; j < n; j++)
            {
                double factor = A[j * n + i] / A[i * n + i];
                B[j] -= factor * B[i];
                for (int k = i; k < n; k++)
                {
                    A[j * n + k] -= factor * A[i * n + k];
                }
            }
        }

        // Back substitution
        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++)
            {
                sum += A[i * n + j] * result[j];
            }
            result[i] = (B[i] - sum) / A[i * n + i];
        }
    }

    double EvaluatePolynomial(double[] coefficients, double x)
    {
        // Evaluate the polynomial for a given x
        double result = 0;
        for (int i = 0; i < coefficients.Length; i++)
        {
            result += coefficients[i] * Mathf.Pow((float)x, i); // Use Mathf.Pow for exponentiation
        }
        return result;
    }
}
