using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox
{
    public sealed record OccupaBoxCommand(
        int Piano,
        int Colonna,
        string Cella,
        string Box,
        string Targa
    );
}
