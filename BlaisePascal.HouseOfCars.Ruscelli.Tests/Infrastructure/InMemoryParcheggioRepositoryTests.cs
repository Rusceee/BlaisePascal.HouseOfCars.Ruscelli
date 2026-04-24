using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Enums;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.Factories;
using BlaisePascal.HouseOfCars.Ruscelli.Domain.VO;
using BlaisePascal.HouseOfCars.Ruscelli.Infrastructure.Persistence;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Infrastructure;

public class InMemoryParcheggioRepositoryTests
{
    [Fact]
    public void Get_DeveRestituireParcheggio()
    {
        var repo = new InMemoryParcheggioRepository();
        var parcheggio = repo.Get();
        Assert.NotNull(parcheggio);
    }

    [Fact]
    public void Get_ParcheggioIniziale_DeveEssereVuoto()
    {
        var repo = new InMemoryParcheggioRepository();
        var parcheggio = repo.Get();
        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        Assert.False(parcheggio.GetBox(pos).Occupato);
    }

    [Fact]
    public void Save_DeveAggiornareIlParcheggio()
    {
        var repo = new InMemoryParcheggioRepository();
        var parcheggio = repo.Get();

        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        parcheggio.OccupaBox(pos, new Macchina("AB123CD"));
        repo.Save(parcheggio);

        var retrieved = repo.Get();
        Assert.True(retrieved.GetBox(pos).Occupato);
    }

    [Fact]
    public void Get_ChiamataMultipla_StessaIstanza()
    {
        var repo = new InMemoryParcheggioRepository();
        var p1 = repo.Get();
        var p2 = repo.Get();
        Assert.Same(p1, p2);
    }

    [Fact]
    public void Save_SostituisceIlParcheggio()
    {
        var repo = new InMemoryParcheggioRepository();
        var nuovoParcheggio = ParcheggioFactory.CreaVuoto();

        var pos = new Posizione(new Piano(5), new Colonna(5), Cella.Sud, Box.Ovest);
        nuovoParcheggio.OccupaBox(pos, new Macchina("NUOVO"));

        repo.Save(nuovoParcheggio);
        var retrieved = repo.Get();

        Assert.Same(nuovoParcheggio, retrieved);
        Assert.True(retrieved.GetBox(pos).Occupato);
    }

    [Fact]
    public void DueRepository_SonoIndipendenti()
    {
        var repo1 = new InMemoryParcheggioRepository();
        var repo2 = new InMemoryParcheggioRepository();

        var pos = new Posizione(new Piano(1), new Colonna(1), Cella.Nord, Box.Est);
        var p1 = repo1.Get();
        p1.OccupaBox(pos, new Macchina("AB123CD"));
        repo1.Save(p1);

        Assert.False(repo2.Get().GetBox(pos).Occupato);
    }
}
