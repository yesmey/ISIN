# ISIN (International Securities Identification Number)
Requires .NET 8

> The ISIN code is a 12-character alphanumeric code that serves for uniform identification of a security through normalization of the assigned National Number, where one exists, at trading and settlement. 

Library to parse/format ISIN codes following the ISO 6166 standard.

Implemented as a non-allocating struct that can live on the stack.

## Usage

```chsarp
Isin isin = Isin.Parse("US0378331005");

string cc        = isin.CountryCode // "US"
string basicCode = isin.BasicCode   // "037833100"
int checkDigit   = isin.CheckDigit  // 5
```

```chsarp
if (Isin.TryParse(inputString, out var isin))
{
    ...
}
```

## Benchmark

**Isin.Benchmark:**
| Method           | Mean      | Error     | StdDev    | Gen0   | Allocated |
|----------------- |----------:|----------:|----------:|-------:|----------:|
| Isin_Parse       | 26.269 ns | 0.0949 ns | 0.0888 ns |      - |         - |
| Isin_TryParse    | 27.478 ns | 0.1148 ns | 0.1018 ns |      - |         - |
| Isin_ToString    |  9.533 ns | 0.1451 ns | 0.1357 ns | 0.0010 |      48 B |
| Isin_BasicCode   |  9.028 ns | 0.2238 ns | 0.2198 ns | 0.0008 |      40 B |
| Isin_CountryCode |  8.739 ns | 0.2169 ns | 0.2498 ns | 0.0006 |      32 B |
