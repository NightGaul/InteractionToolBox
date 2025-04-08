namespace Code.Scrips
{
    public class CoverageGrid
    {
        private int _width;
        private int _height;
        private bool[,] _gridCoverage;

        public CoverageGrid(int w, int h)
        {
            _width = w;
            _height = h;
            _gridCoverage = new bool[w, h];
        }

        public void MarkCoverage(int x, int y)
        {
            _gridCoverage[x, y] = true;
        }

        public bool IsCovered(int x, int y)
        {
            return _gridCoverage[x, y];
        }

        public bool[,] GetGridCoverage()
        {
            return _gridCoverage;
        }
        public float GetCoveragePercent()
        {
            var coverageInPercent = 0;
            for (int i = 0; i < _gridCoverage.GetLength(0); i++)
            {
                for (int j = 0; j < _gridCoverage.GetLength(1); j++)
                {
                    if (IsCovered(i,j)) coverageInPercent++;
                }
            }
            return coverageInPercent / 100f;
        }
    }
}
