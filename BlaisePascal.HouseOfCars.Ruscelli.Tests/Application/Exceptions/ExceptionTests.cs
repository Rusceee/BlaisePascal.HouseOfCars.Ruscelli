using BlaisePascal.HouseOfCars.Ruscelli.Application.Exceptions;

namespace BlaisePascal.HouseOfCars.Ruscelli.Tests.Application.Exceptions;

public class ExceptionTests
{
    [Fact]
    public void BoxGiaLiberoException_DeveContenerePosizioneNelMessaggio()
    {
        var ex = new BoxGiaLiberoException("Piano 1, Colonna 2");
        Assert.Contains("Piano 1, Colonna 2", ex.Message);
        Assert.Contains("già libero", ex.Message);
    }

    [Fact]
    public void BoxGiaOccupatoException_DeveContenerePosizioneNelMessaggio()
    {
        var ex = new BoxGiaOccupatoException("Piano 3, Colonna 4");
        Assert.Contains("Piano 3, Colonna 4", ex.Message);
        Assert.Contains("già occupato", ex.Message);
    }

    [Fact]
    public void BoxNonTrovatoException_DeveContenerePosizioneNelMessaggio()
    {
        var ex = new BoxNonTrovatoException("Piano 5, Colonna 6");
        Assert.Contains("Piano 5, Colonna 6", ex.Message);
    }

    [Fact]
    public void BoxGiaLiberoException_DeveEreditareDaException()
    {
        var ex = new BoxGiaLiberoException("test");
        Assert.IsAssignableFrom<Exception>(ex);
    }

    [Fact]
    public void BoxGiaOccupatoException_DeveEreditareDaException()
    {
        var ex = new BoxGiaOccupatoException("test");
        Assert.IsAssignableFrom<Exception>(ex);
    }

    [Fact]
    public void BoxNonTrovatoException_DeveEreditareDaException()
    {
        var ex = new BoxNonTrovatoException("test");
        Assert.IsAssignableFrom<Exception>(ex);
    }

    [Fact]
    public void BoxGiaLiberoException_PosizioneVuota_DeveCrearsi()
    {
        var ex = new BoxGiaLiberoException("");
        Assert.NotNull(ex.Message);
    }

    [Fact]
    public void BoxGiaOccupatoException_PosizioneVuota_DeveCrearsi()
    {
        var ex = new BoxGiaOccupatoException("");
        Assert.NotNull(ex.Message);
    }

    [Fact]
    public void BoxNonTrovatoException_PosizioneVuota_DeveCrearsi()
    {
        var ex = new BoxNonTrovatoException("");
        Assert.NotNull(ex.Message);
    }
}
