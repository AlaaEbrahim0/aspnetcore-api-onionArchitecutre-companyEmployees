using System.Diagnostics.Contracts;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Contracts;
using Entities;
using Entities.Exceptions;
using Moq;
using Services;
using Shared.DTOs;
using Shared.RequestFeatures;

namespace CompanyEmployees.Tests;

public class CompanyServiceTests
{
	private readonly CompanyService _sut;
	private readonly Mock<IRepositoryManager> _repoManagerMock = new();
	private readonly Mock<ILoggerManager> _loggerMock = new();
	private readonly Mapper _mapper;

	public CompanyServiceTests()
	{
		var profile = new MappingProfile();
		var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
		_mapper = new Mapper(configuration);

		_sut = new CompanyService(_repoManagerMock.Object, _loggerMock.Object, _mapper);
	}

	[Fact]
	public async Task GetCompanyAsync_ShouldReturnCompany_WhenCompanyExists()
	{
		// Arrange
		int companyId = It.IsAny<int>();
		bool trackChanges = false;
		var company = new Company
		{
			Id = companyId,
			Name = "test",
			Address = "test",
			Country = "test"
		};
		var companyDtoActual = _mapper.Map<CompanyDto>(company);

		_repoManagerMock.Setup(e => e.Company.GetCompanyAsync(companyId, trackChanges))
			.ReturnsAsync(company);

		// Act
		var companyDto = await _sut.GetCompanyAsync(companyId, trackChanges);

		// Assert 
		Assert.IsType<CompanyDto>(companyDto);
		Assert.NotNull(companyDto);
		Assert.Equal(companyDto, companyDtoActual);

	}

	[Fact]
	public async Task GetCompanyAsync_ShouldThrowCompanyNotFoundException_WhenCompanyDoesNotExist()
	{
		// Arrange
		int companyId = It.IsAny<int>();
		bool trackChanges = false;
		Company company = null!;

		_repoManagerMock.Setup(e => e.Company.GetCompanyAsync(companyId, trackChanges))
			.ReturnsAsync(company);

		// Act
		Func<int, bool, Task<CompanyDto?>> func = async (companyId, trackChanges) 
			=> await _sut.GetCompanyAsync(companyId, trackChanges);

		// Assert 
		await Assert.ThrowsAsync<CompanyNotFoundException>(() => func(companyId, trackChanges));

	}

	[Fact]
	public async Task CreateCompanyAsync_ShouldReturnCompanyDto()
	{
		// Arrange
		var companyForCreationDto = new CompanyForCreationDto()
		{
			Name = "test",
			Address = "address test",
			Country = "country test",
		};
		var company = _mapper.Map<Company>(companyForCreationDto);
		var actualCompanyDto = _mapper.Map<CompanyDto>(company);

		_repoManagerMock.Setup(e => e.Company.CreateCompany(company))
			.Verifiable(Times.Once);

		_repoManagerMock.Setup(e => e.SaveAsync())
			.Verifiable(Times.Once);

		// Act
		var companyDto = await _sut.CreateCompanyAsync(companyForCreationDto);

		// Assert
		Assert.IsType<CompanyDto>(companyDto);
		Assert.NotNull(companyDto);
		Assert.Equal(companyDto, actualCompanyDto);
		Mock.Verify();
	}

	[Fact]
	public async Task DeleteCompanyAsync_ShouldThrowCompanyNotFoundException_WhenCompanyDoesNotExist()
	{
		// Arrange 
		int companyId = It.IsAny<int>();
		bool trackChanges = true;

		Company company = null!;

		_repoManagerMock.Setup(e => e.Company.GetCompanyAsync(companyId, trackChanges))
			.ReturnsAsync(company);

		// Act
		Func<int, bool, Task> func = async (companyId, trackChanges) 
			=> await _sut.DeleteCompanyAsync(companyId, trackChanges);

		// Assert
		await Assert.ThrowsAsync<CompanyNotFoundException>(() => func(companyId, trackChanges));
	}

	[Fact]
	public async Task DeleteCompanyAsync_ShouldReturnNothing_WhenCompanyExist()
	{
		// Arrange 
		int companyId = It.IsAny<int>();
		bool trackChanges = true;

		Company company = new Company
		{
			Id = companyId,
			Name = "test",
			Address = "test address",
			Country = "test country"
		};

		_repoManagerMock.Setup(e => e.Company.GetCompanyAsync(companyId, trackChanges))
			.ReturnsAsync(company);

		_repoManagerMock.Setup(e => e.Company.DeleteCompany(company))
			.Verifiable(Times.Once);

		_repoManagerMock.Setup(e => e.SaveAsync())
			.Verifiable(Times.Once);

		// Act
		await _sut.DeleteCompanyAsync(companyId, trackChanges);

		// Assert
		Mock.Verify();
	}

	[Fact]
	public async Task UpdateCompanyAsync_ShouldThrowCompanyNotFoundException_WhenCompanyDoesNotExist()
	{
		// Arrange 
		int companyId = It.IsAny<int>();
		bool trackChanges = true;

		Company company = null!;
		CompanyForUpdationDto companyForUpdate = It.IsAny<CompanyForUpdationDto>();

		_repoManagerMock.Setup(e => e.Company.GetCompanyAsync(companyId, trackChanges))
			.ReturnsAsync(company);

		// Act
		Func<int, bool, Task> func = async (companyId, trackChanges)
			=> await _sut.UpdateCompanyAsync(companyId, companyForUpdate, trackChanges);

		// Assert
		await Assert.ThrowsAsync<CompanyNotFoundException>(() => func(companyId, trackChanges));
	}

	[Fact]
	public async Task UpdateCompanyAsync_ShouldReturnNothing_WhenCompanyExists()
	{
		// Arrange 
		int companyId = It.IsAny<int>();
		bool trackChanges = true;

		Company company = new Company
		{
			Id = companyId,
			Name = "test",
			Address = "test address",
			Country = "test country"
		};

		CompanyForUpdationDto companyForUpdate = It.IsAny<CompanyForUpdationDto>();

		_repoManagerMock.Setup(e => e.Company.GetCompanyAsync(companyId, trackChanges))
			.ReturnsAsync(company)
			.Verifiable(Times.Once);

		_repoManagerMock.Setup(e => e.SaveAsync())
			.Verifiable(Times.Once);

		// Act
		await _sut.UpdateCompanyAsync(companyId, companyForUpdate, trackChanges);

		// Assert
		Mock.Verify();
		
	}

	[Fact]
	public async Task GetAllCompaniesAsync_ShouldReturnListOfCompanyDto()
	{
		// Arrange
		var companyParameters = new CompanyParameters();
		bool trackChanges = false;

		var companies = new List<Company>()
		{
			new Company(){ Id = 1, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 2, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 3, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 4, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 5, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 6, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 7, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 8, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 9, Name = "test", Address = "test address", Country = "test country"},
			new Company(){ Id = 10, Name = "test", Address = "test address", Country = "test country"},
		};
		var expectedCompaniesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

		_repoManagerMock.Setup(e => e.Company.GetAllCompaniesAsync(companyParameters, trackChanges))
			.ReturnsAsync(companies);

		// Act
		var actualCompaniesDto = await _sut.GetAllCompaniesAsync(companyParameters, trackChanges);

		// Assert
		Assert.NotNull(actualCompaniesDto);
		Assert.IsAssignableFrom<IEnumerable<CompanyDto>>(actualCompaniesDto);
		Assert.Equal(expectedCompaniesDto, actualCompaniesDto);
		
	}

	[Fact]
	public async Task GetByIdsAsync_ShouldThrowIdParametersBadRequestException_WhenIdsIsNull()
	{
		// Arrange
		IEnumerable<int> ids = null!;
		var trackChanges = false;


		// Act
		Func<IEnumerable<int>, bool, Task<IEnumerable<CompanyDto>>> func = async (ids, trackChanges)
			=> await _sut.GetByIdsAsync(ids, trackChanges);

		// Assert
		await Assert.ThrowsAsync<IdParametersBadRequestException>(() => func(ids, trackChanges));
	}

	[Fact]
	public async Task GetByIdsAsync_ShouldThrowCollectionByIdsBadRequestException_WhenIdsCountNotEqualReturnedCompaniesCount()
	{
		// Arrange
		IEnumerable<int> ids = new List<int>() { 1, 2, 3, 4, 5 };
		var trackChanges = false;
		var companies = new List<Company>()
		{
			new Company(){ Id = 1, Name = "test1", Country = "country test1", Address = "address test1" },
			new Company(){ Id = 2, Name = "test2", Country = "country test2", Address = "address test2" },
			new Company(){ Id = 3, Name = "test3", Country = "country test3", Address = "address test3" },
			new Company(){ Id = 4, Name = "test4", Country = "country test4", Address = "address test4" },
		};

		// Act
		Func<IEnumerable<int>, bool, Task<IEnumerable<CompanyDto>>> func = async (ids, trackChanges)
			=> await _sut.GetByIdsAsync(ids, trackChanges);

		_repoManagerMock.Setup(e => e.Company.GetByIdsAsync(ids, trackChanges))
			.ReturnsAsync(companies);

		// Assert
		Assert.NotEqual(companies.Count, ids.Count());
		await Assert.ThrowsAsync<CollectionByIdsBadRequestException>(() => func(ids, trackChanges));
	}

	[Fact]
	public async Task GetByIdsAsync_ShouldReturnCompaniesDto_WhenIdsCountEqualReturnedCompaniesCount()
	{
		// Arrange
		IEnumerable<int> ids = new List<int>() { 1, 2, 3, 4, 5 };
		var trackChanges = false;
		var companies = new List<Company>()
		{
			new Company(){ Id = 1, Name = "test1", Country = "country test1", Address = "address test1" },
			new Company(){ Id = 2, Name = "test2", Country = "country test2", Address = "address test2" },
			new Company(){ Id = 3, Name = "test3", Country = "country test3", Address = "address test3" },
			new Company(){ Id = 4, Name = "test4", Country = "country test4", Address = "address test4" },
			new Company(){ Id = 5, Name = "test5", Country = "country test5", Address = "address test5" },
		};
		var expectedCompaniesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);

		_repoManagerMock.Setup(e => e.Company.GetByIdsAsync(ids, trackChanges))
			.ReturnsAsync(companies);

		// Act
		var actualCompaniesDto = await _sut.GetByIdsAsync(ids, trackChanges);

		// Assert
		Assert.NotNull(actualCompaniesDto);
		Assert.IsAssignableFrom<IEnumerable<CompanyDto>>(actualCompaniesDto);
		Assert.Equal(companies.Count, ids.Count());
		Assert.Equal(expectedCompaniesDto, actualCompaniesDto);

	}

}