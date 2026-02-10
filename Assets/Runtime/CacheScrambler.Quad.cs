public static partial class CacheScrambler
{
    /// <summary>
    /// Simple quad map for storing data without arrays.
    /// </summary>
    public sealed class Quad
    {
        public readonly object a1, a2, b1, b2;
        public Quad(object a1, object a2, object b1, object b2)
        {
            this.a1 = a1;
            this.a2 = a2;
            this.b1 = b1;
            this.b2 = b2;
        }

        public Quad(int depth)
        {
            if (depth <= 1)
            {
                a1 = new object();
                a2 = new object();
                b1 = new object();
                b2 = new object();
            }
            else
            {
                a1 = new Quad(depth - 1);
                a2 = new Quad(depth - 1);
                b1 = new Quad(depth - 1);
                b2 = new Quad(depth - 1);
            }
        }
    }
}
