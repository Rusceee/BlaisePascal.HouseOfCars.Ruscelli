using BlaisePascal.HouseOfCars.Ruscelli.Domain.Entities;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Domain.Entities;

public class MacchinaTests
{
    [Fact]
    public void Crea_TargaValida_DeveAvereTargaCorretta()
    {
        var macchina = new Macchina("AB123CD");
        Assert.Equal("AB123CD", macchina.Targa);
    }

    [Fact]
    public void Crea_TargaMinuscola_DeveConvertireInMaiuscolo()
    {
        var macchina = new Macchina("ab123cd");
        Assert.Equal("AB123CD", macchina.Targa);
    }

    [Fact]
    public void Crea_TargaMista_DeveConvertireInMaiuscolo()
    {
        var macchina = new Macchina("Ab123cD");
        Assert.Equal("AB123CD", macchina.Targa);
    }

    [Fact]
    public void Crea_TargaConSpazi_DeveRimuovereSpazi()
    {
        var macchina = new Macchina("  AB123CD  ");
        Assert.Equal("AB123CD", macchina.Targa);
    }

    [Theory]
    [InlineData("AA000AA")]
    [InlineData("ZZ999ZZ")]
    [InlineData("XY456WZ")]
    public void Crea_TargheDiverse_DeveFunzionare(string targa)
    {
        var macchina = new Macchina(targa);
        Assert.Equal(targa.Trim().ToUpperInvariant(), macchina.Targa);
    }

    [Fact]
    public void Crea_TargaNull_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentException>(() => new Macchina(null!));
    }

    [Fact]
    public void Crea_TargaVuota_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentException>(() => new Macchina(""));
    }

    [Fact]
    public void Crea_TargaSoloSpazi_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentException>(() => new Macchina("   "));
    }

    [Fact]
    public void Crea_TargaTab_DeveLanciareEccezione()
    {
        Assert.Throws<ArgumentException>(() => new Macchina("\t"));
    }

    [Fact]
    public void Crea_TargaSingoloCarattere_DeveFunzionare()
    {
        var macchina = new Macchina("A");
        Assert.Equal("A", macchina.Targa);
    }

    [Fact]
    public void Crea_TargaConNumeri_DeveFunzionare()
    {
        var macchina = new Macchina("12345");
        Assert.Equal("12345", macchina.Targa);
    }
}
