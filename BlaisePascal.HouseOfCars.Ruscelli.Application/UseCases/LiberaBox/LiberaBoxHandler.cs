using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.HouseOfCars.Ruscelli.Application.Abstractions.Repositories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.LiberaBox;

public sealed class LiberaBoxHandler
{
    private readonly IParcheggioRepository _repository;

    public LiberaBoxHandler(IParcheggioRepository repository)
    {
        _repository = repository;
    }

    public void Handle(LiberaBoxCommand command)
    {
        if (!System.Enum.TryParse<Cella>(command.Cella, true, out var cella))
            throw new ArgumentException("Cella non valida. Valori ammessi: Nord, Sud");

        if (!System.Enum.TryParse<Box>(command.Box, true, out var box))
            throw new ArgumentException("Box non valido. Valori ammessi: Est, Ovest");

        var posizione = new Posizione(
            new Piano(command.Piano),
            new Colonna(command.Colonna),
            cella,
            box
        );

        var parcheggio = _repository.Get();
        parcheggio.LiberaBox(posizione);
        _repository.Save(parcheggio);
    }
}
