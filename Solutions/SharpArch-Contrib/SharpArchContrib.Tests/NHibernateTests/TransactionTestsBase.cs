using System;
using System.Data;
using System.IO;
using System.Reflection;
using FluentNHibernate.Automapping;
using NHibernate.Tool.hbm2ddl;
using SharpArch.Core.PersistenceSupport;
using SharpArch.Data.NHibernate;
using SharpArch.Data.NHibernate.FluentNHibernate;
using SharpArchContrib.Data.NHibernate;
using Tests.DomainModel.Entities;

namespace Tests.NHibernateTests {
    public class TransactionTestsBase {
        protected IRepository<TestEntity> testEntityRepository;

        public void SetUp() {
            InitializeDatabase();
            InitializeData();
        }

        protected virtual void InitializeData() {
            testEntityRepository = new Repository<TestEntity>();
        }

        public void TearDown() {
            Shutdown();
        }

        private void InitializeDatabase() {
            if (File.Exists("db.dat")) {
                File.Delete("db.dat");
            }
            NHibernate.Cfg.Configuration cfg = InitializeNHibernateSession();
            IDbConnection connection = NHibernateSession.Current.Connection;
            new SchemaExport(cfg).Execute(false, true, false, connection, null);
        }

        private NHibernate.Cfg.Configuration InitializeNHibernateSession() {
            string[] mappingAssemblies = GetMappingAssemblies();
            AutoPersistenceModel autoPersistenceModel = GetAutoPersistenceModel(mappingAssemblies);
            return NHibernateSession.Init(new ThreadSessionStorage(), mappingAssemblies, autoPersistenceModel,
                                          "HibernateFile.cfg.xml");
        }

        private string[] GetMappingAssemblies() {
            return new[] {"SharpArchContrib.Tests"};
        }

        private AutoPersistenceModel GetAutoPersistenceModel(string[] assemblies) {
            foreach (string asmName in assemblies) {
                Assembly asm = Assembly.Load(asmName);
                Type[] asmTypes = asm.GetTypes();

                foreach (Type asmType in asmTypes) {
                    if (typeof(IAutoPersistenceModelGenerator).IsAssignableFrom(asmType)) {
                        var generator = Activator.CreateInstance(asmType) as IAutoPersistenceModelGenerator;
                        return generator.Generate();
                    }
                }
            }

            return null;
        }

        private void Shutdown() {
            NHibernateSession.CloseAllSessions();
            NHibernateSession.Reset();
        }
    }
}