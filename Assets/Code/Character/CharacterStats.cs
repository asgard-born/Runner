namespace Character
{
    public class CharacterStats
    {
        private float _speed;
        private float _jumpForce;
        
        public float speed => _speed;
        public float jumpForce => _jumpForce;

        public CharacterStats(float speed, float jumpForce)
        {
            _speed = speed;
            _jumpForce = jumpForce;
        }
    }
}