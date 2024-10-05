namespace Salems_Draw
{
    public interface IMover
    {
        public bool CanMove { set; }

        public void SetSpeed(float speed);
    }
}
