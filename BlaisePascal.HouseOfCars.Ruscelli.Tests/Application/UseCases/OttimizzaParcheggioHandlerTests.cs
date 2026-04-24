using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OttimizzaParcheggio;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;
using BlaisePascal.HouseOfCars.Ruscelli.Infrastructure.Persistence;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Application.UseCases;

public class OttimizzaParcheggioHandlerTests
{
    private readonly InMemoryParcheggioRepository _repo = new();
    private readonly OttimizzaParcheggioHandler _handler;
    private readonly OccupaBoxHandler _occupaHandler;

    public OttimizzaParcheggioHandlerTests()
    {
        _handler = new OttimizzaParcheggioHandler(_repo);
        _occupaHandler = new OccupaBoxHandler(_repo);
    }

    [Fact]
    public void Handle_ParcheggioVuoto_NonDeveLanciareEccezione()
    {
        _handler.Handle(new OttimizzaParcheggioCommand());
    }

    [Fact]
    public void Handle_MacchinaAlPiano8_DeveSpostarla()
    {
        _occupaHandler.Handle(new OccupaBoxCommand(8, 1, "Nord", "Est", "AB123CD"));

        _handler.Handle(new OttimizzaParcheggioCommand());

        var pos = new Posizione(new Piano(8), new Colonna(1), Cella.Nord, Box.Est);
        Assert.False(_repo.Get().GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_MacchinaAlPiano7_NonDeveSpostarla()
    {
        _occupaHandler.Handle(new OccupaBoxCommand(7, 1, "Nord", "Est", "AB123CD"));

        _handler.Handle(new OttimizzaParcheggioCommand());

        var pos = new Posizione(new Piano(7), new Colonna(1), Cella.Nord, Box.Est);
        Assert.True(_repo.Get().GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_MacchineAiPiani8_9_10_DeveSpostarleTutte()
    {
        _occupaHandler.Handle(new OccupaBoxCommand(8, 1, "Nord", "Est", "T1"));
        _occupaHandler.Handle(new OccupaBoxCommand(9, 2, "Sud", "Ovest", "T2"));
        _occupaHandler.Handle(new OccupaBoxCommand(10, 3, "Nord", "Ovest", "T3"));

        _handler.Handle(new OttimizzaParcheggioCommand());

        var p1 = new Posizione(new Piano(8), new Colonna(1), Cella.Nord, Box.Est);
        var p2 = new Posizione(new Piano(9), new Colonna(2), Cella.Sud, Box.Ovest);
        var p3 = new Posizione(new Piano(10), new Colonna(3), Cella.Nord, Box.Ovest);

        Assert.False(_repo.Get().GetBox(p1).Occupato);
        Assert.False(_repo.Get().GetBox(p2).Occupato);
        Assert.False(_repo.Get().GetBox(p3).Occupato);
    }

    [Fact]
    public void Handle_MacchinaAlPiano12_DeveSpostarla()
    {
        _occupaHandler.Handle(new OccupaBoxCommand(12, 10, "Sud", "Ovest", "TOP"));

        _handler.Handle(new OttimizzaParcheggioCommand());

        var pos = new Posizione(new Piano(12), new Colonna(10), Cella.Sud, Box.Ovest);
        Assert.False(_repo.Get().GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_MacchineAiPianiBassPositivi_NonDeveSpostarle()
    {
        _occupaHandler.Handle(new OccupaBoxCommand(1, 1, "Nord", "Est", "LOW1"));
        _occupaHandler.Handle(new OccupaBoxCommand(3, 2, "Sud", "Ovest", "LOW2"));
        _occupaHandler.Handle(new OccupaBoxCommand(5, 5, "Nord", "Ovest", "LOW3"));

        _handler.Handle(new OttimizzaParcheggioCommand());

        var p1 = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        var p2 = new Posizione(new Piano(3), new Colonna(2), Cella.Sud, Box.Ovest);
        var p3 = new Posizione(new Piano(5), new Colonna(5), Cella.Nord, Box.Ovest);

        Assert.True(_repo.Get().GetBox(p1).Occupato);
        Assert.True(_repo.Get().GetBox(p2).Occupato);
        Assert.True(_repo.Get().GetBox(p3).Occupato);
    }

    [Fact]
    public void Handle_MacchineAlPianoSotterraneo_NonDeveSpostarle()
    {
        _occupaHandler.Handle(new OccupaBoxCommand(-1, 1, "Nord", "Est", "SOTTO"));

        _handler.Handle(new OttimizzaParcheggioCommand());

        var pos = new Posizione(new Piano(-1), new Colonna(1), Cella.Nord, Box.Est);
        Assert.True(_repo.Get().GetBox(pos).Occupato);
    }
}
