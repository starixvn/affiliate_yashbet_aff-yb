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
    public class SubAffDomainServiceTest
    {
        [Fact]
        public void GetSubAffDomainList_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.GetDomainList(It.IsAny<string>())).Returns(new List<DomainInfo> { new DomainInfo() }).Callback<string>(p => spKey = p);

            var service = CreateSubAffDomainService(repository.Object);

            // Action
            var result = service.GetSubAffDomainList();

            // Assert
            Assert.Single(result);
            Assert.Equal("Aff_Admin_SubAffiliateDomain_Sel", spKey);
        }

        [Fact]
        public void InsertSubAffDomain_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.InsertDomain(It.IsAny<InsertUpdateDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam, string>((p, s) => spKey = s);

            var service = CreateSubAffDomainService(repository.Object);

            // Action
            var result = service.InsertSubAffDomain(new InsertUpdateDomainParam());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_SubAffiliateDomain_Ins", spKey);
        }

        [Fact]
        public void UpdateSubAffDomain_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.UpdateDomain(It.IsAny<InsertUpdateDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<InsertUpdateDomainParam, string>((p, s) => spKey = s);

            var service = CreateSubAffDomainService(repository.Object);

            // Action
            var result = service.UpdateSubAffDomain(new InsertUpdateDomainParam());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_SubAffiliateDomain_Upd", spKey);
        }

        [Fact]
        public void DeleteSubAffDomain_WhenCall_CallCorrect()
        {
            // Arrange
            var spKey = string.Empty;
            var repository = new Mock<IDomainRepository> { CallBase = true };
            repository.Setup(r => r.DeleteDomain(It.IsAny<DeleteDomainParam>(), It.IsAny<string>())).Returns(new ResultInfo()).Callback<DeleteDomainParam, string>((p, s) => spKey = s);

            var service = CreateSubAffDomainService(repository.Object);

            // Action
            var result = service.DeleteSubAffDomain(new DeleteDomainParam());

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.Equal("Aff_Admin_SubAffiliateDomain_Del", spKey);
        }

        private static SubAffDomainService CreateSubAffDomainService(IDomainRepository subAffDomainRepository = null)
        {
            var fakeSubAffDomainRepository = subAffDomainRepository ?? Mock.Of<IDomainRepository>();

            return new SubAffDomainService(fakeSubAffDomainRepository);
        }
    }
}
