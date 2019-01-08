using System;
using System.Security.Permissions;

namespace System.Security
{
    abstract public class CodeAccessPermission : IPermission
    {
        abstract public SecurityElement ToXml();
        abstract public void FromXml(SecurityElement elem);
        public abstract IPermission Copy();
        public abstract IPermission Intersect(IPermission target);
        public abstract bool IsSubsetOf(IPermission target);
        public virtual IPermission Union(IPermission other)
        {
            return null;
        }
        internal static PermissionState CheckPermissionState (PermissionState state, bool allowUnrestricted)
        {
            return PermissionState.Unrestricted;
        }
        internal static int CheckSecurityElement (SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
        {
            return 0;
        }
        internal static void ThrowInvalidPermission (IPermission target, Type expected)
        {
        }
        internal static bool IsUnrestricted (SecurityElement se)
        {
            return true;
        }
        internal SecurityElement Element (int version)
        {
            return null;
        }
        public void Demand ()
        {
        }
    }
}