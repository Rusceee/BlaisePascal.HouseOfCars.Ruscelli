using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Factories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;
using BlaisePascal.HouseOfCars.Ruscelli.Infrastructure.Persistence;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Application.UseCases;

public class OccupaBoxHandlerTests
{
    private readonly InMemoryParcheggioRepository _repo = new();
    private readonly OccupaBoxHandler _handler;

    public OccupaBoxHandlerTests()
    {
        _handler = new OccupaBoxHandler(_repo);
    }

    [Fact]
    public void Handle_CommandValido_DeveOccupareBox()
    {
        var command = new OccupaBoxCommand(1, 1, "Nord", "Est", "AB123CD");
        _handler.Handle(command);

        var parcheggio = _repo.Get();
        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        Assert.True(parcheggio.GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_TargaMinuscola_DeveAccettare()
    {
        var command = new OccupaBoxCommand(1, 1, "Nord", "Est", "ab123cd");
        _handler.Handle(command);

        var parcheggio = _repo.Get();
        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        Assert.Equal("AB123CD", parcheggio.GetBox(pos).MacchinaParcheggiata!.Targa);
    }

    [Fact]
    public void Handle_CellaCaseInsensitive_DeveFunzionare()
    {
        var command = new OccupaBoxCommand(1, 1, "nord", "est", "AB123CD");
        _handler.Handle(command);

        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        Assert.True(_repo.Get().GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_CellaNonValida_DeveLanciareEccezione()
    {
        var command = new OccupaBoxCommand(1, 1, "Invalida", "Est", "AB123CD");
        Assert.Throws<ArgumentException>(() => _handler.Handle(command));
    }

    [Fact]
    public void Handle_BoxNonValido_DeveLanciareEccezione()
    {
        var command = new OccupaBoxCommand(1, 1, "Nord", "Invalido", "AB123CD");
        Assert.Throws<ArgumentException>(() => _handler.Handle(command));
    }

    [Fact]
    public void Handle_PianoNonValido_DeveLanciareEccezione()
    {
        var command = new OccupaBoxCommand(0, 1, "Nord", "Est", "AB123CD");
        Assert.Throws<ArgumentOutOfRangeException>(() => _handler.Handle(command));
    }

    [Fact]
    public void Handle_ColonnaNonValida_DeveLanciareEccezione()
    {
        var command = new OccupaBoxCommand(1, 0, "Nord", "Est", "AB123CD");
        Assert.Throws<ArgumentOutOfRangeException>(() => _handler.Handle(command));
    }

    [Fact]
    public void Handle_TargaVuota_DeveLanciareEccezione()
    {
        var command = new OccupaBoxCommand(1, 1, "Nord", "Est", "");
        Assert.Throws<ArgumentException>(() => _handler.Handle(command));
    }

    [Fact]
    public void Handle_BoxGiaOccupato_DeveLanciareEccezione()
    {
        var cmd1 = new OccupaBoxCommand(1, 1, "Nord", "Est", "AB123CD");
        _handler.Handle(cmd1);

        var cmd2 = new OccupaBoxCommand(1, 1, "Nord", "Est", "XY456WZ");
        Assert.Throws<InvalidOperationException>(() => _handler.Handle(cmd2));
    }

    [Fact]
    public void Handle_PianoSotterraneo_DeveFunzionare()
    {
        var command = new OccupaBoxCommand(-3, 5, "Sud", "Ovest", "AB123CD");
        _handler.Handle(command);

        var pos = new Posizione(new Piano(-3), new Colonna(5), Cella.Sud, Box.Ovest);
        Assert.True(_repo.Get().GetBox(pos).Occupato);
    }

    [Theory]
    [InlineData("Nord", "Est")]
    [InlineData("Nord", "Ovest")]
    [InlineData("Sud", "Est")]
    [InlineData("Sud", "Ovest")]
    public void Handle_TutteLeCombinazioni_DeveFunzionare(string cella, string box)
    {
        var command = new OccupaBoxCommand(1, 1, cella, box, $"T{cella}{box}");
        _handler.Handle(command);
    }
}
