using AutoMapper;
using Contracts;
using Entities;
using Entities.Exceptions;
using Moq;
using Services;
using Shared.DTOs;

namespace CompanyEmployees.Tests;
public class EmployeeServiceTests
{
	private readonly EmployeeService _sut;
	private readonly Mock<IRepositoryManager> _repository;
	private readonly Mock<IDataShaper<EmployeeDto>> _dataShaper;
	private readonly Mock<ILoggerManager> _loggerManager;
	private readonly Mapper _mapper;

    public EmployeeServiceTests()
    {
		var mappingProfile = new MappingProfile();
		var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mappingProfile));
		_mapper = new Mapper(configuration);
		_loggerManager = new Mock<ILoggerManager>();
        _repository = new Mock<IRepositoryManager>();
		_dataShaper = new Mock<IDataShaper<EmployeeDto>>();
		_sut = new EmployeeService(_repository.Object, _loggerManager.Object, _mapper, _dataShaper.Object);
    }

	[Fact]
	public async Task CreateEmployeeForCompanyAsync_ShouldThrowCompanyNotFoundException_WhenCompanyIdDoesNotExist()
	{
		// Arrange 
		int companyIdParam = It.IsAny<int>();
		var employeeForCreationParam = new EmployeeForCreationDto();
		bool trackChangesParam = true;

		_repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, trackChangesParam))
			.ThrowsAsync(new CompanyNotFoundException(companyIdParam));

		// Act
		Func<int, EmployeeForCreationDto, bool, Task<EmployeeDto>> func = async (companyId, employeeForCreation, trackChanges)
			=> await _sut.CreateEmployeeForCompanyAsync(companyId, employeeForCreation, trackChanges);

		// Assert
		await Assert.ThrowsAsync<CompanyNotFoundException>(() => func(companyIdParam, employeeForCreationParam, trackChangesParam));
	}

	[Fact]
	public async Task CreateEmployeeForCompanyAsync_ShouldReturnEmployeeDto_WhenCompanyIdExist()
	{
		// Arrange
		int companyIdParam = 3;
		bool trackChangesParam = true;
		var employeeForCreationParam = new EmployeeForCreationDto
		{
			Name = "test",
			Age = 20,
			Position = "test"
		};

		var companyEntity = new Company();

		var employeeEntity = _mapper.Map<Employee>(employeeForCreationParam);
		var actualEmployeeDto = _mapper.Map<EmployeeDto>(employeeEntity);

		_repository.Setup(e => e.Employee.CreateEmployeeForCompany(companyIdParam, employeeEntity))
			.Verifiable(Times.Once);

		_repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, trackChangesParam))
			.ReturnsAsync(companyEntity);

		_repository.Setup(e => e.SaveAsync())
			.Verifiable(Times.Once);

		// Act
		var expectedEmployeeDto = await _sut.CreateEmployeeForCompanyAsync(companyIdParam, employeeForCreationParam, trackChangesParam);

		// Assert
		Assert.IsType<EmployeeDto>(expectedEmployeeDto);
		Assert.NotNull(expectedEmployeeDto);
		Assert.Equal(expectedEmployeeDto, actualEmployeeDto);
		Mock.Verify();

	}

	[Fact]
	public async Task DeleteEmployeeForCompanyAsync_ShouldThrowCompanyNotFoundException_WhenCompanyDoesNotExist()
	{
		// Arrange
		var companyIdParam = It.IsAny<int>();
		var employeeIdParam = It.IsAny<int>();
		var trackChangesParam = true;

		_repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, trackChangesParam))
			.ThrowsAsync(new CompanyNotFoundException(companyIdParam));

		// Act
		Func<int, int, bool, Task> func = async (companyId, employeeId, trackChanges) => 
			await _sut.DeleteEmployeeForCompanyAsync(companyId, employeeId, trackChanges);

		// Assert
		await Assert.ThrowsAsync<CompanyNotFoundException>(() => func(companyIdParam, employeeIdParam, trackChangesParam));
    }

    [Fact]
    public async Task DeleteEmployeeForCompanyAsync_ShouldThrowEmployeeNotFoundException_WhenCompanyExistAndEmployeeIdDoesNotExist()
    {
        // Arrange
        var companyIdParam = It.IsAny<int>();
        var employeeIdParam = It.IsAny<int>();
        var trackChangesParam = true;

		_repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, trackChangesParam))
			.ReturnsAsync(new Company());

		_repository.Setup(e => e.Employee.GetEmployeeAsync(companyIdParam, employeeIdParam, trackChangesParam))
			.ThrowsAsync(new EmployeeNotFoundException(employeeIdParam));

        // Act
        Func<int, int, bool, Task> func = async (companyId, employeeId, trackChanges) =>
            await _sut.DeleteEmployeeForCompanyAsync(companyId, employeeId, trackChanges);

        // Assert
        await Assert.ThrowsAsync<EmployeeNotFoundException>(() => func(companyIdParam, employeeIdParam, trackChangesParam));
    }

    [Fact]
    public async Task DeleteEmployeeForCompanyAsync_ShouldDeleteEmployeeAndSaveChanges_WhenCompanyExistAndEmployeeExist()
    {
        // Arrange
        var companyIdParam = It.IsAny<int>();
        var employeeIdParam = It.IsAny<int>();
        var trackChangesParam = true;
		var employeeToBeDeleted = new Employee();

        _repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, trackChangesParam))
            .ReturnsAsync(new Company());

        _repository.Setup(e => e.Employee.GetEmployeeAsync(companyIdParam, employeeIdParam, trackChangesParam))
            .ReturnsAsync(employeeToBeDeleted);

		_repository.Setup(e => e.Employee.DeleteEmployee(employeeToBeDeleted))
			.Verifiable(Times.Once);

		_repository.Setup(e => e.SaveAsync())
            .Verifiable(Times.Once);

        // Act
    
        await _sut.DeleteEmployeeForCompanyAsync(companyIdParam, employeeIdParam, trackChangesParam);

		// Assert
		Mock.Verify();
    }

	[Fact]
	public async Task UpdateEmployeeForCompanyAsync_ShouldThrowCompanyNotFoundException_WhenCompanyDoesNotExist()
	{
        // Arrange
        var companyIdParam = It.IsAny<int>();
        var employeeIdParam = It.IsAny<int>();
        var empTrackChangesParam = true;
        var compTrackChangesParam = true;

		_repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, compTrackChangesParam))
			.ThrowsAsync(new CompanyNotFoundException(companyIdParam));

        // Act
        Func<int, int, EmployeeForUpdationDto, bool ,bool, Task> func = async (companyId, employeeId, updateDto, empChanges, compChanges) =>
            await _sut.UpdateEmployeeForCompanyAsync(companyId, employeeId, updateDto, empChanges, compChanges);

        // Assert
        await Assert.ThrowsAsync<CompanyNotFoundException>(() => func(companyIdParam, employeeIdParam, new(), empTrackChangesParam, compTrackChangesParam));
    }

    [Fact]
    public async Task UpdateEmployeeForCompanyAsync_ShouldThrowEmployeeNotFoundException_WhenCompanyExistAndEmployeeDoesNotExist()
    {
        // Arrange
        var companyIdParam = It.IsAny<int>();
        var employeeIdParam = It.IsAny<int>();
        var empTrackChangesParam = true;
        var compTrackChangesParam = true;

        _repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, compTrackChangesParam))
            .ReturnsAsync(new Company());

		_repository.Setup(e => e.Employee.GetEmployeeAsync(companyIdParam, employeeIdParam, empTrackChangesParam))
			.ReturnsAsync(() => null);

        // Act
        Func<int, int, EmployeeForUpdationDto, bool, bool, Task> func = async (companyId, employeeId, updateDto, empChanges, compChanges) =>
            await _sut.UpdateEmployeeForCompanyAsync(companyId, employeeId, updateDto, empChanges, compChanges);

        // Assert
        await Assert.ThrowsAsync<EmployeeNotFoundException>(() => func(companyIdParam, employeeIdParam, new(), empTrackChangesParam, compTrackChangesParam));
    }

    [Fact]
    public async Task UpdateEmployeeForCompanyAsync_ShouldUpdateEmployeeAndSaveChanges_WhenCompanyExistAndEmployeeExist()
    {
        // Arrange
        var companyIdParam = It.IsAny<int>();
        var employeeIdParam = It.IsAny<int>();
        var empTrackChangesParam = true;
        var compTrackChangesParam = true;
		var employee = new Employee
		{
			Name = "TEST",
			Age = 25,
			Position = "TEST",
		};

		var employeeWithUpdates = new EmployeeForUpdationDto
		{
			Name = "test",
			Age = 30,
			Position = "test"
		};

        _repository.Setup(e => e.Company.GetCompanyAsync(companyIdParam, compTrackChangesParam))
            .ReturnsAsync(new Company());

        _repository.Setup(e => e.Employee.GetEmployeeAsync(companyIdParam, employeeIdParam, empTrackChangesParam))
            .ReturnsAsync(employee);

		_repository.Setup(e => e.Employee.DeleteEmployee(employee))
			.Verifiable(Times.Once);

		_repository.Setup(e => e.SaveAsync())
			.Verifiable(Times.Once);

		_mapper.Map(employeeWithUpdates, employee);

        // Act
		// 
        await _sut.UpdateEmployeeForCompanyAsync(companyIdParam, employeeIdParam, employeeWithUpdates, empTrackChangesParam, compTrackChangesParam);

		// Assert
		Assert.Equal(employeeWithUpdates.Name, employee.Name);
		Assert.Equal(employeeWithUpdates.Age, employee.Age);
		Assert.Equal(employeeWithUpdates.Position, employee.Position);
		Mock.Verify();
    }
}
