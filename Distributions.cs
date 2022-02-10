using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BDSP_Randomizer
{
    /// <summary>
    ///  Container for classes handling discrete probability distributions.
    /// </summary>
    public static class Distributions
    {
        private static Random rnd = new();

        /// <summary>
        ///  Converts a uniform distribution 0-1 to an approximated normal distribution.
        /// </summary>
        private static double QuantileFunction(double p, double mean, double std)
        {
            if (p >= 1)
                return double.PositiveInfinity;
            if (p <= 0)
                return double.NegativeInfinity;
            return mean + Math.Sqrt(2) * std * InvErf(2 * p - 1);
        }

        private static double InvErf(double z)
        {
            double sum = 0;
            double precision = 0.000000001;
            for (int k = 0; true; k++)
            {
                if (k == c.Count)
                    c.Add(C(k));
                if (double.IsInfinity(c[k]))
                    break;
                double add = c[k] / (2 * k + 1) * Math.Pow(Math.Sqrt(Math.PI) / 2 * z, 2 * k + 1);
                if (Math.Abs(add) < precision)
                    break;
                sum += add;
            }
            return sum;
        }

        private static double C(double k)
        {
            if (k == 0)
                return 1;
            double sum = 0;
            for (int m = 0; m <= k - 1; m++)
                sum += c[(int)(k - 1 - m)] / (m + 1)  / (2 * m + 1) * c[m];
            return sum;
        }

        private static List<double> c = new();
        public static readonly string[] numericDistributionNames = new string[]
        {
            "Uniform, Constant",
            "Uniform, Relative",
            "Uniform, Proportional",
            "Normal, Constant",
            "Normal, Relative",
            "Normal, Proportional"
        };
        public static readonly string[] itemDistributionNames = new string[]
        {
            "Empirical",
            "Uniform"
        };
        public static readonly string[] numericDistributionDescriptions = new string[]
        {
            "Selects all values using the\nsame boundaries:\nY ~ U(min, max)",
            "Selects all values using\nrelative boundaries:\nY ~ U(x + min, x + max)",
            "Selects all values using\nproportionally defined\nboundaries:\nY ~ U(x * min, x * max)",
            "Selects all values using the\nsame mean and standard\ndeviation:\nY ~ N(mean, std)",
            "Selects all values using the\nsame standard deviation:\nY ~ N(x, std)",
            "Selects all values using\nproportionally defined\nstandard deviations:\nY ~ N(x, std * x)",
        };
        public static readonly string[] itemDistributionDescriptions = new string[]
        {
            "Selects values based on\ntheir weight value. Larger\nweight implies higher\nfrequency.",
            "Selects values from the\nspecified sample.\nConfigure checkboxes to\ncontrol inclusion."
        };
        public static readonly (string, string, string)[] numericDistributionArgNames = new (string, string, string)[]
        {
            ("Randomization Probability", "Min", "Max"),
            ("Randomization Probability", "Min", "Max"),
            ("Randomization Probability", "Min", "Max"),
            ("Randomization Probability", "Mean", "Standard Deviation"),
            ("Randomization Probability", "Standard Deviation", ""),
            ("Randomization Probability", "Standard Deviation", "")
        };

        /// <summary>
        ///  Converts a string representation into a IDistribution object.
        /// </summary>
        public static IDistribution Parse(string s)
        {
            Regex wordRegex = new(@"\A[A-Z]+");
            Regex commaRegex = new(@"\A,");
            double p = 0;
            string pRemoved = ExtractDouble(s, out p).ToUpper();
            if (!wordRegex.IsMatch(pRemoved))
                return null;
            string dString = wordRegex.Matches(pRemoved).Aggregate((a, m) => a.Value.Length < m.Value.Length ? m : a).Value;
            string dRemoved = pRemoved.Substring(dString.Length, pRemoved.Length - dString.Length);
            switch (dString)
            {
                case "UC":
                    double ucMin = 0;
                    string ucMinRemoved = ExtractDouble(dRemoved, out ucMin);
                    if (!commaRegex.IsMatch(ucMinRemoved))
                        return null;
                    ucMinRemoved = ucMinRemoved.Substring(1);
                    double ucMax = 0;
                    if (ExtractDouble(ucMinRemoved, out ucMax).Length != 0)
                        return null;
                    if (double.IsNaN(ucMin) || double.IsNaN(ucMax) || ucMin > ucMax)
                        return null;
                    return new UniformConstant(p, ucMin, ucMax);
                case "UR":
                    double urMin = 0;
                    string urMinRemoved = ExtractDouble(dRemoved, out urMin);
                    if (!commaRegex.IsMatch(urMinRemoved))
                        return null;
                    urMinRemoved = urMinRemoved.Substring(1);
                    double urMax = 0;
                    if (ExtractDouble(urMinRemoved, out urMax).Length != 0)
                        return null;
                    if (double.IsNaN(urMin) || double.IsNaN(urMax) || urMin > urMax)
                        return null;
                    return new UniformRelative(p, urMin, urMax);
                case "UP":
                    double upMin = 0;
                    string upMinRemoved = ExtractDouble(dRemoved, out upMin);
                    if (!commaRegex.IsMatch(upMinRemoved))
                        return null;
                    upMinRemoved = upMinRemoved.Substring(1);
                    double upMax = 0;
                    if (ExtractDouble(upMinRemoved, out upMax).Length != 0)
                        return null;
                    if (double.IsNaN(upMin) || double.IsNaN(upMax) || upMin > upMax)
                        return null;
                    return new UniformProportional(p, upMin, upMax);
                case "NC":
                    double ncMean = 0;
                    string ncMeanRemoved = ExtractDouble(dRemoved, out ncMean);
                    if (!commaRegex.IsMatch(ncMeanRemoved))
                        return null;
                    ncMeanRemoved = ncMeanRemoved.Substring(1);
                    double ncStd = 0;
                    if (ExtractDouble(ncMeanRemoved, out ncStd).Length != 0)
                        return null;
                    if (double.IsNaN(ncMean) || double.IsNaN(ncStd))
                        return null;
                    return new NormalConstant(p, ncMean, ncStd);
                case "NR":
                    double nrStd = 0;
                    if (ExtractDouble(dRemoved, out nrStd).Length != 0)
                        return null;
                    if (double.IsNaN(nrStd))
                        return null;
                    return new NormalRelative(p, nrStd);
                case "NP":
                    double npStd = 0;
                    if (ExtractDouble(dRemoved, out npStd).Length != 0)
                        return null;
                    if (double.IsNaN(npStd))
                        return null;
                    return new NormalProportional(p, npStd);
                default:
                    return null;
            }
        }

        /// <summary>
        ///  Removes a double from the start of a string.
        /// </summary>
        private static string ExtractDouble(string input, out double d)
        {
            Regex doubleRegex = new(@"\A\-?(\d+(\.\d+)?(E(\-|\+)\d+)?|∞)");
            if (!doubleRegex.IsMatch(input))
            {
                d = double.NaN;
                return input;
            }
            string dString = doubleRegex.Matches(input).Aggregate((a, m) => a.Value.Length < m.Value.Length ? m : a).Value;
            d = double.Parse(dString);
            return input.Substring(dString.Length, input.Length - dString.Length);
        }

        /// <summary>
        ///  Returns a distribution object with the specified config.
        /// </summary>
        public static IDistribution CreateDistribution(List<double> args)
        {
            switch (args[0])
            {
                case 0:
                    return new UniformConstant(args[1], args[2], args[3]);
                case 1:
                    return new UniformRelative(args[1], args[2], args[3]);
                case 2:
                    return new UniformProportional(args[1], args[2], args[3]);
                case 3:
                    return new NormalConstant(args[1], args[2], args[3]);
                case 4:
                    return new NormalRelative(args[1], args[2]);
                case 5:
                    return new NormalProportional(args[1], args[2]);
                case 6:
                    List<int> weights = args.Skip(2).Select(d => (int)d).ToList();
                    return new Empirical(args[1], weights);
                case 7:
                    List<bool> selection = args.Skip(2).Select(d => d == 1).ToList();
                    return new UniformSelection(args[1], selection);
                default:
                    return null;
            }
        }

        /// <summary>
        ///  Calculates the standard deviation of a sequence of double values.
        /// </summary>
        public static double StandardDeviation(this IList<double> observations)
        {
            double sum = 0;
            for (int i = 0; i < observations.Count; i++)
                sum += Math.Pow(observations.Average() - observations[i], 2) / (observations.Count - 1);
            return Math.Sqrt(sum);
        }

        /// <summary>
        ///  Calculates the standard deviation of a sequence of integer values.
        /// </summary>
        public static double StandardDeviation(this IList<int> observations)
        {
            return observations.Select(i => (double)i).ToList().StandardDeviation();
        }

        public interface IDistribution
        {
            /// <summary>
            ///  Returns an unbounded random integer by this class' distribution.
            /// </summary>
            public int Next(int value);

            /// <summary>
            ///  Returns a random integer by this class' distribution, bounded by min and max.
            /// </summary>
            public int Next(int value, int min, int max);

            /// <summary>
            ///  Returns a random double by this class' distribution, bounded by min and max.
            /// </summary>
            public double NextDouble(double value, double min, double max);

            /// <summary>
            ///  Returns the distribution represented as a string.
            /// </summary>
            public string GetString();

            /// <summary>
            ///  Returns the config of the distribution as a List of doubles.
            /// </summary>
            public List<double> GetConfig();
        }

        public class UniformConstant : IDistribution
        {
            private double p;
            private double min;
            private double max;

            public UniformConstant(double p, double min, double max)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.min = Math.Round(Math.Min(min, max), 3);
                this.max = Math.Round(Math.Max(min, max), 3);
            }

            public int Next(int value)
            {
                return rnd.NextDouble() * 100 < p ? rnd.Next((int)min, (int)max + 1) : value;
            }

            public int Next(int value, int min, int max)
            {
                return Math.Clamp(Next(value), min, max);
            }

            public double NextDouble(double value, double min, double max)
            {
                return Math.Clamp(rnd.NextDouble() * 100 < p ? rnd.NextDouble() * (this.max - this.min) + this.min : value, min, max);
            }

            public string GetString()
            {
                return p + "UC" + min + "," + max;
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(0);
                l.Add(p);
                l.Add(min);
                l.Add(max);
                return l;
            }
        }

        public class UniformRelative : IDistribution
        {
            private double p;
            private double min;
            private double max;

            public UniformRelative(double p, double min, double max)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.min = Math.Round(Math.Min(min, max), 3);
                this.max = Math.Round(Math.Max(min, max), 3);
            }

            public int Next(int value)
            {
                return rnd.NextDouble() * 100 < p ? value + rnd.Next((int)min, (int)max + 1) : value;
            }

            public int Next(int value, int min, int max)
            {
                return Math.Clamp(Next(value), min, max);
            }

            public double NextDouble(double value, double min, double max)
            {
                return Math.Clamp(rnd.NextDouble() * 100 < p ? value + rnd.NextDouble() * (this.max - this.min) + this.min : value, min, max);
            }

            public string GetString()
            {
                return p + "UR" + min + "," + max;
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(1);
                l.Add(p);
                l.Add(min);
                l.Add(max);
                return l;
            }
        }

        public class UniformProportional : IDistribution
        {
            private double p;
            private double minX;
            private double maxX;

            public UniformProportional(double p, double minX, double maxX)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.minX = Math.Round(Math.Min(minX, maxX), 3);
                this.maxX = Math.Round(Math.Max(minX, maxX), 3);
            }

            public int Next(int value)
            {
                return rnd.NextDouble() * 100 < p ? rnd.Next((int)(minX * value), (int)(maxX * value) + 1) : value;
            }

            public int Next(int value, int min, int max)
            {
                return Math.Clamp(Next(value), min, max);
            }

            public double NextDouble(double value, double min, double max)
            {
                return Math.Clamp(rnd.NextDouble() * 100 < p ? value * (rnd.NextDouble() * (maxX - minX) + minX) : value, min, max);
            }

            public string GetString()
            {
                return p + "UP" + minX + "," + maxX;
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(2);
                l.Add(p);
                l.Add(minX);
                l.Add(maxX);
                return l;
            }
        }

        public class NormalConstant : IDistribution
        {
            private double p;
            private double mean;
            private double standardDeviation;

            public NormalConstant(double p, double mean, double standardDeviation)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.mean = Math.Round(mean, 3);
                this.standardDeviation = Math.Round(standardDeviation, 3);
            }

            public int Next(int value)
            {
                return rnd.NextDouble() * 100 < p ? (int)Math.Round(QuantileFunction(rnd.NextDouble(), mean, standardDeviation)) : value;
            }

            public int Next(int value, int min, int max)
            {
                return Math.Clamp(Next(value), min, max);
            }

            public double NextDouble(double value, double min, double max)
            {
                return Math.Clamp(rnd.NextDouble() * 100 < p ? QuantileFunction(rnd.NextDouble(), mean, standardDeviation) : value, min, max);
            }

            public string GetString()
            {
                return p + "NC" + mean + "," + standardDeviation;
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(3);
                l.Add(p);
                l.Add(mean);
                l.Add(standardDeviation);
                return l;
            }
        }

        public class NormalRelative : IDistribution
        {
            private double p;
            private double standardDeviation;

            public NormalRelative(double p, double standardDeviation)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.standardDeviation = Math.Round(standardDeviation, 3);
            }

            public int Next(int value)
            {
                return rnd.NextDouble() * 100 < p ? (int)Math.Round(QuantileFunction(rnd.NextDouble(), value, standardDeviation)) : value;
            }

            public int Next(int value, int min, int max)
            {
                return Math.Clamp(Next(value), min, max);
            }

            public double NextDouble(double value, double min, double max)
            {
                return Math.Clamp(rnd.NextDouble() * 100 < p ? QuantileFunction(rnd.NextDouble(), value, standardDeviation) : value, min, max);
            }

            public string GetString()
            {
                return p + "NR" + standardDeviation;
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(4);
                l.Add(p);
                l.Add(standardDeviation);
                return l;
            }
        }

        public class NormalProportional : IDistribution
        {
            private double p;
            private double standardDeviationX;

            public NormalProportional(double p, double standardDeviationX)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.standardDeviationX = Math.Round(standardDeviationX, 3);
            }

            public int Next(int value)
            {
                return rnd.NextDouble() * 100 < p ? (int)Math.Round(QuantileFunction(rnd.NextDouble(), value, standardDeviationX * value)) : value;
            }

            public int Next(int value, int min, int max)
            {
                return Math.Clamp(Next(value), min, max);
            }

            public double NextDouble(double value, double min, double max)
            {
                return Math.Clamp(rnd.NextDouble() * 100 < p ? QuantileFunction(rnd.NextDouble(), value, standardDeviationX * value) : value, min, max);
            }

            public string GetString()
            {
                return p + "NP" + standardDeviationX;
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(5);
                l.Add(p);
                l.Add(standardDeviationX);
                return l;
            }
        }

        public class Empirical : IDistribution
        {
            private double p;
            private List<int> weights;
            private int totalWeight;

            public Empirical(double p, List<int> weights)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.weights = weights;
                totalWeight = 0;
                for (int i = 0; i < weights.Count; i++)
                {
                    weights[i] = Math.Max(weights[i], 0);
                    totalWeight += weights[i];
                }
                if (totalWeight == 0)
                    this.p = 0;
            }

            public int Next(int value)
            {
                if (rnd.NextDouble() * 100 >= p)
                    return value;

                int n = rnd.Next(totalWeight);
                int item = -1;
                for (int i = 0; n >= 0; i++)
                {
                    item = i;
                    n -= Math.Max(weights[i], 0);
                }
                return item;   
            }

            public int Next(int value, int min, int max)
            {
                throw new NotImplementedException();
            }

            public double NextDouble(double value, double min, double max)
            {
                throw new NotImplementedException();
            }

            public string GetString()
            {
                throw new NotImplementedException();
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(6);
                l.Add(p);
                l.AddRange(weights.Select(i => (double)i));
                return l;
            }
        }

        public class UniformSelection : IDistribution
        {
            private double p;
            private List<bool> selection;
            private int selectionCount;

            public UniformSelection(double p, List<bool> selection)
            {
                this.p = Math.Clamp(p, 0, 100);
                this.selection = selection;
                selectionCount = 0;
                for (int i = 0; i < selection.Count; i++)
                    if (selection[i])
                        selectionCount++;
                if (selectionCount == 0)
                    this.p = 0;
            }

            public int Next(int value)
            {
                if (rnd.NextDouble() * 100 >= p)
                    return value;

                int n = rnd.Next(selectionCount);
                int item = -1;
                for (int i = 0; n >= 0; i++)
                {
                    item = i;
                    if (selection[i])
                        n--;
                }
                return item;
            }

            public int Next(int value, int min, int max)
            {
                throw new NotImplementedException();
            }

            public double NextDouble(double value, double min, double max)
            {
                throw new NotImplementedException();
            }

            public string GetString()
            {
                throw new NotImplementedException();
            }

            public List<double> GetConfig()
            {
                List<double> l = new();
                l.Add(7);
                l.Add(p);
                l.AddRange(selection.Select(b => b ? 1.0 : 0.0));
                return l;
            }
        }
    }
}