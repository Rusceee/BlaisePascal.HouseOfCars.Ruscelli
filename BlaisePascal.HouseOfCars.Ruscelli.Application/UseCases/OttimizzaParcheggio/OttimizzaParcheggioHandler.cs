using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.HouseOfCars.Ruscelli.Application.Abstractions.Repositories;

namespace BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OttimizzaParcheggio;

public sealed class OttimizzaParcheggioHandler
{
    private readonly IParcheggioRepository _repository;

    public OttimizzaParcheggioHandler(IParcheggioRepository repository)
    {
        _repository = repository;
    }

    public void Handle(OttimizzaParcheggioCommand command)
    {
        var parcheggio = _repository.Get();
        parcheggio.OttimizzaParcheggio();
        _repository.Save(parcheggio);
    }
}
