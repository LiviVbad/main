using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using AppFrameworkDemo.Authorization.Users;
using AppFrameworkDemo.MultiTenancy;

namespace AppFrameworkDemo.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}