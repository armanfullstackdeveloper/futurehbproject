using NHibernate;
using NHibernate.Cfg;

namespace BusinessLogic.Helpers
{
    /// <summary>
    /// used for nhibernate
    /// </summary>
    public class NHibernateConfiguration
    {
        private static ISessionFactory _sessionFactory;
        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    var configuration = new Configuration();
                    configuration.Configure();
                    configuration.AddAssembly(System.Reflection.Assembly.Load("DataModel"));
                    _sessionFactory = configuration.BuildSessionFactory();
                }
                return _sessionFactory;
            }
        }
        public static ISession OpenSession()
        {
            try
            {
                return SessionFactory.OpenSession();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex.ToString());
            }
        }
    }
}
