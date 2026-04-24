using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaisePascal.HouseOfCars.Ruscelli.Application.Abstractions.Repositories;

using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;

public interface IParcheggioRepository
{
    Parcheggio Get();
    void Save(Parcheggio parcheggio);
}
