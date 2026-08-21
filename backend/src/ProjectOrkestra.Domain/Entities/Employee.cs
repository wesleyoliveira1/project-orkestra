using System;
using ProjectOrkestra.Domain.Enums;

namespace ProjectOrkestra.Domain.Entities;

public class Employee
{
    public Guid Id { get; private set; }
    public Guid BusinessUnitId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public EmployeeStatus Status  { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    private Employee() {}

    public Employee(Guid businessUnitId, string name, string cpf, string email, string phone, string address)
    {
        if(businessUnitId == Guid.Empty)
            throw new ArgumentException($"BusinessId is required.", nameof(businessUnitId));
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException($"Name is required.", nameof(name));
        if(string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException($"CPF is required.", nameof(cpf));
        if(string.IsNullOrWhiteSpace(email))
            throw new ArgumentException($"Email is required.", nameof(email));
        if(string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException($"Phone is required.", nameof(phone));
        if(string.IsNullOrWhiteSpace(address))
            throw new ArgumentException($"Address is required.", nameof(address));

        Id = Guid.NewGuid();
        BusinessUnitId = businessUnitId;
        Name = name;
        Cpf = cpf;
        Email = email;
        Phone = phone;
        Address = address;
        Status = EmployeeStatus.Active;
        CreatedAt = DateTime.UtcNow;

    }

    public void Deactivate(){
        Status = EmployeeStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate(){
        Status = EmployeeStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Vacation(){
        Status = EmployeeStatus.Vacation;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Freeday(){
        Status = EmployeeStatus.FreeDay;
        UpdatedAt = DateTime.UtcNow;
    }

    public void License(){
        Status = EmployeeStatus.License;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Rename(string newName) {
        if(string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name is required.", nameof(newName));

        Name = newName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeCpf(string newCpf) {
        if(string.IsNullOrWhiteSpace(newCpf))
            throw new ArgumentNullException("Cpf is required.", nameof(newCpf));

        Cpf = newCpf;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeEmail(string newEmail) {
        if(string.IsNullOrWhiteSpace(newEmail))
            throw new ArgumentNullException("Email is required.", nameof(newEmail));

        Email = newEmail;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangePhone(string newPhone) {
        if(string.IsNullOrWhiteSpace(newPhone))
            throw new ArgumentException("Phone is required.", nameof(newPhone));

        Phone = newPhone;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeAddress(string newAddress) {
        if(string.IsNullOrWhiteSpace(newAddress))
            throw new ArgumentNullException("Address is required.", nameof(newAddress));

        Address = newAddress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void TransferToBusinessUnit(Guid businessUnitId) {
        if(businessUnitId == Guid.Empty)
            throw new ArgumentNullException("Business Unit Id is required.", nameof(businessUnitId));

        BusinessUnitId = businessUnitId;
        UpdatedAt = DateTime.UtcNow;
    }

}
