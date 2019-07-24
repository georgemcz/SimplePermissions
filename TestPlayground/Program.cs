using System.Collections.Generic;
using System.Linq;
using Simple.Permissions;

namespace Permissions.TestPlayground
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialize permission engine
            var perm = new PermissionEngine<ulong>(new MembershipProvider(), new AclProvider());

            // login the user somehow and then you can start session for the user
            var user = perm.StartSession(47);

            // query directly for Id
            bool has = user.HasRight(800ul, Right.View);

            // query by object instance
            var article = new CrmArticle(800);
            has = user.HasRight(article, Right.View);

            // filter list
            var articles = new List<CrmArticle> {new CrmArticle(801), new CrmArticle(802), article};
            var granter = articles.Where(a => user.HasRight(a, Right.View)).ToArray();
        }
    }

    class Person : IObjectIdentity<ulong>
    {
        public Person(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; }
    }

    public class CrmArticle : IObjectIdentity<ulong>
    {
        public CrmArticle(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; }
    }

    class MembershipProvider : SimplifiedMembershipProvider<ulong>
    {
        protected override IEnumerable<IObjectIdentity<ulong>> GetParents(ulong objectId)
        {
            // simulate object 47 and its parent 77
            if (objectId == 47)
            {
                yield return new Person(47);
                yield return new Person(77);
            }

            if (objectId == 800)
            {
                yield return new CrmArticle(800);
                yield return new CrmArticle(808);
            }
        }
    }

    class AclProvider : IAclProvider<ulong>
    {
        public Acl<ulong> GetById(ulong objectId)
        {
            if (objectId == 808) // parent of crm article
            {
                var acl = new Acl<ulong>();
                acl.Add(new Allow<ulong>(77, Right.View)); // add parent 77 access to read parent of crm articles
                return acl;
            }
            
            return new Acl<ulong>();
        }
    }
}
