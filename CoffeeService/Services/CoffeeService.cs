using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace CoffeeService
{
    [Authorize]
    public class CoffeeService : Coffee.CoffeeBase
    {
        private readonly ILogger<CoffeeService> logger;
        private static readonly string[] Coffees = new[]{
            "Flat White","Long Black","Latte","Americano","Cappuccino"
        };

        public CoffeeService(ILogger<CoffeeService> logger)
        {
            this.logger = logger;
        }

        public override Task<CoffeeResponse> GetRandomCoffee(Empty request, Grpc.Core.ServerCallContext context)
        {
            //return base.GetRandomCoffee(request, context);
            var rnd = new Random();
            var t = Coffees[rnd.Next(Coffees.Length)];
            CoffeeResponse tr = new CoffeeResponse { Name = t };

            return Task.FromResult(tr);
        }

        public override Task<CoffeeResponses> GetAllCoffee(Empty request, Grpc.Core.ServerCallContext context)
        {
            //return base.GetAllCoffee(request, context);
            var tr = new CoffeeResponses
            {
                Names = {Coffees}
            };
            return Task.FromResult(tr);
        }
    }
}