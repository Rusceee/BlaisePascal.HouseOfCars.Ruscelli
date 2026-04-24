using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.LiberaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;
using BlaisePascal.HouseOfCars.Ruscelli.Infrastructure.Persistence;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Application.UseCases;

public class LiberaBoxHandlerTests
{
    private readonly InMemoryParcheggioRepository _repo = new();
    private readonly LiberaBoxHandler _handler;
    private readonly OccupaBoxHandler _occupaHandler;

    public LiberaBoxHandlerTests()
    {
        _handler = new LiberaBoxHandler(_repo);
        _occupaHandler = new OccupaBoxHandler(_repo);
    }

    private void OccupaUnBox(int piano = 1, int colonna = 1,
        string cella = "Nord", string box = "Est", string targa = "AB123CD")
    {
        _occupaHandler.Handle(new OccupaBoxCommand(piano, colonna, cella, box, targa));
    }

    [Fact]
    public void Handle_BoxOccupato_DeveLibera()
    {
        OccupaUnBox();
        _handler.Handle(new LiberaBoxCommand(1, 1, "Nord", "Est"));

        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        Assert.False(_repo.Get().GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_BoxLibero_DeveLanciareEccezione()
    {
        var command = new LiberaBoxCommand(1, 1, "Nord", "Est");
        Assert.Throws<InvalidOperationException>(() => _handler.Handle(command));
    }

    [Fact]
    public void Handle_CellaCaseInsensitive_DeveFunzionare()
    {
        OccupaUnBox();
        _handler.Handle(new LiberaBoxCommand(1, 1, "nord", "est"));

        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        Assert.False(_repo.Get().GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_CellaNonValida_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentException>(
            () => _handler.Handle(new LiberaBoxCommand(1, 1, "Invalida", "Est")));
    }

    [Fact]
    public void Handle_BoxNonValido_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentException>(
            () => _handler.Handle(new LiberaBoxCommand(1, 1, "Nord", "Invalido")));
    }

    [Fact]
    public void Handle_PianoNonValido_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _handler.Handle(new LiberaBoxCommand(0, 1, "Nord", "Est")));
    }

    [Fact]
    public void Handle_ColonnaNonValida_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _handler.Handle(new LiberaBoxCommand(1, 11, "Nord", "Est")));
    }

    [Fact]
    public void Handle_PianoSotterraneo_DeveFunzionare()
    {
        OccupaUnBox(-5, 3, "Sud", "Ovest", "ZZ999ZZ");
        _handler.Handle(new LiberaBoxCommand(-5, 3, "Sud", "Ovest"));

        var pos = new Posizione(new Piano(-5), new Colonna(3), Cella.Sud, Box.Ovest);
        Assert.False(_repo.Get().GetBox(pos).Occupato);
    }

    [Fact]
    public void Handle_OccupaLiberaOccupa_DeveFunzionare()
    {
        OccupaUnBox(2, 4, "Nord", "Est", "PRIMO");
        _handler.Handle(new LiberaBoxCommand(2, 4, "Nord", "Est"));
        OccupaUnBox(2, 4, "Nord", "Est", "SECONDO");

        var pos = new Posizione(new Piano(2), new Colonna(4), Cella.Nord, Box.Est);
        var box = _repo.Get().GetBox(pos);
        Assert.True(box.Occupato);
        Assert.Equal("SECONDO", box.MacchinaParcheggiata!.Targa);
    }
}
