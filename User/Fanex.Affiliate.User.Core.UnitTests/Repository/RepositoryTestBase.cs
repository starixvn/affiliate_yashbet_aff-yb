using Fanex.Affiliate.User.Core.Repository;
using Fanex.Data;
using NSubstitute;
using System.Data;
using System.Reflection;

namespace Fanex.Affiliate.User.Core.UnitTests.Repository
{
    public class RepositoryTestBase<T>
        where T : class
    {
        protected readonly IObjectDBBuilder Builder;

        protected RepositoryTestBase()
        {
            Builder = Substitute.For<IObjectDBBuilder>();
            var db = Substitute.For<IObjectDb>();
            var reader = Substitute.For<IDataReader>();
            var multipleReader = Substitute.For<IMultiResultReader>();
            Builder.Build(null, null).ReturnsForAnyArgs(c =>
            {
                var spKey = (c.Args()[0] as string) + (c.Args()[1] as string);
                db.SettingKey.Returns(spKey);
                return db;
            });
            db.ExecuteReader().ReturnsForAnyArgs(reader);
            db.QueryMultiple().ReturnsForAnyArgs(multipleReader);

        }

        protected static void AssignObjectToReader(IDataReader reader, object obj)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.CanWrite)
                {
                    reader[prop.Name].Returns(prop.GetValue(obj));
                }
            }
        }

        protected static void SetObjectPropetyValue(object obj, string propetyName, object value)
        {
            var type = obj.GetType();
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.Name == propetyName)
                {
                    if (prop.CanWrite)
                    {
                        prop.SetValue(obj, value);
                    }
                    else
                    {
                        string name = $"<{prop.Name}>i__Field";
                        type.GetField(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, value);
                    }
                }
            }
        }

        protected static bool CheckPropertyValue(object obj, string propetyName, object value)
        {
            var props = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.Name == propetyName)
                {
                    return value.Equals(prop.GetValue(obj, null));
                }
            }
            return false;
        }

        protected virtual T CreateRepo()
            => typeof(T).Assembly.CreateInstance(typeof(T).FullName, true, BindingFlags.CreateInstance, null, new[] { Builder }, null, null) as T;
    }
}
