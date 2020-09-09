using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using BackOffice;
using RMClient.Configuration;
using System.Net.Http;
using Grpc.Core;

namespace RMClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = AppSettings.InitConfig();
            // Console.WriteLine("Hello World!");
            // string gRPCAddrerss = "https://localhost:5001";
            // string gRPCAddrerss = config["gRPCServiceAddress:BackOffice"];

            // using var channel = GrpcChannel.ForAddress(gRPCAddrerss);
            // var client = new Employee.EmployeeClient(channel);

            // var response = await client.GetEmployeeByIdAsync(
            //     new EmployeeRequest { Id = 11 }
            // );

            // var employees = await client.GetAllEmployeeAsync(new Empty());

            // Console.WriteLine($"Employee Id: {response.Empid}, Employee Name: {response.Name}");
            // Console.WriteLine("All employee response");
            // foreach (var res in employees.Employees)
            // {
            //     Console.WriteLine($"Employee Id: {res.Empid}, Employee Name: {res.Name}");
            // }

            // call tea service;
            try
            {
                var addr = config.GetSection("gRPCServiceAddress").GetSection("TeaService").Value;

                Console.WriteLine(addr);
                using var tChannel = GrpcChannel.ForAddress(addr);

                var c = new TeaService.Tea.TeaClient(tChannel);

                var r = await c.GetAllTeaAsync(new TeaService.Empty());
                foreach (var item in r.Names)
                {
                    Console.WriteLine(item);
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            // call securly
            HttpClient httpClient = new HttpClient();
            IdentityService identityService = new IdentityService(httpClient);
            var token = await identityService.GetAccessTockenAsync();
            var tokenValue = "Bearer " + token;
            var metadata = new Metadata{
                {"Authorization",tokenValue}
            };
            CallOptions callOptions = new CallOptions(metadata);


            string teaServiceAddress = config["gRPCServiceAddress:TeaService"];

            var teaServiceChannel = GrpcChannel.ForAddress(teaServiceAddress, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });

            var teaClient = new TeaService.Tea.TeaClient(teaServiceChannel);

            var response = await teaClient.GetAllTeaAsync(new TeaService.Empty(), callOptions);

            foreach (var item in response.Names)
            {
                Console.WriteLine(item);
            }

              string coffeeServiceAddress = config["gRPCServiceAddress:CoffeeService"];

            var coffeeServiceChannel = GrpcChannel.ForAddress(coffeeServiceAddress, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });

            var coffeeClient = new CoffeeService.Coffee.CoffeeClient(coffeeServiceChannel);

            var cResponse = await coffeeClient.GetAllCoffeeAsync(new CoffeeService.Empty(), callOptions);

            foreach (var item in cResponse.Names)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Progress any ket to exist !!!");
            Console.ReadKey();
        }
    }
}
