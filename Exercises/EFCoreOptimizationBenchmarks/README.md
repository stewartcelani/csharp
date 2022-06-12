After reading [Fast and Memory Efficient Querying in Entity Framework](https://medium.com/codex/fast-and-memory-efficient-querying-in-entity-framework-ebf906d9e6cb) on Medium I wanted to quantify the performance of some of the advice given.

Benchmark details:
- Benchmark runner: .NET core 6 console app running in linux via Docker container
- Database: Microsoft SQL Server running in linux via separate Docker container
- Queries were against a 20 column Users table with 10k rows

|                         Method |      Mean |     Error |    StdDev |     Gen 0 |     Gen 1 |    Allocated |
|------------------------------- |----------:|----------:|----------:|----------:|----------:|-------------:|
|             SelectSingleColumn |  24.01 ms |  0.652 ms |  1.859 ms |  718.7500 |  218.7500 |  5,925,830 B |
| SelectSingleColumnAsNoTracking |  30.40 ms |  1.949 ms |  5.748 ms |  733.3333 |  200.0000 |  5,927,284 B |
|   SelectTwoColumnsAsNoTracking |  37.60 ms |  2.386 ms |  7.036 ms |  727.2727 |  181.8182 |  6,248,751 B |
|               SelectTwoColumns |  38.70 ms |  2.409 ms |  7.102 ms |  733.3333 |  266.6667 |  6,247,418 B |
|             SelectThreeColumns |  52.87 ms |  2.686 ms |  7.920 ms |  900.0000 |  300.0000 |  7,449,193 B |
| SelectThreeColumnsAsNoTracking |  53.78 ms |  3.143 ms |  9.268 ms |  875.0000 |  250.0000 |  7,449,673 B |
|               SelectAllColumns |  79.45 ms |  1.672 ms |  4.903 ms |  714.2857 |         - |  6,044,914 B |
|   SelectAllColumnsAsNoTracking | 249.47 ms | 12.204 ms | 35.983 ms | 2333.3333 | 1000.0000 | 20,728,608 B |

Running docker-compose.yml will automatically apply migrations, seed 10k users into the SQL database and then run the benchmarks.