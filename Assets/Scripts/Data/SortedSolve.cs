
namespace Kubewatch.Data
{
    public class SortedSolve
    {
        public int Index { get; set; }
        public Solve Solve { get; set; }

        public SortedSolve(int _index, Solve _solve)
        {
            Index = _index;
            Solve = _solve;
        }
    }
}