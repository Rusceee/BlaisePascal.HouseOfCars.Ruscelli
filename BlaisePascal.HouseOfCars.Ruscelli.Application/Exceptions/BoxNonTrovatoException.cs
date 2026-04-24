namespace BlaisePascal.HouseOfCars.Ruscelli.Application.Exceptions;

public sealed class BoxNonTrovatoException(string posizione)
    : Exception($"Nessun box trovato alla posizione: {posizione}");