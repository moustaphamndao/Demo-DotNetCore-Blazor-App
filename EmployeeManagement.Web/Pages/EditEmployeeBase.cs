﻿using AutoMapper;
using EmployeeManagement.Models;
using EmployeeManagement.Web.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Pages
{
    public class EditEmployeeBase : ComponentBase
    {
        [CascadingParameter]
        public Task<AuthenticationState> authenticationStateTask { get; set; }

        [Inject]
        public IEmployeeService employeeService { get; set; }
        private Employee Employee { get; set; } = new Employee();

        public EditEmployeeModel EditEmployeeModel { get; set; } = new EditEmployeeModel();

        [Inject]
        public IDepartmentService departmentService { get; set; }
        public List<Department> Departmnents { get; set; } = new List<Department>();

        [Parameter]
        public string Id { get; set; }

        [Inject]
        public IMapper Mapper { get; set; }

        [Inject]
        public NavigationManager NavigationManager {get; set;}
        public string PageTitle { get; set; }

        protected async override  Task OnInitializedAsync()
        {
            var authenticationState = await authenticationStateTask;
            if (!authenticationState.User.Identity.IsAuthenticated)
            {
                string returnUrl = WebUtility.UrlEncode($"/editEmployee/{Id}");
                NavigationManager.NavigateTo($"/identity/account/login?returnUrl={returnUrl}");
            }
            int.TryParse(Id, out int employeeId);

            if (employeeId != 0)
            {
                PageTitle = "Edit employee";
                Employee = await employeeService.GetEmployee(int.Parse(Id));
            }
            else
            {
                PageTitle = "Create employee";
                Employee = new Employee
                {
                    DepartmentId = 1,
                    DateOfBrith = DateTime.Now,
                    PhotoPath = "images/noimage.jpg"
                };
            }
            
            Departmnents = (await departmentService.GetDepartments()).ToList();
            Mapper.Map(Employee, EditEmployeeModel);

            //This code is now replaced by the mapping (AutoMapper)
            //EditEmployeeModel.EmployeeId = Employee.EmployeeId;
            //EditEmployeeModel.FirstName = Employee.FirstName;
            //EditEmployeeModel.LastName = Employee.LastName;
            //EditEmployeeModel.Email = Employee.Email;
            //EditEmployeeModel.ConfirmEmail = Employee.Email;
            //EditEmployeeModel.DateOfBrith = Employee.DateOfBrith;
            //EditEmployeeModel.Gender = Employee.Gender;
            //EditEmployeeModel.PhotoPath = Employee.PhotoPath;
            //EditEmployeeModel.DepartmentId = Employee.DepartmentId;
            //EditEmployeeModel.Department = Employee.Department;
            
        }

        protected async Task HandleValidSubmit()
        {
            Mapper.Map(EditEmployeeModel, Employee);
            
            Employee result = null;

            if (Employee.EmployeeId != 0)
            {
                result = await employeeService.UpdateEmployee(Employee);
            }
            else
            {
                result = await employeeService.CreateEmployee(Employee);
            }

            if (result != null)
            {
                NavigationManager.NavigateTo("/");
            }
           
        }
        protected Ndawene.Components.ConfirmationBase DeleteConfirmation { get; set; }
        protected void Delete_Click()
        {
            DeleteConfirmation.Show();

        }
        protected async Task ConfirmDelete_Click(bool deleteConfirmed)
        {
            if (deleteConfirmed)
            {
                await employeeService.DeleteEmployee(Employee.EmployeeId);
                NavigationManager.NavigateTo("/");
            }
        }
    }
}
