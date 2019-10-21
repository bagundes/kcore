namespace KCore.Base
{
    /// <summary>
    /// Interface to init script the project. The Kcore will execute in this order:
    /// 10 - Construct
    /// 20 - Populate
    /// 30 - Configure
    /// 40 - Register
    /// 90 - Destruct
    /// </summary>
    public interface IBaseInit
    {
        bool Dependencies();
        bool Construct();
        bool Configure();
        bool Populate();
        bool Destruct();
        bool Register();
    }
}
