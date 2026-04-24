using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.HouseOfCars.Ruscelli.Application.Abstractions.Repositories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Factories;

namespace BlaisePascal.HouseOfCars.Ruscelli.Infrastructure.Persistence;
 
public sealed class InMemoryParcheggioRepository : IParcheggioRepository
{
    private Parcheggio _parcheggio = ParcheggioFactory.CreaVuoto();

    public Parcheggio Get() => _parcheggio;

    public void Save(Parcheggio parcheggio) => _parcheggio = parcheggio;
}