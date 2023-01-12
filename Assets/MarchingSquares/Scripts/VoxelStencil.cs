namespace MarchingSquares.Scripts
{
    public class VoxelStencil
    {
        private bool _fillType;
        private int _centerX, _centerY, _radius;
        
        public int XStart => _centerX - _radius;

        public int XEnd => _centerX + _radius;

        public int YStart => _centerY - _radius;

        public int YEnd => _centerY + _radius;

        public void Initialize(bool fillType, int radius)
        {
            _fillType = fillType;
            _radius = radius;
        }
        public bool Apply(int x, int y)
        {
            return true;
        }
        
        public void SetCenter(int x, int y)
        {
            _centerX = x;
            _centerY = y;
        }
    }
}