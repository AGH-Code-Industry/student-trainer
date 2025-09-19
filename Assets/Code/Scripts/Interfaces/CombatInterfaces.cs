namespace Combat
{

    public interface IBlockingHandler
    {
        void HandleBlock();
    }

    public interface IDodgeHandler
    {
        void HandleDodge();
    }

}