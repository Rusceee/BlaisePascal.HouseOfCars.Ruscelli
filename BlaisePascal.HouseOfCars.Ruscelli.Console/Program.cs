using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.TrovaPrimoPostoLibero;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OttimizzaParcheggio;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.LiberaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Infrastructure.Persistence;
using BlaisePascal.HouseOfCars.Ruscelli.Presentation.Console.Menus;

var repository = new InMemoryParcheggioRepository();

var menu = new MenuPrincipale(
    new TrovaPrimoPostoLiberoHandler(repository),
    new OttimizzaParcheggioHandler(repository),
    new OccupaBoxHandler(repository),
    new LiberaBoxHandler(repository)
);

menu.Avvia();