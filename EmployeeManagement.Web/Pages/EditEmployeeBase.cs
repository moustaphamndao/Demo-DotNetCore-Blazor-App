﻿using EmployeeManagement.Models;
using EmployeeManagement.Web.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Web.Pages
{
    public class EditEmployeeBase : ComponentBase
    {
        [Inject]
        public IEmployeeService employeeService { get; set; }
        public Employee Employee { get; set; } = new Employee();

        [Parameter]
        public string Id { get; set; }

        protected async override  Task OnInitializedAsync()
        {
            Employee = await employeeService.GetEmployee(int.Parse(Id));
        }
    }
}