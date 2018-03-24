using System;
using System.Diagnostics;

namespace GoodNumbers
{
    class Program
    {
        // Total iterations = 9 * Log10(n)/2 = O(logN)  (much better than the O(SqrtN) solution previously proposed).
        // Space requirement is also reduced, although original space was quite small already, at O(logN), so not as interesting.
        /*
         * Results of testing on my laptop (milliseconds per 1 million executions) confirm expected performance,
         * where each new pair of digits (which increases n by 100x) adds 10x to the O(SqrtN) version, and adds 2-3x to the
         * O(logN) version.  By the 1,000,000,000 to 9,999,999,999 version we are 65x faster, an this can scale much higher.
         *  6.1: 608 vs 7395 : 12x (this version uses extra optimizations that do not apply at larger iterations)
         *  6.2: 1652 vs 7395 : 4.5x (this uses the same exact code that scales to the 8 and 10 digit versions)
         *  8: 5136 vs 72867 : 14x, old is 10x slower, new is 3x slower
         *  10: 12290 vs 805222 : 65x, old is 11x slower, new is 2.4x slower
         */
        static void Main(string[] args)
        {
            // First do the 6 digit version from interview, where max total is (6 / 2) * 9 = 27
            int totalDigits = 6;
            int maxTotal = (totalDigits / 2) * 9;

            long goodNumberCount = 0;
            Stopwatch watch = new Stopwatch();
            int perfLoops;

            // Algorithm optimized for 6 digit problem; faster but less elegant and unusable for larger problems
            watch.Restart();
            perfLoops = 1000000;
            for (int perfLoop = 0; perfLoop < perfLoops; perfLoop++)
            {
                // Cache the binomial coefficients for binomial(2, k+2)
                int[] binomialCoefficientCache = new int[(maxTotal / 2) + 1];
                for (int i = 0; i <= maxTotal / 2; i++)
                {
                    binomialCoefficientCache[i] = ((i + 2) * (i + 1)) / 2;
                }

                // Now iterate through the possible totals and calculate the number of matching combinations.
                goodNumberCount = 0;
                long leftFrequency, rightFrequency;
                for (int i = 1; i <= maxTotal; i++)
                {
                    // Performance note:
                    //   What we are doing here is really:
                    //    rightFrequency = GetFrequency(3, i);
                    //    leftFrequency = rightFrequency - GetFrequency(2, i); // To remove leading zero numbers from count
                    //   But we get much better performance inlining the calculations, so here is the faster but uglier code:

                    int k = i > 13 ? maxTotal - i : i;  // First apply symmetry
                    rightFrequency = k <= 9 ?
                            binomialCoefficientCache[k] :
                            binomialCoefficientCache[k] - (3 * binomialCoefficientCache[k - 10]);
                    leftFrequency = rightFrequency - (i > 18 ? 0 : i <= 9 ? i + 1 : 19 - i);

                    goodNumberCount += leftFrequency * rightFrequency;
                }
            }
            watch.Stop();
            Console.WriteLine("Result: {0}  ({1} ms for {2} iterations)", goodNumberCount, watch.ElapsedMilliseconds, perfLoops);

            // Generalized solution applied to 6 digit problem; slower than fully optimized version but still faster than interview solution
            watch.Restart();
            perfLoops = 1000000;
            for (int perfLoop = 0; perfLoop < perfLoops; perfLoop++)
            {
                goodNumberCount = CountGoodNumbers(6);
            }
            watch.Stop();
            Console.WriteLine("Result: {0}  ({1} ms for {2} iterations)", goodNumberCount, watch.ElapsedMilliseconds, perfLoops);

            // Counting all combinations (interview solution)
            watch.Restart();
            perfLoops = 1000000;
            for (int perfLoop = 0; perfLoop < perfLoops; perfLoop++)
            {
                int[] leftArray = new int[maxTotal];
                int[] rightArray = new int[maxTotal];
                for (int i = 0;i<10;i++)
                {
                    for (int j=0;j<10;j++)
                    {
                        for (int k=0;k<10;k++)
                        {
                            int total = i + j + k;
                            if (total != 0)
                            {
                                rightArray[total - 1]++;
                            }
                            if (i != 0)
                            {
                                leftArray[total-1]++;
                            }
                        }
                    }
                }
                goodNumberCount = 0;
                for (int i =0;i<maxTotal;i++)
                {
                    goodNumberCount += leftArray[i] * rightArray[i];
                }
            }

            watch.Stop();
            Console.WriteLine("Result: {0}  ({1} ms for {2} iterations)", goodNumberCount, watch.ElapsedMilliseconds, perfLoops);

            // Switch to 8 digit
            totalDigits = 8;
            maxTotal = (totalDigits / 2) * 9;

            // Using binomial coefficients
            watch.Restart();
            perfLoops = 1;
            for (int perfLoop = 0; perfLoop < perfLoops; perfLoop++)
            {
                goodNumberCount = CountGoodNumbers(totalDigits);
            }
            watch.Stop();
            Console.WriteLine("Result: {0}  ({1} ms for {2} iterations)", goodNumberCount, watch.ElapsedMilliseconds, perfLoops);

            // Counting all combinations
            watch.Restart();
            perfLoops = 1;
            for (int perfLoop = 0; perfLoop < perfLoops; perfLoop++)
            {
                int[] leftArray = new int[maxTotal];
                int[] rightArray = new int[maxTotal];
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            for (int l = 0; l < 10; l++)
                            {
                                int total = i + j + k + l;
                                if (total != 0)
                                {
                                    rightArray[total - 1]++;
                                }
                                if (i != 0)
                                {
                                    leftArray[total - 1]++;
                                }
                            }
                        }
                    }
                }
                goodNumberCount = 0;
                for (int i = 0; i < maxTotal; i++)
                {
                    goodNumberCount += leftArray[i] * rightArray[i];
                }
            }

            watch.Stop();
            Console.WriteLine("Result: {0}  ({1} ms for {2} iterations)", goodNumberCount, watch.ElapsedMilliseconds, perfLoops);

            // Switch to 10 digit, with max total of 45 per side
            totalDigits = 10;
            maxTotal = (totalDigits / 2) * 9;

            // Using binomial coefficients
            watch.Restart();
            perfLoops = 1;
            for (int perfLoop = 0; perfLoop < perfLoops; perfLoop++)
            {
                goodNumberCount = CountGoodNumbers(totalDigits);
            }
            watch.Stop();
            Console.WriteLine("Result: {0}  ({1} ms for {2} iterations)", goodNumberCount, watch.ElapsedMilliseconds, perfLoops);

            // Counting all combinations
            watch.Restart();
            perfLoops = 1;
            for (int perfLoop = 0; perfLoop < perfLoops; perfLoop++)
            {
                int[] leftArray = new int[maxTotal];
                int[] rightArray = new int[maxTotal];
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        for (int k = 0; k < 10; k++)
                        {
                            for (int l = 0; l < 10; l++)
                            {
                                for (int m = 0; m < 10; m++)
                                {
                                    int total = i + j + k + l + m;
                                    if (total != 0)
                                    {
                                        rightArray[total - 1]++;
                                    }
                                    if (i != 0)
                                    {
                                        leftArray[total - 1]++;
                                    }
                                }
                            }
                        }
                    }
                }
                goodNumberCount = 0;
                for (int i = 0; i < maxTotal; i++)
                {
                    goodNumberCount += leftArray[i] * rightArray[i];
                }
            }

            watch.Stop();
            Console.WriteLine("Result: {0}  ({1} ms for {2} iterations)", goodNumberCount, watch.ElapsedMilliseconds, perfLoops);

            Console.ReadKey();
        }

        /// <summary>
        /// Counts how many numbers of length "totalDigits", without leading zeroes, have the characteristic
        /// of the sum of the digits in the first half of the number equalling he sum of the digits in the
        /// second half.
        /// </summary>
        /// <param name="totalDigits">Total length of the numbers to solve for.  E.g. totalDigits = 6 will solve for 100,000..999,999</param>
        /// <returns>See summary.</returns>
        private static long CountGoodNumbers(int totalDigits)
        {
            long leftFrequency, rightFrequency, goodNumberCount = 0;
            int maxTotal = (totalDigits / 2) * 9;
            for (int i = 1; i <= maxTotal; i++)
            {
                rightFrequency = GetFrequency(totalDigits / 2, i);
                leftFrequency = rightFrequency - GetFrequency((totalDigits / 2) - 1, i); // To remove leading zero numbers from count
                goodNumberCount += leftFrequency * rightFrequency;
            }
            return goodNumberCount;
        }

        /// <summary>
        /// Finds how many n-digit numbers have the sum of the digits equal to k.
        /// </summary>
        /// <param name="n">Length of numbers to consider.</param>
        /// <param name="k">Sum of digits to count frequency of.</param>
        /// <returns>The count of n-digit numbers with digits summing to k.</returns>
        /// <remarks>This leverages combinatorial math to avoid actually counting the answer.</remarks>
        static long GetFrequency(int n, int k)
        {
            // Note: The general formula for what we are trying to solve is:
            //    T(n,k) = Sum_{i = 0..floor(k/10)} (-1)^i * binomial(n,i) * binomial(n+k-1-10*i,n-1) for n >= 0 and 0 <= k <= 9*n

            // If you are above the maximum total, frequency must be zero
            if (k > n * 9)
            {
                return 0;
            }

            // Take advantage of symmetry to solve the simplest equivalent
            if (k > (n * 9) - k)
            {
                k = (n * 9) - k;
            }

            // Optimization for two digit frequency
            //    T(2,k) = Sum_{i = 0..floor(k/10)} (-1)^i*binomial(2,i)*binomial(1+k-10*i,1) for 0 <= k <= 18
            // Thanks to symmetry we only need to be able to solve 0 <= k <= 9, so floor(k/10) is aways zero, so we can do:
            //    T(2,k) = Sum_{i = 0..0} (-1)^i*binomial(2,i)*binomial(1+k-10*i,1) for 0 <= k <= 9
            // Which is a sum of one item, so simplify:
            //    T(2,k) = binomial(2,0) * binomial(1+k, 1) for 0 <= k <= 9
            // And binomial(2,0) = 1, so:
            //    T(2,k) = binomial(1+k, 1) for 0 <= k <= 9
            // And binomial(x,1) = x, so:
            //    T(2,k) = 1+k for 0 <= k <= 9
            if (n == 2)
            {
                return k + 1;
            }

            // Optimizatinon for three digit frequency
            //    T(3,k) = Sum_{i = 0..floor(k/10)} (-1)^i*binomial(3,i)*binomial(2+k-10*i,2) for 0 <= k <= 27
            // Thanks to symmetry, we only need 0 <= k <= 13, and we can split that into 0 <= k <= 9 and 10 <= k <= 13.
            // For the 0 <= k <= 9 portion, we have:
            //    T(3,k) = Sum_{i = 0..0} (-1)^i*binomial(3,i)*binomial(2+k-10*i,2) for 0 <= k <= 9
            // Which is a sum of only one item, so it can be further simplified as such:
            //    T(3,k) = binomial(2+k,2) for 0 <= k <= 9
            // And binomial(x, 2) is (x * (x - 1)) / 2, so we can simplify to:
            //    T(3,k) = ((k + 2) * (k + 1)) / 2 for 0 <= k <= 9
            // And for the series from 10 <= k <= 13, we have the sum of two items:
            //    T(3,k) = Sum_{i = 0..1} (-1)^i*binomial(3,i)*binomial(2+k-10*i,2) for 10 <= k <= 13
            // Which is:
            //    T(3,k) = (binomial(2 + k, 2)) + (-3 * binomial(2 + k - 10 * i, 2)) for 10 <= k <= 13
            // Which expands to:
            //    T(3,k) = ((k + 2) * (k + 1)) / 2 - (3 * (((k - 8) * (k - 9)) / 2)) for 10 <= k <= 13
            if (n == 3)
            {
                return k <= 9 ?
                    ((k + 2) * (k + 1)) / 2 :
                    ((k + 2) * (k + 1)) / 2 - (3 * (((k - 8) * (k - 9)) / 2));
            }

            // For n > 3 we fall back to the full implementation.
            long frequency = BinomialCoefficient(n + k - 1, n - 1);
            if (k > 9)
            {
                int sign = -1;
                for (int i = 1; i <= k / 10; i++)
                {
                    frequency += sign * BinomialCoefficient(n, i) * BinomialCoefficient(n + k - 1 - (10 * i), n - 1);
                    sign *= -1;
                }
            }
            return frequency;
        }

        /// <summary>
        /// Calculate binomial coefficient, which tells how many ways k items can be chosen from n items.
        /// </summary>
        /// <param name="n">Total items to choose from.</param>
        /// <param name="k">Number of items we want to choose.</param>
        /// <returns>Count of number of ways k items can be chosen from n items.</returns>
        /// <remarks>This leverages the binomial theorem in order to get the solution without having to actually count.</remarks>
        public static long BinomialCoefficient(int n, int k)
        {
            if (k > n) { return 0; }
            if (k == 0 || n == k) { return 1; }
            if (k > n - k) { k = n - k; } // Take advantage of symmetry
            long coefficient = n--;
            for (int i = 2; i <= k; i++)
            {
                coefficient *= n--;
                coefficient /= i;
            }
            return coefficient;
        }
    }
}
