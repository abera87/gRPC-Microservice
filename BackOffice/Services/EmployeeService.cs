using System.Threading.Tasks;
using Grpc.Core;



namespace BackOffice.Services
{
    public class EmployeeService : Employee.EmployeeBase
    {
        public override Task<EmployeesResponse> GetAllEmployee(Empty request, ServerCallContext context)
        {
            return Task.FromResult(
                new EmployeesResponse
                {
                    Employees ={
                        new EmployeeResponse
                        {
                            Empid = 1,
                            Name = "Animesh",
                            Address = "Kolkata",
                            Phone = "1234567890",
                            Deptid = 1
                        },
                        new EmployeeResponse
                        {
                            Empid = 2,
                            Name = "Sovan",
                            Address = "Hyderabad",
                            Phone = "1234567890",
                            Deptid = 1
                        }
                    }
                }
            );
        }
        public override Task<EmployeeResponse> GetEmployeeById(EmployeeRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new EmployeeResponse
                {
                    Empid = request.Id,
                    Name = "Animesh",
                    Address = "Kolkata",
                    Phone = "1234567890",
                    Deptid = 1
                }
            );
        }
    }
}