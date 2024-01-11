using Fanex.Affiliate.Admin.Core.DTO;
using Fanex.Affiliate.Admin.Core.DTO.Domain;
using Fanex.Affiliate.Admin.Core.MetaModels.Domain;
using Fanex.Affiliate.Admin.Core.Repository.Domain;
using Fanex.Affiliate.Admin.Core.Services.Domain;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Fanex.Affiliate.Admin.Core.UnitTests.Services.Domain
{
    public class DomainServiceTest
    {
        [Fact]
        public void GetDomainList_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.GetDomainList(It.IsAny<string>())).Returns(new List<DomainInfo> { new DomainInfo() }).Callback<string>(p => spKey = p);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.GetDomainList(true);

            // Assert
            Assert.Single(result);
            Assert.Equal("Aff_Admin_AffiliateDomain_Sel", spKey);
        }

        [Fact]
        public void GetDomainList_WhenCallWithFalse_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.GetDomainList(It.IsAny<string>())).Returns(new List<DomainInfo> { new DomainInfo() }).Callback<string>(p => spKey = p);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.GetDomainList(false);

            // Assert
            Assert.Single(result);
            Assert.Equal("Aff_Admin_LicenseeDomain_Sel", spKey);
        }

        [Fact]
        public void InsertDomain_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.InsertDomain(It.IsAny<InsertUpdateDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam, string>((p, s) => spKey = s);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.InsertDomain(new InsertUpdateDomainParam { IsAffDomain = true });

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_AffiliateDomain_Ins", spKey);
        }

        [Fact]
        public void InsertDomain_WhenCallWithFalse_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.InsertDomain(It.IsAny<InsertUpdateDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam, string>((p, s) => spKey = s);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.InsertDomain(new InsertUpdateDomainParam());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_LicenseeDomain_Ins", spKey);
        }

        [Fact]
        public void UpdateDomain_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.UpdateDomain(It.IsAny<InsertUpdateDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam, string>((p, s) => spKey = s);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.UpdateDomain(new InsertUpdateDomainParam { IsAffDomain = true });

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_AffiliateDomain_Upd", spKey);
        }

        [Fact]
        public void UpdateDomain_WhenCallWithFalse_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.UpdateDomain(It.IsAny<InsertUpdateDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam, string>((p, s) => spKey = s);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.UpdateDomain(new InsertUpdateDomainParam());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_LicenseeDomain_Upd", spKey);
        }

        [Fact]
        public void DeleteDomain_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.DeleteDomain(It.IsAny<DeleteDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<DeleteDomainParam, string>((p, s) => spKey = s);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.DeleteDomain(new DeleteDomainParam { IsAffDomain = true });

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_AffiliateDomain_Del", spKey);
        }

        [Fact]
        public void DeleteDomain_WhenCallWithFalse_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.DeleteDomain(It.IsAny<DeleteDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<DeleteDomainParam, string>((p, s) => spKey = s);

            var service = CreateDomainService(repository.Object);

            // Action
            var result = service.DeleteDomain(new DeleteDomainParam());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_LicenseeDomain_Del", spKey);
        }

        private static DomainService CreateDomainService(IDomainRepository domainRepository = null)
        {
            var fakeDomainRepository = domainRepository ?? Mock.Of<IDomainRepository>();

            return new DomainService(fakeDomainRepository);
        }
    }
}
