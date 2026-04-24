using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.LiberaBox
{
    public sealed record LiberaBoxCommand(
        int Piano,
        int Colonna,
        string Cella,
        string Box
    );
}
