using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.TrovaPrimoPostoLibero;
using BlaisePascal.HouseOfCars.Ruscelli.Application.UseCases.OccupaBox;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Infrastructure.Persistence;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Application.UseCases;

public class TrovaPrimoPostoLiberoHandlerTests
{
    private readonly InMemoryParcheggioRepository _repo = new();
    private readonly TrovaPrimoPostoLiberoHandler _handler;

    public TrovaPrimoPostoLiberoHandlerTests()
    {
        _handler = new TrovaPrimoPostoLiberoHandler(_repo);
    }

    [Fact]
    public void Handle_ParcheggioVuoto_DeveRestituirePosizione()
    {
        var result = _handler.Handle(new TrovaPrimoPostoLiberoCommand(5));
        Assert.NotNull(result);
    }

    [Fact]
    public void Handle_DeveRestituireColonnaTarget()
    {
        var result = _handler.Handle(new TrovaPrimoPostoLiberoCommand(3));
        Assert.NotNull(result);
        Assert.Equal(3, result!.Value.Colonna.Valore);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Handle_ColonneValideDiverse_DeveFunzionare(int colonna)
    {
        var result = _handler.Handle(new TrovaPrimoPostoLiberoCommand(colonna));
        Assert.NotNull(result);
        Assert.Equal(colonna, result!.Value.Colonna.Valore);
    }

    [Fact]
    public void Handle_ColonnaNonValida_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _handler.Handle(new TrovaPrimoPostoLiberoCommand(0)));
    }

    [Fact]
    public void Handle_ColonnaTroppoAlta_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => _handler.Handle(new TrovaPrimoPostoLiberoCommand(11)));
    }

    [Fact]
    public void Handle_DopoOccupazione_RestituiscePostoLibero()
    {
        var occupaHandler = new OccupaBoxHandler(_repo);
        occupaHandler.Handle(new OccupaBoxCommand(1, 5, "Nord", "Est", "AB123CD"));

        var result = _handler.Handle(new TrovaPrimoPostoLiberoCommand(5));
        Assert.NotNull(result);
        // Il posto restituito non deve essere quello occupato
        var pos = result!.Value;
        bool stessoPosto = pos.Piano.Valore == 1
            && pos.Colonna.Valore == 5
            && pos.Cella == Cella.Nord
            && pos.Box == Box.Est;
        Assert.False(stessoPosto);
    }
}
