using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Core;
using Microsoft.Extensions.Hosting;

namespace DarkStar.Engine.Runner;

public class DarkSunTerminalHostedService : IHostedService
{
    private readonly IDarkSunEngine _darkSunEngine;
    public DarkSunTerminalHostedService(IDarkSunEngine darkSunEngine) => _darkSunEngine = darkSunEngine;


    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(
            () =>
            {
                Console.WriteLine(">> PRESS ENTER FOR START JS SHELL");
                var command = Console.ReadLine();
                while (command != "EXIT")
                {
                    Console.Write("SHELL > ");
                    command = Console.ReadLine();
                    var result = _darkSunEngine.ScriptEngineService.ExecuteCommand(command!);

                    if (result.Result != null)
                    {
                        Console.WriteLine(result.Result);
                    }

                    if (result.Exception != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(result.Exception.Message);
                        Console.ResetColor();
                    }
                }
            },
            cancellationToken
        );


        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
