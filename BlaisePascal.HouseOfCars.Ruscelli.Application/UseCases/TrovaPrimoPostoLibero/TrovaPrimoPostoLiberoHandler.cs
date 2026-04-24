using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.HouseOfCars.Ruscelli.Application.Abstractions.Repositories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.TrovaPrimoPostoLibero;

public sealed class TrovaPrimoPostoLiberoHandler
{
    private readonly IParcheggioRepository _repository;

    public TrovaPrimoPostoLiberoHandler(IParcheggioRepository repository)
    {
        _repository = repository;
    }

    public Posizione? Handle(TrovaPrimoPostoLiberoCommand command)
    {
        var parcheggio = _repository.Get();
        return parcheggio.TrovaPrimoPostoLibero(new Colonna(command.Colonna));
    }
}