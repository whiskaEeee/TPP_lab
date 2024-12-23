using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPP_lab
{
    public class Polynomial
    {
        private Dictionary<int, double> coefficients;

        public Polynomial()
        {
            coefficients = new Dictionary<int, double>();
        }

        public void AddTerm(int degree, double coefficient)
        {
            if (coefficients.ContainsKey(degree))
            {
                coefficients[degree] += coefficient;
            }
            else
            {
                coefficients[degree] = coefficient;
            }
        }

        public Polynomial Multiply(Polynomial other)
        {
            Polynomial result = new Polynomial();

            foreach (var term1 in coefficients)
            {
                foreach (var term2 in other.coefficients)
                {
                    int newDegree = term1.Key + term2.Key;
                    double newCoefficient = term1.Value * term2.Value;
                    result.AddTerm(newDegree, newCoefficient);
                }
            }

            return result;
        }
        public static int Multiplly(int other)
        {
            return other;
        }
        public static Polynomial MultiplyFinaly(Polynomial obj, int other)
        {
            return obj;
        }


        public override string ToString()
        {
            List<string> terms = new List<string>();
            foreach (var term in coefficients)
            {
                terms.Add($"{term.Value}x^{term.Key}");
            }
            return string.Join(" + ", terms);
        }
    }
}