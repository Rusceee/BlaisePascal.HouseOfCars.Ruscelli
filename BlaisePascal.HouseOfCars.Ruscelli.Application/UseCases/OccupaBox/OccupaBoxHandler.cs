using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlaisePascal.HouseOfCars.Ruscelli.Application.Abstractions.Repositories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;

namespace BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox
{
    public sealed class OccupaBoxHandler
    {
        private readonly IParcheggioRepository _repository;

        public OccupaBoxHandler(IParcheggioRepository repository)
        {
            _repository = repository;
        }

        public void Handle(OccupaBoxCommand command)
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
            parcheggio.OccupaBox(posizione, new Macchina(command.Targa));
            _repository.Save(parcheggio);
        }
    }
}
