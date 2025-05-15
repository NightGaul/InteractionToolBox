using UnityEngine;

namespace Code.Scrips.DrawAndCover
{
    public class CoverageGrid
    {
        private int _width;
        private int _height;
        private bool[,] _gridCoverage;
        public BrushShape brushshape;

        public CoverageGrid(int w, int h)
        {
            _width = w;
            _height = h;
            _gridCoverage = new bool[w, h];
        }

        public void MarkCoverage(int x, int y, int radius)
        {
            switch (brushshape)
            {
                case BrushShape.DIAMOND:
                    for (int dx = -radius; dx <= radius; dx++)
                    {
                        int maxDy = radius - Mathf.Abs(dx);
                        for (int dy = -maxDy; dy <= maxDy; dy++)
                        {
                            int newX = x + dx;
                            int newY = y + dy;

                            //check bounds
                            if (newX >= 0 && newX < _gridCoverage.GetLength(0) &&
                                newY >= 0 && newY < _gridCoverage.GetLength(1))
                            {
                                _gridCoverage[newX, newY] = true;
                            }
                        }
                    }

                    break;
                case BrushShape.SQUARE:
                    for (int i = x - radius; i <= x + radius; i++)
                    {
                        for (int j = y - radius; j <= y + radius; j++)
                        {
                            if (i >= 0 && i < _gridCoverage.GetLength(0) &&
                                j >= 0 && j < _gridCoverage.GetLength(1))
                            {
                                _gridCoverage[i, j] = true;
                            }
                        }
                    }

                    break;
                case BrushShape.CIRCLE:
                    int radiusSquared = radius * radius;

                    for (int dx = -radius; dx <= radius; dx++)
                    {
                        for (int dy = -radius; dy <= radius; dy++)
                        {
                            // check if inside circle
                            if (dx * dx + dy * dy <= radiusSquared)
                            {
                                int newX = x + dx;
                                int newY = y + dy;

                                // optional bounds check
                                if (newX >= 0 && newX < _gridCoverage.GetLength(0) &&
                                    newY >= 0 && newY < _gridCoverage.GetLength(1))
                                {
                                    _gridCoverage[newX, newY] = true;
                                }
                            }
                        }
                    }

                    break;
                case BrushShape.BAR:
                    var height = Mathf.FloorToInt(radius / 4);
                    for (int i = x - radius; i <= x + radius; i++)
                    {
                        for (int j = y - height; j <= y + height; j++)
                        {
                            if (i >= 0 && i < _gridCoverage.GetLength(0) &&
                                j >= 0 && j < _gridCoverage.GetLength(1))
                            {
                                _gridCoverage[i, j] = true;
                            }
                        }
                    }

                    break;
            }
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
                    if (IsCovered(i, j)) coverageInPercent++;
                }
            }
            
            return coverageInPercent / (_height * _width / 100f);
        }
    }
}