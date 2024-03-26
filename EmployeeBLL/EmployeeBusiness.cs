using EmployeeDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EmployeeBLL
{
    public class EmployeeBusiness:IDisposable
    {
            private readonly EmployeeDBEntities dbcontext;
            public bool status;
            public EmployeeBusiness(EmployeeDBEntities dbcontext)
            {
                this.dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
                status = false;
            }

            public List<EmpProfile> GetAllEmployee()
            {
                return dbcontext.EmpProfiles.ToList();
            }
           public EmployeeDTO GetEmployeeById(int empcode)
           {
                using (EmployeeDAL.EmployeeDBEntities db = new EmployeeDBEntities())
                {
                    EmpProfile empProfile = db.EmpProfiles.FirstOrDefault(e => e.EmpCode == empcode);

                    if (empProfile != null)
                    {
                        EmployeeDTO empDTO = new EmployeeDTO
                        {
                            EmpName = empProfile.EmpName,
                            EmpCode = empProfile.EmpCode,
                            Email = empProfile.Email,
                            DeptCode = empProfile.DeptCode,
                            DateOfBirth = empProfile.DateOfBirth
                        };

                        return empDTO;
                    }
                   
                    return null;
                }

           }
        public EmpProfile AddEmployee(EmployeeDTO employeeDTO)
        {
            if (employeeDTO== null)
            {
                throw new ArgumentNullException(nameof(employeeDTO));
            }
            if (dbcontext.EmpProfiles.Any(e => e.EmpCode == employeeDTO.EmpCode))
            {
                throw new ArgumentException($"Employee with code {employeeDTO.EmpCode} already exists.");
            }
            EmpProfile newEmployee = new EmpProfile
            {
                EmpCode = employeeDTO.EmpCode,
                EmpName = employeeDTO.EmpName,
                Email = employeeDTO.Email,
                DeptCode = employeeDTO.DeptCode,
                DateOfBirth = (DateTime)employeeDTO.DateOfBirth
            };

            // Add the new employee to the context
            dbcontext.EmpProfiles.Add(newEmployee);

            // Save changes to the database
            dbcontext.SaveChanges();
            return newEmployee;
        }

        public EmpProfile UpdateEmployee(EmployeeDTO emp)
        {
            if (emp == null)
            {
                throw new ArgumentNullException(nameof(emp));
            }

            var employee = dbcontext.EmpProfiles.Find(emp.EmpCode);

            if (employee == null)
            {
                throw new ArgumentException($"User with ID {emp.EmpCode} not found");
            }

            employee.EmpName = emp.EmpName;
            employee.Email = emp.Email;
            employee.DeptCode = emp.DeptCode;
            employee.DateOfBirth = (DateTime)emp.DateOfBirth;

            dbcontext.SaveChanges();
            return employee;

        }

        public void DeleteEmpProfile(int empCode)
        {
            EmpProfile empProfile = dbcontext.EmpProfiles.FirstOrDefault(e => e.EmpCode == empCode);

            if (empProfile != null)
            {
                dbcontext.EmpProfiles.Remove(empProfile);
                dbcontext.SaveChanges();
            }
           
        }
        public List<DeptMaster> GetAllStatuses()
        {
            return dbcontext.DeptMasters.ToList();
        }
        public void Dispose()
        {
            dbcontext.Dispose();
        }
    }

}

