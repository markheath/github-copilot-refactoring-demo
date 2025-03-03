
To run the benchmarks, navigate to the benchmark project directory

```sh
cd GloboticketWeb.Benchmarks
```

Then run benchmarks in Release mode

```sh
dotnet run -c Release
```

Example results:

| Method                   | Mean       | Error     | StdDev    | Median     |RRatio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------- |-----------:|----------:|----------:|-----------:|------:|--------:|-------:|----------:|------------:|
| Original_Simple          |  39.445 ns | 1.1498 ns | 3.3357 ns |  38.948 ns |  1.01 |    0.12 | 0.0095 |     120 B |        1.00 |
| Optimized_Simple         |   2.743 ns | 0.1509 ns | 0.4353 ns |   2.562 ns |  0.07 |    0.01 |      - |         - |        0.00 |
| Original_WithCommas      | 151.019 ns | 2.8739 ns | 2.8226 ns | 150.633 ns |  3.85 |    0.32 | 0.0381 |     480 B |        4.00 |
| Optimized_WithCommas     | 144.743 ns | 2.8114 ns | 2.3476 ns | 144.149 ns |  3.69 |    0.30 | 0.0381 |     480 B |        4.00 |
| Original_WithWhitespace  |  55.628 ns | 1.1778 ns | 2.2409 ns |  54.798 ns |  1.42 |    0.13 | 0.0140 |     176 B |        1.47 |
| Optimized_WithWhitespace |  51.290 ns | 1.1033 ns | 2.4903 ns |  50.800 ns |  1.31 |    0.12 | 0.0121 |     152 B |        1.27 |