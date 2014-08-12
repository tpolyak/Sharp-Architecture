namespace SharpArch.Domain
{
    public class Enums
    {
        /// <summary>
        ///     Provides an NHibernate.LockMode facade so as to avoid a direct dependency on the NHibernate DLL.
        ///     Further information concerning lockmodes may be found at:
        ///     http://www.hibernate.org/hib_docs/nhibernate/html_single/#transactions-locking
        /// </summary>
        public enum LockMode
        {
            None, 
            Read, 
            Upgrade, 
            UpgradeNoWait, 
            Write
        }
    }
}