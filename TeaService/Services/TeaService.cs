using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace TeaService
{
    [Authorize]
    public class TeaService : Tea.TeaBase
    {
        private readonly ILogger<TeaService> logger;
        private static readonly string[] Teas = new[]{
            "Grean","Peppermint","Earl Grey","English Breakfast","Camomile"
        };

        public TeaService(ILogger<TeaService> logger)
        {
            this.logger = logger;
        }

        public override Task<TeaResponse> GetRandomTea(Empty request, Grpc.Core.ServerCallContext context)
        {
            //return base.GetRandomTea(request, context);
            var rnd = new Random();
            var t = Teas[rnd.Next(Teas.Length)];
            TeaResponse tr = new TeaResponse { Name = t };

            return Task.FromResult(tr);
        }

        public override Task<TeaResponses> GetAllTea(Empty request, Grpc.Core.ServerCallContext context)
        {
            //return base.GetAllTea(request, context);
            var tr = new TeaResponses
            {
                Names = {Teas}
            };
            return Task.FromResult(tr);
        }
    }
}