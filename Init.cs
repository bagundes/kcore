namespace KCore
{
    public class Init : KCore.Base.IBaseInit
    {

        public bool Configure()
        {
            return true; // throw new NotImplementedException();
        }

        public bool Construct()
        {
            return true;
        }

        public bool Dependencies()
        {
            return true;// throw new NotImplementedException();
        }

        public bool Destruct()
        {
            return true;// throw new NotImplementedException();
        }

        public bool Populate()
        {
            return true; // throw new NotImplementedException();
        }

        public bool Register()
        {
            KCore.R.RegisterProject(R.ID, R.Project.Name);
            return true; // throw new NotImplementedException();
        }
    }
}
