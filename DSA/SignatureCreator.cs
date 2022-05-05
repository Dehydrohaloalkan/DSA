using System.Numerics;

namespace DSA
{
    public class SignatureCreator
    {
        public BigInteger Q { get; set; }
        public BigInteger P { get; set; }
        public BigInteger H { get; set; }
        public BigInteger X { get; set; }

        private BigInteger G { get; set; }
        private BigInteger Y { get; set; }

        public SignatureCreator(BigInteger q, BigInteger p, BigInteger h, BigInteger x)
        {
            if (!IsPrime(q))
                throw new LogicException("q должно быть простым числом");
            Q = q;

            if (!IsPrime(p))
                throw new LogicException("p должно быть простым числом");
            if ((p - 1) % q != 0)
                throw new LogicException("q должно являться делителем (p - 1)");
            P = p;

            if (h <= 1 || h >= p - 1)
                throw new LogicException("h должно быть в интервале от 1 до (p - 1)");
            H = h;

            G = Pow(h, (p - 1) / q, p);
            if (G <= 1)
                throw new LogicException("h образует g меньше или равное 1");

            if (x <= 0 || x >= q)
                throw new LogicException("x должно быть в интервале от 0 до q");
            X = x;

            Y = Pow(G, x, p);
        }

        public (byte[] data, int hash, int r, int s) Create(byte[] message, int k)
        {
            BigInteger hash = GetHash(message);

            if (k <= 0 && k >= Q)
                throw new LogicException("k должно быть в интервале от 0 до q");
            
            var r = Pow(G, k, P) % Q;
            var s = ((hash + X * r) * Pow(k, Q-2, Q)) % Q;

            if (s == 0)
                throw new LogicException("s получилось равным 0");
            if (r == 0)
                throw new LogicException("r получилось равным 0");

            return (TextAnalyzer.Concatenate(message, (int)r, (int)s), (int)hash, (int)r, (int)s);
        }

        public (int v, int r, int s) Check(byte[] data)
        {
            var (message, r, s) = TextAnalyzer.Analyze(data);

            var hash = GetHash(message);

            var w = Pow(s, Q - 2, Q);
            var u1 = (hash * w) % Q;
            var u2 = (r * w) % Q;
            var v = ((Pow(G, u1, P) * Pow(Y, u2, P)) % P) % Q;

            return ((int)v, r, s);
        }

        private BigInteger GetHash(byte[] data)
        {
            BigInteger h = 100;

            foreach (var b in data)
            {
                h = Pow(h + b, 2, Q);
            }

            return h;
        }

        private static bool IsPrime(BigInteger number)
        {
            if (number == 1) return false;
            if (number == 2) return true;

            var limit = Math.Ceiling(Math.Sqrt((int)number));

            for (var i = 2; i <= limit; ++i)
                if (number % i == 0)
                    return false;
            return true;
        }

        private static BigInteger Pow(BigInteger value, BigInteger power, BigInteger mod)
        {
            BigInteger a1 = value;
            BigInteger z1 = power;
            BigInteger x = 1;
            while (z1 != 0)
            {
                while ((z1 % 2) == 0)
                {
                    z1 /= 2;
                    a1 = (a1 * a1) % mod;
                }

                z1--;
                x = (x * a1) % mod;
            }

            return x;
        }
    }
}
